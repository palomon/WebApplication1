using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;

namespace WebApplication1.Models
{
    public class OrderDetail
    {
        public ObjectId id;
        public int DetailID { get; set; }
        public int Amount { get; set; }
        public Product ProductID { get; set; }

        public OrderDetail() { }
        public OrderDetail(int DetailID, int Amount,Product ProductID)
        {
            this.DetailID = DetailID;
            this.Amount = Amount;
            this.ProductID = ProductID;
        }
    }
}