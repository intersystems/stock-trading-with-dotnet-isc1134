using System;
using InterSystems.Data.IRISClient; 

namespace myApp
{
    class adonetplaystocksTask5
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
                        Console.WriteLine("Creating table...");
					    CreatePortfolioTable(connect);
                        break;
                    case "3":
                        Console.WriteLine("Name: ");
                        String name = Console.ReadLine();

                        Console.WriteLine("Date: ");
                        String tDate = Console.ReadLine();

                        Console.WriteLine("Price: ");
                        String price = Console.ReadLine();

                        Console.WriteLine("Number of shares: ");
                        String share = Console.ReadLine();
                        int shares;
                        Int32.TryParse(share, out shares);

                        AddPortfolioItem(connect, name, tDate, price, shares);	
                        break;
                    case "4":
                        Console.WriteLine("Which stock would you like to update? ");
                        String stockName = Console.ReadLine();

                        Console.WriteLine("New Price: ");
                        String updatePrice = Console.ReadLine();

                        Console.WriteLine("New Date: ");
                        String updateDate = Console.ReadLine();

                        Console.WriteLine("New number of shares: ");
                        String upShare = Console.ReadLine();
                        int updateShares;
                        Int32.TryParse(upShare, out updateShares);

                        UpdateStock(connect, stockName, updatePrice, updateDate, updateShares);
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

        public static void CreatePortfolioTable(IRISADOConnection dbconnection) 
	    {
            String createTable = "CREATE TABLE Demo.Portfolio(Name varchar(50) unique, PurchaseDate date, PurchasePrice numeric(10,4), Shares int, DateTimeUpdated DateTime)";
            try 
            {
                IRISCommand cmd = new IRISCommand(createTable, dbconnection);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Created Demo.Portfolio table successfully.");
            } 
            catch (Exception e) 
            {
                Console.WriteLine("Table not created and likely already exists. " + e);
		    }
	    }
	    
        public static void AddPortfolioItem (IRISADOConnection dbconnection, String name, String purchaseDate, String price, int shares)
        {
            DateTime t = DateTime.Now;
            try 
            {
                String sql = "INSERT INTO Demo.Portfolio (Name, PurchaseDate, PurchasePrice, Shares, DateTimeUpdated) VALUES (?,?,?,?,?)";
                IRISCommand cmd = new IRISCommand(sql, dbconnection);
                cmd.Parameters.AddWithValue("Name", name);
                cmd.Parameters.AddWithValue("PurchaseDate", Convert.ToDateTime(purchaseDate));
                cmd.Parameters.AddWithValue("PurchasePrice", price);
                cmd.Parameters.AddWithValue("Shares", shares);
                cmd.Parameters.AddWithValue("DateTimeUpdated", t);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Added new line item for stock " + name + ".");
            } 
            catch (Exception e) 
            {
                Console.WriteLine("Error adding portfolio item: " + e);	
            }         
        }

        public static void UpdateStock(IRISADOConnection dbconnection, String stockname, String price, String transDate, int shares)
	    {
            DateTime t = DateTime.Now;
            try 
            {
                String sql = "UPDATE Demo.Portfolio SET purchaseDate = ?, purchasePrice= ?, shares = ?, DateTimeUpdated= ? WHERE name= ?";
                IRISCommand cmd = new IRISCommand(sql, dbconnection);
                cmd.Parameters.AddWithValue("PurchaseDate", Convert.ToDateTime(transDate));
                cmd.Parameters.AddWithValue("PurchasePrice", price);           
                cmd.Parameters.AddWithValue("Shares", shares);
                cmd.Parameters.AddWithValue("DateTimeUpdated", t);
                cmd.Parameters.AddWithValue("Name", stockname);             
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    Console.WriteLine(stockname + " updated.");
                }
                else 
                {
                    Console.WriteLine(stockname + " not found");
                }
            } 
            catch (Exception e) 
            {
                Console.WriteLine("Error updating " + stockname + " : " + e);
            }
        }
    }
}