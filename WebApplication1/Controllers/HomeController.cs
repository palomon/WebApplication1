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
            if (Session["cid"] != null)
            {
                List<Table> TableList = this.TableCollection.FindAll().SetSortOrder(SortBy.Ascending("TableID")).ToList<Table>();
                foreach (var item in TableList)
                {
                    if (item.id.Equals(Session["ctableid"]))
                    {
                        ViewBag.cTableID = item.TableID.ToString();
                    }
                }
                ViewBag.CBill = this.BillCollection.FindOneById(ObjectId.Parse(Session["cid"].ToString()));

                return View();
            }
            else
            {
                ViewBag.cTableID = "0";
                return View();
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Bill customer)
        {
            var query = Query.And(
                Query<Bill>.EQ(e => e.BillID, customer.BillID),
                Query<Bill>.EQ(e => e.BillPassword, customer.BillPassword)
                );
            var result = this.BillCollection.FindOne(query);
            if (result != null)
            {
                Session["cid"] = result.id;
                Session["cbillid"] = result.BillID;
                Session["cbillpass"] = result.BillPassword;
                Session["ctableid"] = result.TableID;
                return Redirect("/Home/Customer");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return Redirect("/Home/");
        }

        public ActionResult List()
        {
            if (Session["cid"] != null)
            {
                List<Table> TableList = this.TableCollection.FindAll().SetSortOrder(SortBy.Ascending("TableID")).ToList<Table>();
                foreach (var item in TableList)
                {
                    if (item.id.Equals(Session["ctableid"]))
                    {
                        ViewBag.cTableID = item.TableID.ToString();
                    }
                }

                ViewBag.CBill = this.BillCollection.FindOneById(ObjectId.Parse(Session["cid"].ToString()));
                List<Product> Prod_List = this.ProductCollection.FindAll().SetSortOrder(SortBy.Ascending("PID")).ToList<Product>();
                //List<double> Prod_Pecent = new List<double>();
                foreach (var item in Prod_List)
                {
                    //Prod_Pecent.Add((item.Remain * 1.0 / item.AmountMAX * 1.0) * 100.0);

                }
                ViewBag.ProdList = Prod_List;
                //ViewBag.ProdPercent = Prod_Pecent;

                return View();
            }
            else
            {
                ViewBag.cTableID = "0";
                return Redirect("/Home/Login");
            }
        }

        [HttpPost]
        public ActionResult AddOd(OrderDetail od)
        {
            od.Status = 0;
            this.OrderDetailCollection.Save(od);
            return Redirect("/Home/List");
        }
        public ActionResult Customer()
        {
            if (Session["cid"] != null)
            {
                List<Table> TableList = this.TableCollection.FindAll().SetSortOrder(SortBy.Ascending("TableID")).ToList<Table>();
                foreach (var item in TableList)
                {
                    if (item.id.Equals(Session["ctableid"]))
                    {
                        ViewBag.cTableID = item.TableID.ToString();
                    }
                }

                //List<Bill> BilList = this.BillCollection.FindAll().SetSortOrder(SortBy.Ascending("BillID")).ToList<Bill>();
                ViewBag.CBill = this.BillCollection.FindOneById(ObjectId.Parse(Session["cid"].ToString()));

                return View();
            }
            else
            {
                ViewBag.cTableID = "0";
                return Redirect("/Home/Login");
            }
        }

    }
}