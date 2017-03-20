using System;   
using System.Net.Http;
using System.Threading;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Sockets.Client;
using Microsoft.Extensions.Logging;


namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(3000);

            var baseUrl = "http://localhost:5000/hubs/testHub";

            var loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole()
                .AddDebug();

            var logger = loggerFactory.CreateLogger<Program>();

            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, a) =>
            {
                a.Cancel = true;
                logger.LogInformation("Stopping loops...");
                cts.Cancel();
            };

            using (var httpClient = new HttpClient(new LoggingMessageHandler(loggerFactory, new HttpClientHandler())))
            {
                var transport  = new LongPollingTransport(httpClient, loggerFactory);
                var connection = new HubConnection(new Uri(baseUrl), new JsonNetInvocationAdapter(), loggerFactory);

                try
                {
                    logger.LogInformation("Connecting to {0}", baseUrl);
                    connection.StartAsync(transport, httpClient).GetAwaiter();
                    logger.LogInformation("Connected to {0}", baseUrl);

                    connection.On("echo", new[] { typeof(string) }, a =>
                    {
                        var message = (string)a[0];
                        logger.LogInformation("RECEIVED: " + message);
                    });
                    
                    var result = connection.Invoke<string>("echo").GetAwaiter();

                    while (!cts.Token.IsCancellationRequested)
                    {
                        //var line = Console.ReadLine();
                        //logger.LogInformation("Sending: {0}", line);
                        //
                        //connection.Invoke<object>("Send", line).GetAwaiter();
                    }
                }
                finally
                {
                    connection.DisposeAsync().GetAwaiter();
                }

            }

        }
    }
}