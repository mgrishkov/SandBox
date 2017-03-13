using System;
using System.Collections.Concurrent;
using System.Linq;
using RabbitMQ.Client;

namespace Sample01
{
    public class ConnectionPool : IDisposable
    {
        private const Int32 MAX_BAG_SIZE = 100;

        private static readonly Object _locker = new Object();

        private static readonly ConcurrentDictionary<String, ConcurrentBag<IConnection>> _pool = new ConcurrentDictionary<String, ConcurrentBag<IConnection>>();
        private static readonly ConcurrentDictionary<String, Int32> _liveConnections = new ConcurrentDictionary<String, Int32>(); 

        public static IConnection Take(string uri)
        {
            lock (_locker)
            {
                if (!_pool.TryGetValue(uri, out ConcurrentBag<IConnection> bag))
                {
                    bag = CreateBag(uri);
                    _pool.TryAdd(uri, bag);
                }

                if (bag.IsEmpty)
                {
                    if(_liveConnections.TryGetValue(uri, out int liveConnections))
                        throw new Exception($"Unable to get live connections for {uri}");

                    if (liveConnections > MAX_BAG_SIZE)
                        throw new OverflowException($"Too many connections for {uri}. Try later.");

                    _liveConnections.AddOrUpdate(uri, 1, (k, v) => v + 1);
                    return CreateConnection(uri);
                }

                bag.TryTake(out IConnection result);

                return result;
            }
        }

        public static void Return(string uri, IConnection connection)
        {
            if (!connection.IsOpen)
            {
                connection.Dispose();
                return;
            }

            lock (_locker)
            {
                if (!_pool.TryGetValue(uri, out ConcurrentBag<IConnection> bag) && bag == null)
                    throw new Exception("Unable to get connection bag");

                _liveConnections.AddOrUpdate(uri, 0, (k, v) => v - 1);
                bag.Add(connection);
            }

        }

        public static void Execute(string uri, Action<IConnection> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var connection = ConnectionPool.Take(uri);

            try
            {
                action.Invoke(connection);
            }
            finally
            {
                ConnectionPool.Return(uri, connection);
            }
        }

        public static void Execute(string uri, Action<IModel> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var connection = ConnectionPool.Take(uri);

            try
            {
                using (var channel = connection.CreateModel())
                {
                    action.Invoke(channel);
                }
            }
            finally
            {
                ConnectionPool.Return(uri, connection);
            }
        }

        public static BasicGetResult ExecuteAndReturn(string uri, Func<IModel, BasicGetResult> function)
        {
            if (function == null)
                throw new ArgumentNullException(nameof(function));

            var connection = ConnectionPool.Take(uri);

            try
            {
                using (var channel = connection.CreateModel())
                {
                    return function.Invoke(channel);
                }
            }
            finally
            {
                ConnectionPool.Return(uri, connection);
            }
        }

        private static ConcurrentBag<IConnection> CreateBag(string uri)
        {
            _liveConnections.TryAdd(uri, 1);
            return new ConcurrentBag<IConnection> { CreateConnection(uri) };
        }

        private static IConnection CreateConnection(string uri)
        {
            var factory = new ConnectionFactory() { Uri = uri };
            return factory.CreateConnection();
        }


        #region IDisposable Support
        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue)
                return;

            if (!disposing)
                return;
            
            lock (_locker)
            {
                if (!_pool.Any())
                    return;

                foreach (var p in _pool)
                {
                    foreach (var c in p.Value)
                    {
                        if (c.IsOpen)
                            c.Close();

                        c.Dispose();
                    }
                }
            }

            _disposedValue = true;
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
