using System;
using InterSystems.Data.IRISClient;

namespace myApp
{
    class adonetplaystocksTask1
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            String host = "104.197.75.13";
            int port = 21652;
            String username = "SuperUser";
            String password = "SYS";
            String Namespace = "USER";
            
            try {
                IRISADOConnection connect = new IRISADOConnection();
                connect.ConnectionString = "Server = " + host + "; Port = " + port + "; Namespace =  " + Namespace + "; Password = " + password + "; User ID = " + username;
                connect.Open();
                Console.WriteLine("Connected to InterSystems IRIS.");
            } catch (Exception e) { 
                Console.WriteLine("Interactive prompt failed:\n" + e); 
            }
        }
    }
}