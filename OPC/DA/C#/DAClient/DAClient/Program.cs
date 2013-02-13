using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPC.Common;
using OPC.Data;
using OPC.Data.Interface;
using System.Runtime.InteropServices;

namespace DAClient
{
    class Program
    {

        public class OPCClientItem
        {
            public enum EAccessRight { ReadOnly, ReadAndWrite }

            public int ClientHanle { get; set; }
            public int ServerHandle { get; set; }
            public string Name { get; set; }
            public EAccessRight AccessRight { get; set; }
        }

        static void Main(string[] args)
        {
            /*create array of readable Tags*/
            var Tags = new List<OPCClientItem>();
            Tags.Add(new OPCClientItem() {
                 Name = ".test",
                 ClientHanle = 1
            });
            
            OpcServer server = new OpcServer();
            try
            {
                int transactionID = new Random().Next(1024, 65535);
                int cancelID = 0;
                int updateRate = 1000;

                /*connect to the OPC Server and check it's state*/
                server.Connect("Matrikon.OPC.Simulation.1");
                var serverStatus = new SERVERSTATUS();
                server.GetStatus(out serverStatus);
                if (serverStatus.eServerState == OPCSERVERSTATE.OPC_STATUS_RUNNING)
                {
                    /*create group of items*/
                    OpcGroup group = server.AddGroup("Group1", true, updateRate);
                    group.ReadCompleted += group_ReadCompleted;
                    List<OPCItemDef> items = new List<OPCItemDef>();
                    Tags.ToList()
                        .ForEach(x => items.Add(new OPCItemDef(x.Name, true, x.ClientHanle, VarEnum.VT_EMPTY)));

                    /* add items and collect their attributes*/
                    OPCItemResult[] itemAddResults = null;
                    group.AddItems(items.ToArray(), out itemAddResults);
                    for(int i = 0; i < itemAddResults.Length; i++)
                    {
                        OPCItemResult itemResult = itemAddResults[i];
                        OPCClientItem tag = Tags[i];
                        tag.ServerHandle = itemResult.HandleServer;
                        tag.AccessRight = (itemResult.AccessRights == OPCACCESSRIGHTS.OPC_READABLE) ? OPCClientItem.EAccessRight.ReadOnly : OPCClientItem.EAccessRight.ReadAndWrite;
                    };
                    
                    /*Refresh items in group*/
                    // group.Refresh2(OPCDATASOURCE.OPC_DS_DEVICE, transactionID, out cancelID);

                    /*Async read data for the group items*/
                    int[] serverHandles = new int[Tags.Count];
                    for(int i = 0; i < Tags.Count; i++)
                    {
                        serverHandles[i] = Tags[i].ServerHandle;
                    };
                    OPCItemState[] itemsStateResult = null;
                    /*sync read*/
                    group.Read(OPCDATASOURCE.OPC_DS_DEVICE, serverHandles, out itemsStateResult);
                    Console.WriteLine("Sync read:");
                    for (int i = 0; i < itemsStateResult.Length; i++)
                    {
                        OPCItemState itemResult = itemsStateResult[i];
                        Console.WriteLine(" -> item:{0}; value:{1}; timestamp{2}; qualituy:{3}", Tags[i].Name, itemResult.DataValue.ToString(), itemResult.TimeStamp, itemResult.Quality); 
                    };

                    /*sync write*/
                    object[] values = new object[Tags.Count];
                    int[] resultErrors = new int[Tags.Count];
                    values[0] = (object)256;
                    group.Write(serverHandles, values, out resultErrors);

                    /*async read*/
                    group.Read(serverHandles, transactionID, out cancelID, out resultErrors);

                    /*wait for a while befor remove group to process async event*/
                    System.Threading.Thread.Sleep(3000);

                    /*the group must be removed !!! */
                    group.Remove(true);
                };
            }
            finally
            {
                server.Disconnect();
                server = null;
                GC.Collect();
            };
            Console.ReadKey();
        }

        static void group_ReadCompleted(object sender, ReadCompleteEventArgs e)
        {
            Console.WriteLine("Async read:");
            for (int i = 0; i < e.sts.Length; i++)
            {
                OPCItemState itemResult = e.sts[i];
                Console.WriteLine(" -> item:{0}; value:{1}; timestamp{2}; qualituy:{3}", "n/a", itemResult.DataValue.ToString(), itemResult.TimeStamp, itemResult.Quality);
            };
        }
    }
}
