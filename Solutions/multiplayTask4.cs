using System;
using InterSystems.Data.IRISClient;
using InterSystems.Data.IRISClient.ADO;
using InterSystems.XEP;
using System.Collections.Generic;

namespace myApp
{
    class multiplayTask4
    { 
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            String host = "104.197.75.13";
            int port = 21652;
            String username = "SuperUser";
            String password = "SYS";
            String Namespace = "USER";
            String className = "myApp.StockInfo";
            
            try {
                // Connect to database using EventPersister
                EventPersister xepPersister = PersisterFactory.CreatePersister();
                xepPersister.Connect(host, port, Namespace, username, password);
                Console.WriteLine("Connected to InterSystems IRIS.");
                xepPersister.DeleteExtent(className);   // remove old test data
                xepPersister.ImportSchema(className);   // import flat schema
            
                // Create Event
                Event xepEvent = xepPersister.GetEvent(className);
                IRISADOConnection connection = (IRISADOConnection) xepPersister.GetAdoNetConnection();
                IRIS native = IRIS.CreateIRIS(connection);

                String sql = "SELECT distinct name FROM demo.stock";
                IRISCommand cmd = new IRISCommand(sql, connection);
                IRISDataReader reader = cmd.ExecuteReader();

                var array = new List<StockInfo>();
                while(reader.Read()){
                    StockInfo stock = new StockInfo();
                    stock.name = (string) reader[reader.GetOrdinal("Name")];
				    Console.WriteLine("created stockinfo array.");
				
				    //generate mission and founder names (Native API)
				    stock.founder = native.ClassMethodString("%PopulateUtils", "Name");
				    stock.mission = native.ClassMethodString("%PopulateUtils", "Mission");
                    Console.WriteLine("Adding object with name " + stock.name + " founder " + stock.founder + " and mission " + stock.mission);
				    array.Add(stock);
                }
                string combindedString = string.Join(",", array);
                xepEvent.Store(combindedString);

                xepEvent.Close();
                xepPersister.Close();

            
            } catch (Exception e) { 
                Console.WriteLine("Interactive prompt failed:\n" + e); 
            }
        }
    }
}