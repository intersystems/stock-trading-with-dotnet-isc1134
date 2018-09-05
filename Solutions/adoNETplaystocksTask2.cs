using System;
using InterSystems.Data.IRISClient;

namespace myApp
{
    class adonetplaystocksTask2
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

                bool always = true;
                while (always) {
                    Console.WriteLine("1. View top 10");
                    Console.WriteLine("2. Create Portfolio table");
                    Console.WriteLine("3. Add to Portfolio");
                    Console.WriteLine("4. Update Portfolio");
                    Console.WriteLine("5. Delete from Portfolio");
                    Console.WriteLine("6. View Portfolio");
                    Console.WriteLine("7. Quit");
                    Console.WriteLine("What would you like to do? ");
                    
                    String option = Console.ReadLine();
                    switch (option) {
                    case "1":
                        Console.WriteLine("On which date? (YYYY/MM/DD) ");
					    String queryDate = Console.ReadLine();
					    FindTopOnDate(connect, queryDate);
                        break;
                    case "2":
                        Console.WriteLine("TO DO: Create Portfolio table");
                        break;
                    case "3":
                        Console.WriteLine("TO DO: Add to Portfolio");	
                        break;
                    case "4":
                        Console.WriteLine("TO DO: Update Portfolio");
                        break;
                    case "5":
                        Console.WriteLine("TO DO: Delete from Portfolio");
                        break;
                    case "6":
                        Console.WriteLine("TO DO: View Portfolio");
                        break;
                    case "7":
                        Console.WriteLine("Exited.");
                        always = false;
                        break;
                    default: 
                        Console.WriteLine("Invalid option. Try again!");
                        break;
                    }
                }
            } catch (Exception e) { 
                Console.WriteLine("Interactive prompt failed:\n" + e); 
            }
        }

        public static void FindTopOnDate(IRISADOConnection dbconnection, String onDate)
        {
            //Find top 10 stocks on a particular date
            try 
            {
                String sql = "SELECT distinct top 10 TransDate,Name,StockClose,StockOpen,High,Low,Volume FROM Demo.Stock WHERE TransDate=? ORDER BY stockclose desc";
                IRISCommand cmd = new IRISCommand(sql, dbconnection);
                cmd.Parameters.AddWithValue("TransDate", Convert.ToDateTime(onDate));	
                IRISDataReader reader = cmd.ExecuteReader();
                Console.WriteLine("Date\t\tName\tOpening Price\tDaily High\tDaily Low\tClosing Price\tVolume");
                while(reader.Read()){
                    DateTime date = (DateTime) reader[reader.GetOrdinal("TransDate")];
                    decimal open = (decimal) reader[reader.GetOrdinal("StockOpen")];
                    decimal high = (decimal) reader[reader.GetOrdinal("High")];
                    decimal low = (decimal) reader[reader.GetOrdinal("Low")];
                    decimal close = (decimal) reader[reader.GetOrdinal("StockClose")];
                    int volume = (int) reader[reader.GetOrdinal("Volume")];
                    String name = (string) reader[reader.GetOrdinal("Name")];
                    Console.WriteLine(date.ToString("MM/dd/yyyy") + "\t" + name + "\t" + open + "\t" + high+ "\t" + low + "\t" + close+ "\t" + volume);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}