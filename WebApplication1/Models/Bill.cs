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
        public string BillID { get; set; }
        public ObjectId TableID { get; set; }
        public string BillPassword { get; set; }
        public int Adult { get; set; }
        public int Child { get; set; }
        public float price { get; set; }
        public List<OrderDetail> DetailID { get; set; }


        public Bill() 
        {
            this.DetailID = new List<OrderDetail>();
        }
        public Bill(string billid, string password)
        {

            this.BillID = billid;
            this.BillPassword = GenPassword();
            this.DetailID = new List<OrderDetail>();
        }
        public string GenPassword()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] ch = chars.ToCharArray();
            char[] rndstring = new char[8];
            Random rd = new Random();

            for (int i = 0; i < rndstring.Length; i++)
            {
                rndstring[i] += chars[rd.Next(chars.Length)];
            }

            string Password = new string(rndstring);
            return Password;
        }
    }
}