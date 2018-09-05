using System;
using InterSystems.Data.IRISClient;
using InterSystems.Data.IRISClient.ADO;
using InterSystems.XEP;
using System.Data.Common;

namespace myApp
{
    class multiplayTask1
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
                
                // Create IRIS native
                IRIS irisNative = IRIS.CreateIRIS((IRISADOConnection) xepPersister.GetAdoNetConnection());               
                 xepEvent.Close();
                xepPersister.Close();
            
            } catch (Exception e) { 
                Console.WriteLine("Interactive prompt failed:\n" + e); 
            }
        }
    }
}