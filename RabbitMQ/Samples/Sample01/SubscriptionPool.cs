using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;

namespace Sample01
{
    /// <summary>
    /// Используется как  хранилище для подписок, чтобы при завершении метода Subscribe подписка , подключение и канал оставались живыми
    /// </summary>
    public class SubscriptionPool
    {
        private static readonly List<Subscription> _pool = new List<Subscription>();

        public static Subscription Create(IModel channel, string queue)
        {
            var s  = new Subscription(channel, queue, false);
            _pool.Add(s);
            return s;
        }

        public static void Remove(Subscription s)
        {
            s.Close();
            _pool.RemoveAll(x => x == s);
            s = null;
        }
    }
}
