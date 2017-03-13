using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using SmartService.Common.Extensions;

namespace Sample01
{
    public class AmqpHelper
    {
        private const int TIMEOUT = 200;

        public String ConnectionUri { get; set; }
        public Encoding TextEncoding { get; set; }

        public event EventHandler OnTakeConnection;
        public event EventHandler OnReturnConnection;

        public AmqpHelper()
        {
            TextEncoding = Encoding.UTF8;
        }

        public AmqpHelper(string conectionUri)
        {
            ConnectionUri = conectionUri;
        }

        private IConnection TakeConnection()
        {
            OnTakeConnection?.Invoke(typeof(AmqpHelper), new EventArgs());
            return ConnectionPool.Take(ConnectionUri);
        }

        private void ReturnConnection(IConnection connection)
        {
            OnReturnConnection?.Invoke(typeof(AmqpHelper), new EventArgs());
            ConnectionPool.Return(ConnectionUri, connection);
        }

        protected void Execute(Action<IModel> action)
        {
            ConnectionPool.Execute(ConnectionUri, action);
        }

        protected Task ExecuteAsync(Action<IModel> action)
        {
            return Task.Factory.StartNew(() => Execute(action) );
        }

        protected void Push(string exchange, string routingKey, object payload, IBasicProperties properties = null)
        {
            if (String.IsNullOrWhiteSpace(exchange))
                throw new ArgumentNullException(nameof(exchange));

            if (String.IsNullOrWhiteSpace(routingKey))
                throw new ArgumentNullException(nameof(routingKey));

            ConnectionPool.Execute(ConnectionUri, (IModel channel) =>
            {
                var props = properties ?? GetDefaultProperties(channel);

                var payloadJson  = payload != null ? payload.ToJson() : String.Empty;
                var messageBytes = TextEncoding.GetBytes(payloadJson);

                channel.BasicPublish(exchange, routingKey, props, messageBytes);
            });
        }

        protected Task PushAsync(string exchange, string routingKey, object payload, IBasicProperties properties = null)
        {
            return Task.Factory.StartNew(() => Push(exchange, routingKey, payload, properties));
        }

        protected (string Body, IBasicProperties Props) Pull(string queue)
        {
            if (String.IsNullOrWhiteSpace(queue))
                throw new ArgumentNullException(nameof(queue));

            var result = ConnectionPool.ExecuteAndReturn(ConnectionUri, (IModel channel) => channel.BasicGet(queue, true));

            if (result == null)
                return (null, null);

            var payLoad = result.Body != null ? TextEncoding.GetString(result.Body) : null;

            return (payLoad, result.BasicProperties);
        }

        protected async Task<(string Body, IBasicProperties Props)> PullAsync(string queue)
        {
            return await Task.FromResult(Pull(queue));
        }

        protected CancellationTokenSource Subscribe(string queue, Func<BasicDeliverEventArgs, bool> function, CancellationTokenSource tokenSource = null)
        {
            if (String.IsNullOrWhiteSpace(queue))
                throw new ArgumentNullException(nameof(queue));

            var cancellationTokenSource = tokenSource ?? new CancellationTokenSource();

            if (cancellationTokenSource.IsCancellationRequested)
                throw new OperationCanceledException("Subscription has been cancelled by token", cancellationTokenSource.Token);


            var connection   = ConnectionPool.Take(ConnectionUri);
            var channel      = connection.CreateModel();

            var subscription = SubscriptionPool.Create(channel, queue);

            Task.Factory.StartNew(() =>
            {
                var fn = function;
                var ct = cancellationTokenSource;
                var s  = subscription;

                while (!ct.IsCancellationRequested)
                {
                    if (subscription == null || !subscription.Next(TIMEOUT, out BasicDeliverEventArgs args))
                        continue;

                    if (args == null)
                    {
                        cancellationTokenSource.Cancel();
                        continue;
                    }

                    if (fn(args))
                        s.Ack(args);
                }
            }, cancellationTokenSource.Token)
            .ContinueWith(t => /* метод будет выполнен в результате срабатывания cancellationTokenSource - нельзя делаеть его зависимым от токена */
            {
                    var uri = ConnectionUri;
                    var c   = connection;
                    var ch  = channel;
                    var s   = subscription;

                    SubscriptionPool.Remove(s);
                    ch.Close();

                    ConnectionPool.Return(uri, c);
                });

            return cancellationTokenSource;
        }


        private static IBasicProperties GetDefaultProperties(IModel channel)
        {
            var properties = channel.CreateBasicProperties();
            properties.ContentType = "application/json";
            properties.DeliveryMode = 2 /* persistent */;
            properties.Timestamp = new AmqpTimestamp(DateTime.Now.ToUnixEpochDate());
            return properties;
        }

    }
}
