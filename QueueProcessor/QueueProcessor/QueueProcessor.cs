using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace QueueProcessor
{
    public interface IRequest
    {
        void Execute();
    }
    
    public class QueueProcessor<T> : IDisposable where T : IRequest
    {
        public class RequesExecutingEventArgs<T> : EventArgs
        {
            public enum EStatus
            {
                Executing = 1,
                Comleted = 2,
                Failed = 3
            }

            public T Request { get; set; }
            public EStatus State { get; set; }
            public string Message { get; set; }
        }

        public event EventHandler<RequesExecutingEventArgs<T>> RequesExecuting;
        public event EventHandler QueueProcessed;

        private BlockingCollection<T> _queue;
        private readonly int _maxConcurrent;
        private List<Task> _tasks;
        private volatile bool _locker;

        public QueueProcessor(int MaxConcurrent)
        {
            _maxConcurrent = MaxConcurrent;
            _queue = new BlockingCollection<T>();
            _tasks = new List<Task>();
        }
        public void Process()
        {
            _queue.CompleteAdding();
            for (int i = 0; i < _maxConcurrent; i++)
            {
                if (!_queue.IsCompleted)
                {
                    var task = Task.Factory.StartNew(() => processRequest());
                    _tasks.Add(task);
                };
            };
            Task.WaitAll(_tasks.ToArray());
            OnQueueProcessed();
        }
        private void processRequest()
        {
            if (!_queue.IsCompleted)
            {
                T request = _queue.Take();
                Execute(request);
                if (!_queue.IsCompleted)
                {
                    var task = Task.Factory.StartNew(() => processRequest());
                    task.Wait();
                };
            };
        }
        private void Execute(T request)
        {
            var eventArgs = new RequesExecutingEventArgs<T>()
            {
                Request = request,
                State = RequesExecutingEventArgs<T>.EStatus.Executing,
                Message = String.Empty
            };
            OnRequesExecuting(eventArgs);
            try
            {
                request.Execute();
                eventArgs.State = RequesExecutingEventArgs<T>.EStatus.Comleted;
            }
            catch (Exception e)
            {
                eventArgs.State = RequesExecutingEventArgs<T>.EStatus.Failed;
                eventArgs.Message = e.Message;
            };
        }
        protected virtual void OnRequesExecuting(RequesExecutingEventArgs<T> e)
        {
            if (RequesExecuting != null)
            {
                RequesExecuting(this, e);
            };
        }
        protected virtual void OnQueueProcessed()
        {
            if (QueueProcessed != null)
            {
                QueueProcessed(this, new EventArgs());
            };
        }
        
        public void Add(T Request)
        {
            _queue.Add(Request);
        }

#region Implement IDispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_queue != null)
                {
                    _queue.Dispose();
                    _queue = null;
                };
                if (_tasks != null)
                {
                    _tasks.ForEach(x => x.Dispose());
                    _tasks.Clear();
                };
            };
        }
        ~QueueProcessor()
        {
            Dispose(false);
        }
#endregion
    }
}
