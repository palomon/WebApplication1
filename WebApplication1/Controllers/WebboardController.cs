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
    public class WebboardController : Controller
    {
        private MongoDatabase database;

        private MongoCollection<Employee> EmployeeCollection;
        private MongoCollection<Bill> BillCollection;
        private MongoCollection<OrderDetail> OrderDetailCollection;
        private MongoCollection<Table> TableCollection;
        private MongoCollection<Product> ProductCollection;
        private MongoCollection<BuffetPrice> BuffetPriceCollection;

        public WebboardController()
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

        // GET: Webboard
        public ActionResult Index()
        {
            return View();
        }
        
        ///////////////////LOGIN EMPLOYEE

        /*
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

                return Redirect("/Webboard/PostList");
            }
            return View();
        }
        */
        ///////////////////LOGIN EMPLOYEE


        ///////////////////REGISTER EMPLOYEE
        
        [HttpPost]
        public ActionResult EmployeeRegister(Employee userabc)
        {
            List<Employee> EmList = this.EmployeeCollection.FindAll().ToList<Employee>();
            if (userabc != null)
            {
                foreach (var item in EmList)
                {
                    if (item.Username.ToUpper().Equals(userabc.Username.ToUpper()))
                    {
                        return Redirect("/Home/AddData");
                    }
                }
            }
            this.EmployeeCollection.Save(userabc);
            return Redirect("/Home/Index");
        }
        ///////////////////REGISTER EMPLOYEE
        
    }
}