using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WebApplication1.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private MongoDatabase database;

        private MongoCollection<Employee> EmployeeCollection;
        private MongoCollection<Bill> BillCollection;
        private MongoCollection<OrderDetail> OrderDetailCollection;
        private MongoCollection<Table> TableCollection;
        private MongoCollection<Product> ProductCollection;
        private MongoCollection<BuffetPrice> BuffetPriceCollection;

        public HomeController()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var server = client.GetServer();
            this.database = server.GetDatabase("WebApplication1");


            this.EmployeeCollection = database.GetCollection<Employee>("Employee");
            this.BillCollection = database.GetCollection<Bill>("Bill");
            this.OrderDetailCollection = database.GetCollection<OrderDetail>("OrderDetail");
            this.TableCollection = database.GetCollection<Table>("Table");
            this.ProductCollection = database.GetCollection<Product>("Product");
            this.BuffetPriceCollection = database.GetCollection<BuffetPrice>("BuffetPrice");
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Customer()
        {
            return View();
        }



        [HttpPost]
        public ActionResult Login(Employee user)
        {
            var query = Query.And(Query<Employee>.EQ(e => e.Username, user.Username),
                        Query<Employee>.EQ(e => e.Password, user.Password)); //เช็คว่ารหัสกับชื่อตรงมั้ย
            var result = this.EmployeeCollection.FindOne(query); //มีข้อมูลที่กรอกมั้ย
            if (result != null)
            {
                Session["id"] = result.id;
                Session["username"] = result.Username;

                return Redirect("/Home/Index");
            }
            return View();
        }
    }
}