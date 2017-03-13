using Plivo.API;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            string auth_id = "XXX";  // obtained from Plivo account dashboard
            string auth_token = "YYY";  // obtained from Plivo account dashboard

            // Creating the Plivo Client
            var plivo = new RestAPI(auth_id, auth_token);

            IRestResponse<MessageResponse> resp = plivo.send_message(new Dictionary<string, string>()
            {
                { "src", "Sample" },
                { "dst", "+79219784672" },
                { "text", "Hello, how are you?" }
            });

        }
    }
}
