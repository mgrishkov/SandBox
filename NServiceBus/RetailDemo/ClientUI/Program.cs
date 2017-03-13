using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace ClientUI
{
    class Program
    {
        private const string CLIENT = "ClientUI";
        private const string ERROR  = "ERROR";


        static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Console.Title = CLIENT;

            //Confiure
            var endpointConfiguration = new EndpointConfiguration(CLIENT);
            endpointConfiguration.UseTransport<MsmqTransport>();
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo(ERROR);
            endpointConfiguration.EnableInstallers();

            //startup
            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
