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
        private int BillID { get; set; }
        private Table TableID { get; set; }
        private string BillPassword { get; set; }
        private int Adult { get; set; }
        private int Child { get; set; }
        private BuffetPrice price { get; set; }
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