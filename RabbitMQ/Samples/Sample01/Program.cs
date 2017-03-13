using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using SmartService.Common.Extensions;

namespace Sample01
{
    class Program
    {
        private const string CONNECTION_URI = "amqp://ApiUser:c8kNQhLn24jxQT2g@app-hbx01-linx:5672";

        static void Main(string[] args)
        {
            var helper = new NotificationHelper() { ConnectionUri = CONNECTION_URI };
            helper.Configure();

            if (args == null || !args.Any())
                return;

            if (args[0] == "--push")
            {
                if (args[1] == "-- parallel")
                {
                    Parallel.ForEach(Enumerable.Range(0, 9), i =>
                    {
                        helper.PushEmail(new {recepient = "grishkov.mn@gmail.com", message = $"Parallel test message #{i}"});
                    });
                }
                else
                {
                    Enumerable.Range(0, 100).ToList().ForEach(i =>
                    {
                        helper.PushEmail(new { recepient = "grishkov.mn@gmail.com", message = $"Test message #{i}" });
                    });
                }
            }

            if (args[0] == "--pull")
            {
                var message = helper.PullEmail();
                Console.WriteLine(message);
            }

            if (args[0] == "--subscribe")
            {
                var ct = helper.SubscribeEmail((e) =>
                {
                    Console.WriteLine(helper.TextEncoding.GetString(e.Body));
                    Thread.Sleep(100);
                    return true;
                });
                
                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(10000);
                    Console.WriteLine($"Cancel subscription");
                    ct.Cancel();
                }, ct.Token);

                while(true) { }
            }
        }

        

        
        
    }
}