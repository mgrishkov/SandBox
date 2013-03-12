using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QueueProcessor
{
    class Program
    {
        private class Request : IRequest
        {
            private static Random random = new Random();

            public int ID { get; set; }
            public void Execute()
            {
                
                int randomNumber = random.Next(1000, 10000);

                Thread.Sleep(randomNumber);
                Console.WriteLine("-- Request #{0} was processed by thread #{1} within task #{2}", ID, Thread.CurrentThread.ManagedThreadId, Task.CurrentId);
            }
        }

        static void Main(string[] args)
        {
            using (var requests = new QueueProcessor<Request>(5))
            {
                requests.RequesExecuting += requests_RequesExecuting;
                requests.QueueProcessed += requests_QueueProcessed;
                requests.Add(new Request() { ID = 101 });
                requests.Add(new Request() { ID = 102 });
                requests.Add(new Request() { ID = 103 });
                requests.Add(new Request() { ID = 104 });
                requests.Add(new Request() { ID = 105 });
                requests.Add(new Request() { ID = 106 });
                requests.Add(new Request() { ID = 107 });
                requests.Add(new Request() { ID = 108 });
                requests.Add(new Request() { ID = 109 });
                requests.Add(new Request() { ID = 110 });
                requests.Add(new Request() { ID = 111 });
                requests.Add(new Request() { ID = 112 });
                requests.Add(new Request() { ID = 113 });
                requests.Add(new Request() { ID = 114 });
                requests.Add(new Request() { ID = 115 });
                requests.Add(new Request() { ID = 116 });
                requests.Add(new Request() { ID = 117 });
                requests.Add(new Request() { ID = 118 });
                requests.Add(new Request() { ID = 119 });
                requests.Add(new Request() { ID = 120 });
                requests.Add(new Request() { ID = 121 });
                requests.Add(new Request() { ID = 122 });
                requests.Add(new Request() { ID = 123 });
                requests.Add(new Request() { ID = 124 });
                requests.Add(new Request() { ID = 125 });
                requests.Add(new Request() { ID = 126 });
                requests.Add(new Request() { ID = 127 });
                requests.Add(new Request() { ID = 128 });
                requests.Add(new Request() { ID = 129 });
                requests.Add(new Request() { ID = 130 });
                requests.Add(new Request() { ID = 131 });
                requests.Add(new Request() { ID = 132 });

                requests.Process();
            };
            Console.ReadKey();
        }

        static void requests_QueueProcessed(object sender, EventArgs e)
        {
            Console.WriteLine("Queue was processed");
        }

        static void requests_RequesExecuting(object sender, QueueProcessor<Program.Request>.RequesExecutingEventArgs<Program.Request> e)
        {
            Console.WriteLine("Request #{0} change state to {1}", e.Request.ID, e.State);
        }

        
    }
}
