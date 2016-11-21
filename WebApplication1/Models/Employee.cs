using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;

namespace WebApplication1.Models
{
    public class Employee
    {
        public ObjectId id;
        public int EmID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Position { get; set; }
        public string EmFirstName { get; set; }
        public string EmLastName { get; set; }
        public string Tel { get; set; }

        public Employee() { }
        public Employee(string Username, string Password, string EmFirstName, string EmLastName, string Tel, string Position)
        {
            this.Username = Username;
            this.Password = Password;
            this.EmFirstName = EmFirstName;
            this.EmLastName = EmLastName;
            this.Tel = Tel;
            this.Position = Position;
        }
    }
}