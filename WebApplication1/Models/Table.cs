using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;

namespace WebApplication1.Models
{
    public class Table
    {
        public ObjectId id;
        public int TableID { get; set; }
        public string Tablename { get; set; }
        public Boolean Available { get; set; }

        public Table() { }
        public Table(int id, string Tname)
        {
            this.TableID = id;
            this.Tablename = Tname;
        }
    }
}