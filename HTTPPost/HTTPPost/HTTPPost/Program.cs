using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HTTPPost
{
    class Program
    {
        static void Main(string[] args)
        {
            /*const string l_content = @"<Request>
	<Project>1074</Project>
    <DateTime>2012-04-25 11:25:57</DateTime>
    <Sign>3e84c5b2a16d5b1d835923b05967bfe93d8de8bf97b459544ed21b475619975d40344755d06f82757205d1c3393bf8f339ec121232c2d3edb4fc42d8d4de2699eb1daca6e3793b2844f9c692a94692ece2a70a4276fa19a702ecc41636f2deafcaedf23b2e9e1057760e0de891f4ccfef72a676480808cf0d9216530bc5bcc95</Sign>
    <Balance/>
</Request>";
            const string l_content = @"<Request>
    <Project>1074</Project>
    <DateTime>2012-04-25 11:08:12</DateTime>
    <Sign>634cf6baf61f658ed501df3cdd0db2b2873cd35151097fc7e789827a317a24fd3c87b8c4fbe56cb5b55ea66e7f1c3ffccea600aed61ead1e59f43db8e76bf37992fd8f2664407bfd5d0cd3c2bd92cc6f664662ff7fff0f77ff688ba4b8467bb6b815dfa04c88efc2423b3731eb088bd15c7e2275e10518f138c169bd9fb1a696</Sign>
    <Payment>
		<OrderId>242</OrderId> 
		<PurseTo>c485e3b5b439e02ba6f8dfc573955aa5ccf9d0460c5ad890f0bd67aea4a1cbc4c03b63ef589c2b701a77c785b1987afdf282a5ed121acbac06fb5ed14b58fc98d5853ee7463b229146337d2a5072bc20c33119212a339df68e96cadf4766184afc72e1b5f86817098ca9098aecaa43bbbf91f9a63832c077ed8301effcbc15e3</PurseTo> 
		<PaySystem>bc</PaySystem> 
		<Amount>1</Amount> 
		<Description>Test Payment To BC</Description>
    </Payment>
</Request>";
            Post(l_content);*/
            Post("Z156761335728");
            Console.ReadKey();
        }
        /*
        public static void Post(string Data)
        {
            System.Net.WebRequest reqPOST = System.Net.WebRequest.Create(@"http://www.onlinedengi.ru/dev/conclusion.php");
            reqPOST.Method = "POST"; 
            reqPOST.Timeout = 120000;
            reqPOST.ContentType = "application/x-www-form-urlencoded";
            byte[] sentData = Encoding.GetEncoding(1251).GetBytes(Data);
            reqPOST.ContentLength = sentData.Length;

            System.IO.Stream sendStream = reqPOST.GetRequestStream();
            sendStream.Write(sentData, 0, sentData.Length);
            sendStream.Close();
            
            System.Net.WebResponse respPOST = reqPOST.GetResponse();
            System.IO.Stream l_rcv_stream = respPOST.GetResponseStream();
                
            System.IO.StreamReader l_sreader = new System.IO.StreamReader(l_rcv_stream, Encoding.GetEncoding(1251));
            string l_response = l_sreader.ReadToEnd();
            l_rcv_stream.Close();
        }*/
        public static void Post(string WMZ)
        {
            string l_url = String.Format(@"http://passport.webmoney.ru/asp/VerifyWMID.asp?wmid={0}", WMZ);
            System.Net.WebRequest l_req = System.Net.WebRequest.Create(l_url);
            l_req.Method = "POST";
            l_req.Timeout = 120000;
            l_req.ContentType = "application/x-www-form-urlencoded";

            byte[] sentData = Encoding.GetEncoding(1251).GetBytes(WMZ);
            l_req.ContentLength = sentData.Length;

            System.IO.Stream sendStream = l_req.GetRequestStream();
            sendStream.Write(sentData, 0, sentData.Length);
            sendStream.Close();

            System.Net.WebResponse l_resp = l_req.GetResponse();
            string l_resp_q = l_resp.ResponseUri.Query;
            if (String.IsNullOrEmpty(l_resp_q))
            {
                string l_wmid = l_resp.ResponseUri.Query.Substring(6, 12);
            };
                        
        }

    }
}
