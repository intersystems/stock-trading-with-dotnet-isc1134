using System;

namespace myApp{
    public class StockInfo {
        public String name;
		public String mission;
		public String founder;
        //Constructors
        public StockInfo(){}
        public StockInfo(String n, String m, String f) {
            name = n;
            mission = m;
            founder = f;
        }    
	}
}