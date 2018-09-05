using System;
using InterSystems.Data.IRISClient;
using InterSystems.Data.IRISClient.ADO;

namespace myApp
{
    class nativeAPIplaystocksTask1
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
			//Making connection
			IRISConnection connection = new IRISConnection();
            connection.ConnectionString = "Server = " + host + "; Port = " + port + "; Namespace = " +
                                    Namespace + "; Password = " + password + "; User ID = " + username;
            connection.Open();
            Console.WriteLine("Connected to InterSystems IRIS.");

					
			IRIS irisNative = IRIS.CreateIRIS(connection);
					
			bool always = true;
			
			while (always) {
				Console.WriteLine("1. Test");
				Console.WriteLine("2. Store stock data");
				Console.WriteLine("3. View stock data");
				Console.WriteLine("4. Generate Trades");
				Console.WriteLine("5. Quit");
				Console.WriteLine("What would you like to do? ");
				String option = Console.ReadLine();
				switch (option) {
				case "1":
					SetTestGlobal(irisNative);
					break;
				case "2":
					Console.WriteLine("TO DO: Stock stock data");
					break;
				case "3":
					Console.WriteLine("TO DO: View stock data");
					break;
				case "4":
					Console.WriteLine("TO DO: Generate trades");
					break;
				case "5":
					Console.WriteLine("Exited.");
					always = false;
					break;
				default: 
					Console.WriteLine("Invalid option. Try again!");
					break;
				}				
			}				
			irisNative.Close();
	
		}
		catch (Exception e) 
		{
			Console.WriteLine("Error - Exception thrown: " + e);
		} 
	}
        public static void SetTestGlobal(IRIS irisNative)
        {
            //Write to a test global
            irisNative.Set(8888, "^testglobal", "1");
            int globalValue = (int) irisNative.GetInt32("^testglobal", "1");
            Console.WriteLine("The value of ^testglobal(1) is " + globalValue);
		}
    }
}