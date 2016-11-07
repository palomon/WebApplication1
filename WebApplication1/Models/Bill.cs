using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;

namespace WebApplication1.Models
{
    public class Bill
    {
        public ObjectId id;
        public int BillID { get; set; }
        public Table TableID { get; set; }
        public string BillPassword { get; set; }
        public int Adult { get; set; }
        public int Child { get; set; }
        public BuffetPrice price { get; set; }
        public List<OrderDetail> DetailID { get; set; }


        public Bill() { }
        public Bill(int billid, Table Tid, string password)
        {
            this.BillID = billid;
            this.TableID = Tid;
            this.BillPassword = password;
        }
        public float CalTotal( int tID)
        {
            
            return 3;
        }
    }
}