using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace EFPlay
{
    class Program
    {
        static void Main(string[] args)
        {
            //Starting interactive prompt

             using (var ctx = new HRContext())
             {
                 Boolean active = true;

                 while (active)
                 {
                    Console.WriteLine("0. Display all traders by name");
                     Console.Out.WriteLine("1. Make a trade (and save)");
                     Console.Out.WriteLine("2. Delete all rows");
                     Console.Out.WriteLine("3. Display trader by ID");
                     Console.Out.WriteLine("4. Display trades by trader last name");
                     Console.Out.WriteLine("5. Display leaderboard");
                     Console.Out.WriteLine("6. Quit");
                     Console.Out.Write("What would you like to do? ");

                     string option = Console.ReadLine(); // Key().ToString();
                     Console.WriteLine(option);
                     switch (option)
                     {
                        case "0":
                            var persons = (from b in ctx.Persons
                                                orderby b.lastname
                                                select b).ToList();

                            // Write all blogs out to Console
                            Console.WriteLine("Query completed with following results:");
                            foreach (var blog in persons)
                            {
                                Console.WriteLine(" - " + blog.lastname + ", " + blog.firstname + " with ID: " + blog.PersonId);
                            }
                            break;
                        // Task 2
                        case "1":
                            // uncomment below line to run Task 2
                            Task2(ctx);
                            break;

                        // Task 3
                        case "2":
                            // uncomment below line to run Task 3
                            Task3(ctx);
                            break;

                        // Task 4
                        case "3":
                            // uncomment below line to run Task 4
                            Task4(ctx);
                            break;

                        // Task 5
                        case "4":
                            // uncomment below line to run Task 5
                            Task5(ctx);
                            break;

                        // Task 6
                        case "5":
                            // uncomment below line to run Task 6
                            Task6(ctx);
                            break;

                         case "6":
                             Console.Out.WriteLine("Exited.");
                             active = false;
                             break;

                         default:
                             Console.Out.WriteLine("Invalid option. Try again!");
                             break;
                     }
                 }
             }
        }

        public static void Task2(HRContext ctx)
        {
            Console.Out.Write("Stock name: ");
            String stockName = Console.ReadLine();

            Console.Out.Write("DateTime (YYYY-MM-DD): ");
            DateTime.TryParse(Console.ReadLine(), out DateTime tempDate);

            Console.Out.Write("Price: ");
            Decimal.TryParse(Console.ReadLine(), out Decimal price);

            Console.Out.Write("Number of Shares: ");
            int.TryParse(Console.ReadLine(), out int shares);

            Console.Out.WriteLine("Choose one:");
            Console.Out.WriteLine("1. Existing trader");
            Console.Out.WriteLine("2. New trader");
            String newOrExisting = Console.ReadLine();
            if (newOrExisting.Equals("1"))
            {
                Console.Out.Write("Link trade to trader with which ID? ");
                long.TryParse(Console.ReadLine(), out long traderID);
                Person person1 = ctx.Persons.Where(p => p.PersonId == traderID).FirstOrDefault();
                //Trade2 trade = new Trade2() { stockName = stockName, purchaseDate = tempDat e, purchasePrice = price, shares = shares, Trade2Id = traderID };
                //Trade2 trade = new Trade2() { stockName = stockName, purchaseDate = tempDate, purchasePrice = price, shares = shares, trader = person1 };
                Trade2 trade = new Trade2(stockName, tempDate, price, shares, person1);
                ctx.Trades.Add(trade);
                ctx.SaveChanges();
                Console.WriteLine("Trade saved.");
            }
            else if (newOrExisting.Equals("2"))
            {
                Console.Out.Write("Trader first name: ");
                String traderFirstName = Console.ReadLine();

                Console.Out.Write("Trader last name: ");
                String traderLastName = Console.ReadLine();

                Console.Out.Write("Trader phone: ");
                String phone = Console.ReadLine();
                Person person2 = new Person() { firstname = traderFirstName, lastname = traderLastName, phone = phone };
                //Trade2 trade = new Trade2() { stockName = stockName, purchaseDate = tempDate, purchasePrice = price, shares = shares, trader = person2 };
                Trade2 trade = new Trade2(stockName, tempDate, price, shares, person2);
                ctx.Trades.Add(trade);
                ctx.Persons.Add(person2);
                ctx.SaveChanges();
                Console.WriteLine("Trade and person saved.");
            }
            else
            {
                Console.Out.WriteLine("Invalid option. Try again");
            }
        }

        public static void Task3(HRContext ctx)
        {
            Console.WriteLine("transaction started");
            ctx.Persons.RemoveRange(ctx.Persons);
            ctx.Trades.RemoveRange(ctx.Trades);
            Console.WriteLine("All trades and traders deleted from the database.");
            ctx.SaveChanges();
        }

        public static void Task4(HRContext ctx)
        {
            Console.Out.Write("Find trader for which ID? ");
            long.TryParse(Console.ReadLine(), out long personID);


            Person person3 = ctx.Persons.Where(p => p.PersonId == personID).FirstOrDefault();
            Console.WriteLine("Trader Name: " + person3.firstname + " " + person3.lastname);
            Console.WriteLine("Trader ID: " + person3.PersonId + " and Trader phone: " + person3.phone);
            var trades = (from b in ctx.Trades
                          orderby b.purchaseDate
                          where b.trader.PersonId == person3.PersonId
                          select b).ToList();
            Console.WriteLine(" has " + trades.Count + " trades: ");
            foreach (Trade2 trade in trades)
            {
                Console.WriteLine(" Name: " + trade.stockName + " with purchase price " + trade.purchasePrice + " on " + trade.purchaseDate);
            }
        }

        public static void Task5(HRContext ctx)
        {
            Console.Out.Write("Find all traders with which last name? ");
            String personName = Console.ReadLine();

            Console.WriteLine("Query completed with following results:");

            var traders = ctx.Persons.Where(p => p.lastname == personName).Include(p => p.trades).ToList();
            foreach(var p in traders)
            {
                Console.WriteLine(" - " + p.firstname + " " + p.lastname + " with ID: " + p.PersonId);
                foreach (var trade in p.trades) {
                    Console.WriteLine(" Name: " + trade.stockName + " with purchase price " + trade.purchasePrice + " on " + trade.purchaseDate);
                }
            }
        }

        public static void Task6(HRContext ctx)
        {
            Console.Out.WriteLine("Displaying leaderboard.");

            String hql = "select t.trader_PersonID, sum((s.stockclose-t.purchasePrice)*t.shares) as gain, (sum((s.stockclose-t.purchasePrice)*t.shares)/sum(t.purchasePrice) * 100) as percentIncrease " +
                            "from dbo.Trade2 as t left join Demo.Stock as s on t.stockName = s.Name where s.TransDate = TO_DATE('2017/08/10', 'yyyy/mm/dd') " +
                            "group by t.trader_PersonID " +
                            "order by gain desc";

            var query = ctx.Database.SqlQuery<TestEntity>(hql).ToList();

            Console.WriteLine("Trader\t\tGain\t\tPercent Increase");
            foreach (TestEntity aRow in query)
            {
                int id5 = (int) aRow.trader_PersonID;
                Person person5 = ctx.Persons.Where(p => p.PersonId == id5).FirstOrDefault();
                Decimal gain = (Decimal) aRow.gain;
                Decimal percentIncrease = (Decimal) aRow.percentIncrease;
                Console.WriteLine(person5.firstname + " " + person5.lastname + "\t$" + gain + "\t" + Math.Ceiling(percentIncrease * 100) / 100 + "%");
            }
        }

        public class TestEntity
        {
            public Int64 trader_PersonID { get; set; }
            public Decimal gain { get; set; }
            public Decimal percentIncrease { get; set; }
        }

        //Context Class
        public class HRContext : DbContext
        {
            public HRContext() : base("name=StockDBConnectionString")
            {
            }
            public DbSet<Person> Persons { get; set; }
            public DbSet<Trade2> Trades { get; set; }

        }

        //Data Classes
        public class Person
        {
            public Person() { }

            public Person(string firstname, string lastname, string phone)
            {
                this.firstname = firstname;
                this.lastname = lastname;
                this.phone = phone;
                this.trades = new HashSet<Trade2>();
            }

            public long PersonId { get; set; }
            public string firstname { get; set; }
            public string lastname { get; set; }
            public string phone { get; set; }
            public ICollection<Trade2> trades { get; set; }

        }

        public class Trade2
        {
            public Trade2() { }

            public long Trade2Id { get; set; }
            public string stockName { get; set; }
            public DateTime purchaseDate { get; set; }
            public Decimal purchasePrice { get; set; }
            public int shares { get; set; }
            public Person trader { get; set; }

            public Trade2(String stockName, DateTime purchaseDate, Decimal purchasePrice, int shares, Person trader)
            {
                this.stockName = stockName;
                this.purchaseDate = purchaseDate;
                this.purchasePrice = purchasePrice;
                this.shares = shares;
                this.trader = trader;
            }
        }
    }
}
