using System;
using InterSystems.Data.IRISClient;

namespace myApp{
    public class Stock {
        private long id;
        private DateTime tDate;
        private decimal open;
        private decimal high;
        private decimal low;
        private decimal close;
        private int volume;
        private String stockName;

        //Constructors
        public Stock(){}
        public Stock(DateTime tDate, decimal open, decimal high, decimal low, decimal close, int volume, String stockName) {
            this.tDate = tDate;
            this.open = open;
            this.high = high;
            this.close = close;
            this.volume = volume;
            this.stockName = stockName;
        }
        
        public long getId() {
            return id;
        }
        
        public void setId(long id) {
            this.id = id;
        }
        
        public DateTime gettDate() {
            return tDate;
        }
        public void settDate(DateTime tDate) {
            this.tDate = tDate;
        }
        
        public decimal getOpen() {
            return open;
        }

        public void setOpen(decimal open) {
            this.open = open;
        }

        public decimal getHigh() {
            return high;
        }

        public void setHigh(decimal high) {
            this.high = high;
        }

        public decimal getLow() {
            return low;
        }

        public void setLow(decimal low) {
            this.low = low;
        }

        public decimal getClose() {
            return close;
        }

        public void setClose(decimal close) {
            this.close = close;
        }

        public int getVolume() {
            return volume;
        }

        public void setVolume(int volume) {
            this.volume = volume;
        }

        public string getStockName() {
            return stockName;
        }

        public void setStockName(string stockName) {
            this.stockName = stockName;
        }

        public string toString() {
            return "Stock [date=" + tDate + ", open=" + open + ", high=" + high + ", low=" + low + ", close=" + close
                    + ", volume=" + volume + ", name=" + stockName + "]";
        }
	}
}