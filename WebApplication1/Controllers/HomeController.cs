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
            if (Session["cid"] != null)
            {
                return Redirect("/Home/Customer");
            }
            else 
            {
                return View();
            }
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
                ViewBag.ProdList = Prod_List;

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
            int am = 0;
            od.PID = ObjectId.Parse(od.Status);
            od.Status = "None";
            od.DetailID = 0;
            List<OrderDetail> ODList = this.OrderDetailCollection.FindAll().SetSortOrder(SortBy.Descending("DetailID")).SetLimit(1).ToList<OrderDetail>();
            foreach (var item in ODList)
            {
                od.DetailID = item.DetailID;
            }
            od.DetailID += 1;
            //od.DetailID = int.Parse(this.OrderDetailCollection.Count().ToString()) + 1;
            if (this.ProductCollection.FindOneById(od.PID).Remain >= od.Amount)
            {
                this.OrderDetailCollection.Save(od);
                am = this.ProductCollection.FindOneById(od.PID).Remain - od.Amount;
                
                var query = Query.EQ("_id", ObjectId.Parse(Session["cid"].ToString()));
                var updatebill = Update<Bill>.AddToSet(e => e.DetailID, od);
                this.BillCollection.Update(query, updatebill);

                query = Query.EQ("_id", od.PID);
                var updatepro = Update<Product>.Set(e => e.Remain, am);
                this.ProductCollection.Update(query, updatepro);
                return Redirect("/Home/List?res=true");
            }
            else 
            {
                return Redirect("/Home/List?res=false");
            }
            
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

                ViewBag.CBill = this.BillCollection.FindOneById(ObjectId.Parse(Session["cid"].ToString()));
                
                List<BuffetPrice> BP_List = this.BuffetPriceCollection.FindAll().SetSortOrder(SortBy.Ascending("BuffetID")).ToList<BuffetPrice>();
                ViewBag.BPList = BP_List;

                List<OrderDetail> OD_List = this.OrderDetailCollection.FindAll().SetSortOrder(SortBy.Ascending("DetailID")).ToList<OrderDetail>();
                ViewBag.ODList = OD_List;

                List<Product> PDList = this.ProductCollection.FindAll().SetSortOrder(SortBy.Ascending("_id")).ToList<Product>();
                ViewBag.PList = PDList;

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