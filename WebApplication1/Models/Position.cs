using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;

namespace WebApplication1.Models
{
    public class Position
    {
        public ObjectId id;
        public string PositionID { get; set; }
        public string PositionName { get; set; }

        public Position(){}
        public Position(string PositionID, string PositionName)
        {
            this.PositionID = PositionID;
            this.PositionName = PositionName;
        }
    }
}