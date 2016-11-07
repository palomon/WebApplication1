using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;

namespace WebApplication1.Models
{
    public class Product
    {
        public ObjectId id;
        public int PID { get; set; }
        public string PName { get; set; }
        public int PPrice { get; set; }
        public int AmountMAX { get; set; }
        public int Remain { get; set; }

        public Product() { }
        public Product( int PID, string PName, int PPrice, int AmounMAX)
        {
            this.PID = PID;
            this.PName = PName;
            this.PPrice = PPrice;
            this.AmountMAX = AmountMAX;
            this.Remain = AmountMAX;

        }
        public int CheckProduct(int PID)
        {
            return 2;
        }
    }
}