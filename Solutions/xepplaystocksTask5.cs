﻿using System;
using InterSystems.Data.IRISClient;
using InterSystems.XEP;
using System.Data.SqlClient;
using System.Data;

namespace myApp
{
    class xepplaystocksTask5
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            String host = "104.197.75.13";
            int port = 21652;
            String username = "SuperUser";
            String password = "SYS";
            String Namespace = "USER";
            String className = "myApp.Trade";

            try {
                Trade[] sampleArray = null;

                // Connect to database using EventPersister
                EventPersister xepPersister = PersisterFactory.CreatePersister();
                xepPersister.Connect(host, port, Namespace, username, password);
                Console.WriteLine("Connected to InterSystems IRIS.");
                xepPersister.DeleteExtent(className);   // remove old test data
                xepPersister.ImportSchema(className);   // import flat schema

                // Create Event
                Event xepEvent = xepPersister.GetEvent(className);

            // Starting interactive prompt
			bool always = true;
			while (always) {
				Console.WriteLine("1. Make a trade (do not save)");
				Console.WriteLine("2. Confirm all trades");
				Console.WriteLine("3. Generate and save multiple trades");
				Console.WriteLine("4. Retrieve all trades; show execution statistics");
				Console.WriteLine("5. ADO.NET Comparison - Create and save multiple trades");
				Console.WriteLine("6. Quit");
				Console.WriteLine("What would you like to do? ");

				String option = Console.ReadLine();
				switch (option) {
				case "1":
					//Create trade object
					Console.WriteLine("Stock name: ");
					String name = Console.ReadLine();

					Console.WriteLine("Date (YYYY/MM/DD): ");
					DateTime date;
					if (DateTime.TryParse(Console.ReadLine(), out date)){

					}
					else{
						Console.WriteLine("Invalid date format!");
					}

					Console.WriteLine("Price: ");
					String inputPrice = Console.ReadLine();
                    double price;
                    double.TryParse(inputPrice, out price);
					if (price <= 0){
						Console.WriteLine("Price has to bigger than 0");
						break;
					}

					Console.WriteLine("Number of Shares: ");
					String inputShare = Console.ReadLine();
                    int shares;
                    Int32.TryParse(inputShare, out shares);
					if (shares <= 0){
						Console.WriteLine("Number of Shares has to bigger than 0");
						break;
					}

					Console.WriteLine("Trader name: ");
					String traderName = Console.ReadLine();

					sampleArray = CreateTrade(name, date, price, shares, traderName, sampleArray);
					break;
				case "2":
					//Save trades
					Console.WriteLine("Saving trades.");
					XEPSaveTrades(sampleArray, xepEvent);
					sampleArray = null;
					break;
				case "3":
					Console.WriteLine("How many items do you want to generate? ");
					String inputNumber = Console.ReadLine();
                    int number;
                    Int32.TryParse(inputNumber, out number);
					if (number <= 0){
						Console.WriteLine("Number of items has to bigger than 0");
						break;
					}
					//Get sample generated array to store
					sampleArray = Trade.generateSampleData(number);


					//Save generated trades
					long totalStore = XEPSaveTrades(sampleArray,xepEvent);
					Console.WriteLine("Execution time: " + totalStore + "ms");
					break;
				case "4":
					Console.WriteLine("Fetching all. Please wait...");
					long totalFetch = ViewAll(xepEvent);
					Console.WriteLine("Execution time: " + totalFetch + "ms");
					break;
				case "5":
					Console.WriteLine("How many items to generate using ADO.NET? ");
					String inputNum = Console.ReadLine();
                    int numberADODOTNET;
                    Int32.TryParse(inputNum, out numberADODOTNET);
					if (numberADODOTNET <= 0){
						Console.WriteLine("Number of items has to bigger than 0");
						break;
					}
					//Get sample generated array to store
					sampleArray = Trade.generateSampleData(numberADODOTNET);

					//Save generated trades using ADO.NET

					long totalADODOTNETStore = StoreUsingADODOTNET(xepPersister, sampleArray);
					Console.WriteLine("Execution time: " + totalADODOTNETStore + " ms");
					break;
				case "6":
					Console.WriteLine("Exited.");
					always = false;
					break;
				default:
					Console.WriteLine("Invalid option. Try again!");
					break;
				}
			}
	        xepEvent.Close();
	        xepPersister.Close();
            } catch (Exception e) {
                Console.WriteLine("Interactive prompt failed:\n" + e);
            }
        } // end main

        public static Trade[] CreateTrade(String stockName, DateTime tDate, double price, int shares, String trader, Trade[] sampleArray)
	    {
            Trade sampleObject = new Trade(stockName, tDate, price, shares, trader); //
            Console.WriteLine("New Trade: " + shares + " shares of " + stockName + " purchased on date " + tDate.ToString() + " at price " + price + " by " + trader + ".");

            int currentSize = 0;
            int newSize = 1;
            if (sampleArray != null)
            {
                currentSize = sampleArray.Length;
                newSize = currentSize + 1;
            }

            Trade[] newArray = new Trade[ newSize ];
            for (int i=0; i < currentSize; i++)
            {
                newArray[i] = sampleArray[i];
            }
            newArray[newSize- 1] = sampleObject;
            Console.WriteLine("Added " + stockName + " to the array. Contains " + newSize + " trade(s).");
            return newArray;
	    }

	    public static long XEPSaveTrades(Trade[] sampleArray,Event xepEvent)
	    {
            long startTime = DateTime.Now.Ticks; //To calculate execution time
            xepEvent.Store(sampleArray);
            long endtime = DateTime.Now.Ticks;
            Console.WriteLine("Saved " + sampleArray.Length + " trade(s).");
            return endtime - startTime;
	    }

		public static long StoreUsingADODOTNET(EventPersister persist, Trade[] sampleArray)
		{
			long totalTime = new long();
			long startTime = DateTime.Now.Ticks;
			//Loop through objects to insert
 		try {
             IRISDataAdapter da = new IRISDataAdapter();
				String ClassName = "Demo.Trade";

				IRISADOConnection con = (IRISADOConnection) persist.GetAdoNetConnection();

				String SQL = "select purchaseDate, purchasePrice, stockName from " + ClassName;
				da.SelectCommand = con.CreateCommand();
				da.SelectCommand.CommandText = SQL;

				SQL = "INSERT INTO Demo.Trade (purchaseDate, purchasePrice, stockName) VALUES (?,?,?)";

				IRISCommand cmd = con.CreateCommand();
				cmd.CommandText = SQL;
				da.InsertCommand = cmd;

				IRISParameter date_param = new IRISParameter("purchaseDate", IRISDbType.DateTime);
				cmd.Parameters.Add(date_param);
				date_param.SourceColumn = "purchaseDate";

				IRISParameter price_param = new IRISParameter("purchasePrice", IRISDbType.Double);
				cmd.Parameters.Add(price_param);
				price_param.SourceColumn = "purchasePrice";

				IRISParameter Name_param = new IRISParameter("stockName", IRISDbType.NVarChar);
				cmd.Parameters.Add(Name_param);
				Name_param.SourceColumn = "stockName";

				da.TableMappings.Add("Table", ClassName);

				DataSet ds = new DataSet();
				da.Fill(ds);

				for (int i=0; i < sampleArray.Length; i++)
 				{
					DataRow newRow = ds.Tables[0].NewRow();
					newRow["purchaseDate"] = sampleArray[i].purchaseDate;
					newRow["purchasePrice"] = sampleArray[i].purchasePrice;
					newRow["stockName"] = sampleArray[i].stockName;
					ds.Tables[0].Rows.Add(newRow);
				}


				da.Update(ds);
				Console.WriteLine("Inserted " + sampleArray.Length + " item(s) via ADO.NET successfully.");
				totalTime = DateTime.Now.Ticks - startTime;
			} catch (Exception e) {
				Console.WriteLine("There was a problem storing items using ADO.NET.\n" + e);
			}
			return totalTime/TimeSpan.TicksPerMillisecond;
		}

		public static long ViewAll(Event xepEvent)
        {
			//Create and execute query using EventQuery
			String sqlQuery = "SELECT * FROM MyApp.Trade WHERE purchaseprice > ? ORDER BY stockname, purchaseDate";
			EventQuery<Trade> xepQuery = xepEvent.CreateQuery<Trade>(sqlQuery);
			xepQuery.AddParameter(0);    // find stocks purchased > $0/share (all)
			long startTime = DateTime.Now.Ticks;
			xepQuery.Execute();

			// Iterate through and write names of stocks using EventQueryIterator
			Trade trade = xepQuery.GetNext();
			while (trade != null) {
				Console.WriteLine(trade.stockName + "\t" + trade.purchasePrice + "\t" + trade.purchaseDate);
				trade = xepQuery.GetNext();
			}
			long totalTime = DateTime.Now.Ticks - startTime;
			xepQuery.Close();
			return totalTime/TimeSpan.TicksPerMillisecond;
		}
    }
}
