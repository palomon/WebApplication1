using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;

namespace WebApplication1.Models
{
    public class BuffetPrice
    {
        public ObjectId id;
        public int BuffetID { get; set; }
        public float BPrice { get; set; }

        public BuffetPrice() { }
        public BuffetPrice(int BuffetID,float BPrice)
        {
            this.BuffetID = BuffetID;
            this.BPrice = BPrice;
        }
    }
}