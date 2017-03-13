using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Sample01
{
    public class NotificationHelper : AmqpHelper
    {
        private const string EXCHANGE_NAME     = "notifications";
        private const string EMAIL_ROUTING_KEY = "email";
        private const string SMS_ROUTING_KEY   = "sms";
        
        public void Configure()
        {
            Execute(channel =>
            {
                channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Direct, true);

                foreach (var rk in new[] { EMAIL_ROUTING_KEY, SMS_ROUTING_KEY })
                {
                    var queueName = $"{EXCHANGE_NAME}-{rk}";
                    channel.QueueDeclare(queueName, true, false, false);
                    channel.QueueBind(queueName, EXCHANGE_NAME, rk);
                }
            });
        }

        public void PushEmail(object payload)
        {
            Push(EXCHANGE_NAME, EMAIL_ROUTING_KEY, payload);
        }

        public void PushSms(object payload)
        {
            Push(EXCHANGE_NAME, SMS_ROUTING_KEY, payload);
        }

        public string PullEmail()
        {
            var data = Pull($"{EXCHANGE_NAME}-{EMAIL_ROUTING_KEY}");
            return data.Body;
        }

        public string PullSms()
        {
            var data = Pull($"{EXCHANGE_NAME}-{SMS_ROUTING_KEY}");
            return data.Body;
        }

        public CancellationTokenSource SubscribeEmail(Func<BasicDeliverEventArgs, bool> function, CancellationTokenSource tokenSource = null)
        {
            return Subscribe($"{EXCHANGE_NAME}-{EMAIL_ROUTING_KEY}", function, tokenSource);
        }
    }
}
