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
    public class AdminController : Controller
    {

        private MongoDatabase database;

        private MongoCollection<Employee> EmployeeCollection;
        private MongoCollection<Bill> BillCollection;
        private MongoCollection<OrderDetail> OrderDetailCollection;
        private MongoCollection<Table> TableCollection;
        private MongoCollection<Product> ProductCollection;
        private MongoCollection<BuffetPrice> BuffetPriceCollection;

        public AdminController()
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

        // GET: Admin
        public ActionResult Index()
        {
            if (Session["id"] != null)
            {
                List<Employee> EmList = this.EmployeeCollection.FindAll().ToList<Employee>();
                foreach (var item in EmList)
                {
                    //Console.WriteLine(item.EmFirstName);
                    if (item.id.Equals(Session["id"]))
                    {
                        //Console.WriteLine(item.EmFirstName);
                        ViewBag.EmFN = item.EmFirstName;
                        ViewBag.EmLN = item.EmLastName;
                        ViewBag.EmPos = item.Position;
                    }
                }
                List<Table> TableList = this.TableCollection.FindAll().SetSortOrder(SortBy.Ascending("TableID")).ToList<Table>();
                
                
                //AUTO CHECK AVALIABLE FROM BILL 
                List<Bill> BilList = this.BillCollection.FindAll().SetSortOrder(SortBy.Ascending("BillID")).ToList<Bill>();
                Boolean flag;
                foreach (var item in TableList)
                {
                    flag = true;
                    foreach (var bitem in BilList)
                    {
                        if (item.id.Equals(bitem.TableID))
                        {
                            flag = false;
                            var query = Query.EQ("_id", item.id);
                            var update = Update<Table>.Set(e => e.Available, false);
                            this.TableCollection.Update(query, update);
                        }
                    }
                    if (flag == true) 
                    {
                        var query = Query.EQ("_id", item.id);
                        var update = Update<Table>.Set(e => e.Available, true);
                        this.TableCollection.Update(query, update);
                    }
                }
                //AUTO CHECK AVALIABLE FROM BILL 

                List<Table> Tabletwo = this.TableCollection.FindAll().SetSortOrder(SortBy.Ascending("TableID")).ToList<Table>();
                ViewBag.TabList = Tabletwo;

                return View();
            }
            else
            {
                return Redirect("/Admin/Login");
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Employee user)
        {
            var query = Query.And(
                Query<Employee>.EQ(e => e.Username, user.Username),
                Query<Employee>.EQ(e => e.Password, user.Password)
                );
            var result = this.EmployeeCollection.FindOne(query);
            if (result != null)
            {
                Session["id"] = result.id;
                Session["username"] = result.Username;
                Session["Password"] = result.Password;
                //Session["EmFN"] = result.EmFirstName;
                //Session["EmLN"] = result.EmLastName;
                //Session["Pos"] = result.Position;
                return Redirect("/Admin/");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return Redirect("/Admin/");
        }

        public ActionResult Stock()
        {
            if (Session["id"] != null)
            {
                List<Employee> EmList = this.EmployeeCollection.FindAll().ToList<Employee>();
                foreach (var item in EmList)
                {
                    //Console.WriteLine(item.EmFirstName);
                    if (item.id.Equals(Session["id"]))
                    {
                        //Console.WriteLine(item.EmFirstName);
                        ViewBag.EmFN = item.EmFirstName;
                        ViewBag.EmLN = item.EmLastName;
                        ViewBag.EmPos = item.Position;
                    }
                }

                List<Product> Prod_List = this.ProductCollection.FindAll().SetSortOrder(SortBy.Ascending("PID")).ToList<Product>();
                List<double> Prod_Pecent = new List<double>();
                foreach (var item in Prod_List)
                {
                    Prod_Pecent.Add((item.Remain * 1.0 / item.AmountMAX * 1.0) * 100.0);

                }
                ViewBag.ProdList = Prod_List;
                ViewBag.ProdPercent = Prod_Pecent;



                return View();
            }
            else
            {
                return Redirect("/Admin/Login");
            }
        }

        public ActionResult ManageEmployee()
        {
            if (Session["id"] != null)
            {
                List<Employee> EmList = this.EmployeeCollection.FindAll().SetSortOrder(SortBy.Ascending("EmID")).ToList<Employee>();
                foreach (var item in EmList)
                {
                    //Console.WriteLine(item.EmFirstName);
                    if (item.id.Equals(Session["id"]))
                    {
                        //Console.WriteLine(item.EmFirstName);
                        ViewBag.EmFN = item.EmFirstName;
                        ViewBag.EmLN = item.EmLastName;
                        ViewBag.EmPos = item.Position;
                    }
                }
                ViewBag.Employee = EmList;



                return View();
            }
            else
            {
                return Redirect("/Admin/Login");
            }
        }

        public ActionResult DeleteEmployee(string id)
        {
            var query = Query.EQ("_id", ObjectId.Parse(id));
            this.EmployeeCollection.Remove(query);
            return Redirect("/Admin/ManageEmployee");
        }

        public ActionResult Order()
        {
            if (Session["id"] != null)
            {
                List<Employee> EmList = this.EmployeeCollection.FindAll().ToList<Employee>();
                foreach (var item in EmList)
                {
                    //Console.WriteLine(item.EmFirstName);
                    if (item.id.Equals(Session["id"]))
                    {
                        //Console.WriteLine(item.EmFirstName);
                        ViewBag.EmFN = item.EmFirstName;
                        ViewBag.EmLN = item.EmLastName;
                        ViewBag.EmPos = item.Position;
                    }
                }

                List<Bill> Bill_List = this.BillCollection.FindAll().SetSortOrder(SortBy.Ascending("BillID")).ToList<Bill>();

                ViewBag.BList = Bill_List;

                List<Table> TableList = this.TableCollection.FindAll().SetSortOrder(SortBy.Ascending("TableID")).ToList<Table>();
                List<string> TabName = new List<string>();
                foreach (var item in Bill_List) 
                {
                    foreach (var item2 in TableList)
                    {
                        if (item.TableID == item2.id) 
                        {
                            TabName.Add(item2.Tablename);
                        }
                    }
                }
                ViewBag.Tab_n = TabName;

                List<Product> PDList = this.ProductCollection.FindAll().SetSortOrder(SortBy.Ascending("_id")).ToList<Product>();
                ViewBag.PList = PDList;

                List<OrderDetail> OD_List = this.OrderDetailCollection.FindAll().SetSortOrder(SortBy.Ascending("_id")).ToList<OrderDetail>();

                ViewBag.ODList = OD_List;

                return View();
            }
            else
            {
                return Redirect("/Admin/Login");
            }
        }


        public ActionResult AddEmployee()
        {
            if (Session["id"] != null)
            {
                List<Employee> EmList = this.EmployeeCollection.FindAll().ToList<Employee>();
                foreach (var item in EmList)
                {
                    //Console.WriteLine(item.EmFirstName);
                    if (item.id.Equals(Session["id"]))
                    {
                        //Console.WriteLine(item.EmFirstName);
                        ViewBag.EmFN = item.EmFirstName;
                        ViewBag.EmLN = item.EmLastName;
                        ViewBag.EmPos = item.Position;
                    }
                }
                ViewBag.Employee = EmList;



                return View();
            }
            else
            {
                return Redirect("/Admin/Login");
            }
        }
        
        // AddUser
        [HttpPost]
        public ActionResult AddEmployee(Employee userabc)
        {
            List<Employee> EmList = this.EmployeeCollection.FindAll().ToList<Employee>();
            if (userabc != null)
            {
                foreach (var item in EmList)
                {
                    if (item.Username.ToUpper().Equals(userabc.Username.ToUpper()) || item.EmID.Equals(userabc.EmID))
                    {
                        return Redirect("/Admin/AddEmployee");
                    }
                }
            }
            this.EmployeeCollection.Save(userabc);
            return Redirect("/Admin/Index");
        }

        public ActionResult EditEmployee()
        {
            if (Session["id"] != null)
            {
                List<Employee> EmList = this.EmployeeCollection.FindAll().ToList<Employee>();
                foreach (var item in EmList)
                {
                    //Console.WriteLine(item.EmFirstName);
                    if (item.id.Equals(Session["id"]))
                    {
                        //Console.WriteLine(item.EmFirstName);
                        ViewBag.EmFN = item.EmFirstName;
                        ViewBag.EmLN = item.EmLastName;
                        ViewBag.EmPos = item.Position;
                    }
                }
                ViewBag.Employee = EmList;



                return View();
            }
            else
            {
                return Redirect("/Admin/Login");
            }
        }

        [HttpGet]
        public ActionResult EditEmployee(string id)
        {
            if (Session["id"] != null)
            {
                List<Employee> EmList = this.EmployeeCollection.FindAll().ToList<Employee>();
                foreach (var item in EmList)
                {
                    //Console.WriteLine(item.EmFirstName);
                    if (item.id.Equals(Session["id"]))
                    {
                        //Console.WriteLine(item.EmFirstName);
                        ViewBag.EmFN = item.EmFirstName;
                        ViewBag.EmLN = item.EmLastName;
                        ViewBag.EmPos = item.Position;
                    }
                }
                ViewBag.Employee = EmList;
                ViewBag.Flag = false;
                if (id != null)
                {
                    ViewBag.EmployeeEdit = this.EmployeeCollection.FindOneById(ObjectId.Parse(id));
                    ViewBag.Flag = true;
                }
                
                return View();
            }
            else
            {
                return Redirect("/Admin/Login");
            }
        }

        [HttpPost]
        public ActionResult EditEmp(string id,Employee user)
        {
            if (Session["id"] != null)
            {
                List<Employee> EmList = this.EmployeeCollection.FindAll().ToList<Employee>();
                
                var query = Query.EQ("EmID", user.EmID);
                var update = Update<Employee>.Set(e => e.EmFirstName, user.EmFirstName);
                this.EmployeeCollection.Update(query, update);
                update = Update<Employee>.Set(e => e.EmLastName, user.EmLastName);
                this.EmployeeCollection.Update(query, update);
                update = Update<Employee>.Set(e => e.Tel, user.Tel);
                this.EmployeeCollection.Update(query, update);
                update = Update<Employee>.Set(e => e.Position, user.Position);
                this.EmployeeCollection.Update(query, update);
                return Redirect("/Admin/ManageEmployee");
            }
            else
            {
                return Redirect("/Admin/Login");
            }
            /*
            var query = Query.EQ("_id", ObjectId.Parse(id));
            var update = Update<Employee>.AddToSet(e => e.comment, comment);
            this.EmployeeCollection.Update(query, update);
            return Redirect("/Admin/ManageEmployee");
            */
        }

        //AddProduct
        public ActionResult AddProduct()
        {
             if (Session["id"] != null)
             {
                 List<Employee> EmList = this.EmployeeCollection.FindAll().ToList<Employee>();
                 foreach (var item in EmList)
                 {
                     //Console.WriteLine(item.EmFirstName);
                     if (item.id.Equals(Session["id"]))
                     {
                         //Console.WriteLine(item.EmFirstName);
                         ViewBag.EmFN = item.EmFirstName;
                         ViewBag.EmLN = item.EmLastName;
                         ViewBag.EmPos = item.Position;
                     }
                 }
                 ViewBag.Employee = EmList;
                return View();
             }
             else
             {
                 return Redirect("/Admin/Login");
             }
        }
        // AddProduct
        [HttpPost]
        public ActionResult AddProduct(Product Prod)
        {
            List<Product> PDList = this.ProductCollection.FindAll().ToList<Product>();
            if (Prod != null)
            {
                foreach (var item in PDList)
                {
                    if (item.PID.Equals(Prod.PID))
                    {
                        return Redirect("/Admin/AddProduct?result=false");
                    }
                }
            }
            Prod.AmountMAX = 100;
            this.ProductCollection.Save(Prod);
            return Redirect("/Admin/AddProduct?result=true");

        }
        //Edit Product
        public ActionResult EditProduct()
        {
             if (Session["id"] != null)
             {
                 List<Employee> EmList = this.EmployeeCollection.FindAll().ToList<Employee>();
                 foreach (var item in EmList)
                 {
                     //Console.WriteLine(item.EmFirstName);
                     if (item.id.Equals(Session["id"]))
                     {
                         //Console.WriteLine(item.EmFirstName);
                         ViewBag.EmFN = item.EmFirstName;
                         ViewBag.EmLN = item.EmLastName;
                         ViewBag.EmPos = item.Position;
                     }
                 }
                 ViewBag.Employee = EmList;



                 return View();
             }
             else
             {
                 return Redirect("/Admin/Login");
             }
        }

        [HttpGet]
        public ActionResult EditProduct(string id)
        {
            if (Session["id"] != null)
            {
                List<Employee> EmList = this.EmployeeCollection.FindAll().ToList<Employee>();
                foreach (var item in EmList)
                {
                    //Console.WriteLine(item.EmFirstName);
                    if (item.id.Equals(Session["id"]))
                    {
                        //Console.WriteLine(item.EmFirstName);
                        ViewBag.EmFN = item.EmFirstName;
                        ViewBag.EmLN = item.EmLastName;
                        ViewBag.EmPos = item.Position;
                    }
                }
                ViewBag.Employee = EmList;

                List<Product> ProdList = this.ProductCollection.FindAll().ToList<Product>();
                ViewBag.Product = ProdList;
                ViewBag.Flag = false;
                if (id != null)
                {
                    ViewBag.ProductEdit = this.ProductCollection.FindOneById(ObjectId.Parse(id));
                    ViewBag.Flag = true;
                }

                return View();
            }
            else
            {
                return Redirect("/Admin/Login");
            }
        }

        [HttpPost]
        public ActionResult EditProd(string id, Product prod)
        {
            if (Session["id"] != null)
            {
                List<Product> ProList = this.ProductCollection.FindAll().ToList<Product>();
                var query = Query.EQ("PName", prod.PName);
                var update = Update<Product>.Set(e => e.PPrice, prod.PPrice);
                this.ProductCollection.Update(query, update);
                update = Update<Product>.Set(e => e.Remain, prod.Remain);
                this.ProductCollection.Update(query, update);
                return Redirect("/Admin/Stock");
            }
            else
            {
                return Redirect("/Admin/Login");
            }
        }

        public ActionResult DeleteProduct(string id)
        {
            var query = Query.EQ("_id", ObjectId.Parse(id));
            this.ProductCollection.Remove(query);
            return Redirect("/Admin/Stock");
        }


        public ActionResult CheckBill()
        {
            return View();
        }

        public ActionResult AddTable()
        {
            if (Session["id"] != null)
            {
                List<Employee> EmList = this.EmployeeCollection.FindAll().ToList<Employee>();
                foreach (var item in EmList)
                {
                    //Console.WriteLine(item.EmFirstName);
                    if (item.id.Equals(Session["id"]))
                    {
                        //Console.WriteLine(item.EmFirstName);
                        ViewBag.EmFN = item.EmFirstName;
                        ViewBag.EmLN = item.EmLastName;
                        ViewBag.EmPos = item.Position;
                    }
                }
                ViewBag.Employee = EmList;
                return View();
            }
            else
            {
                return Redirect("/Admin/Login");
            }
        }
        // AddTable
        [HttpPost]
        public ActionResult AddTable(Table tab)
        {
            List<Table> TableList = this.TableCollection.FindAll().ToList<Table>();
            if (tab != null)
            {
                foreach (var item in TableList)
                {
                    if (item.TableID.Equals(tab.TableID))
                    {
                        return Redirect("/Admin?res=false");
                    }
                }
            }
            tab.Available = true;
            this.TableCollection.Save(tab);
            return Redirect("/Admin?res=true");

        }


        //ManageTable
        public ActionResult ManageTable()
        {
            if (Session["id"] != null)
            {
                List<Employee> EmList = this.EmployeeCollection.FindAll().ToList<Employee>();
                foreach (var item in EmList)
                {
                    //Console.WriteLine(item.EmFirstName);
                    if (item.id.Equals(Session["id"]))
                    {
                        //Console.WriteLine(item.EmFirstName);
                        ViewBag.EmFN = item.EmFirstName;
                        ViewBag.EmLN = item.EmLastName;
                        ViewBag.EmPos = item.Position;
                    }
                }
                ViewBag.Employee = EmList;



                return View();
            }
            else
            {
                return Redirect("/Admin/Login");
            }
        }

        [HttpGet]
        public ActionResult ManageTable(string id)
        {
            if (Session["id"] != null)
            {
                List<Employee> EmList = this.EmployeeCollection.FindAll().ToList<Employee>();
                foreach (var item in EmList)
                {
                    //Console.WriteLine(item.EmFirstName);
                    if (item.id.Equals(Session["id"]))
                    {
                        //Console.WriteLine(item.EmFirstName);
                        ViewBag.EmFN = item.EmFirstName;
                        ViewBag.EmLN = item.EmLastName;
                        ViewBag.EmPos = item.Position;
                    }
                }
                ViewBag.Employee = EmList;

                List<Table> TabList = this.TableCollection.FindAll().ToList<Table>();
                ViewBag.Table = TabList;
                ViewBag.Flag = false;
                foreach (var item in TabList)
                {
                    if (item.id.Equals(ObjectId.Parse(id)))
                    {
                        ViewBag.Flag = true;
                        Session["tabID"] = item.id.ToString();
                        Session["tabname"] = item.TableID.ToString();
                    }
                }
                int billc = int.Parse(this.BillCollection.Count().ToString()) + 1;

                DateTime dt = DateTime.Now;

                ViewBag.Billid = dt.Day.ToString() + dt.Month.ToString() + dt.Year.ToString() + Session["tabname"] + billc.ToString();

                return View();
            }
            else
            {
                return Redirect("/Admin/Login");
            }
        }

        [HttpPost]
        public ActionResult AddBill(string id, Bill bil)
        {
            if (Session["id"] != null)
            {

                List<Bill> BilList = this.BillCollection.FindAll().ToList<Bill>();
                if (bil != null)
                {
                    foreach (var item in BilList)
                    {
                        if (item.BillID.Equals(bil.BillID))
                        {
                            return Redirect("/Admin?res=false");
                        }
                    }
                }
                List<BuffetPrice> BP_List = this.BuffetPriceCollection.FindAll().SetSortOrder(SortBy.Ascending("BuffetID")).ToList<BuffetPrice>();
                ViewBag.BPList = BP_List;

                bil.price = 0;
                foreach (var item in BP_List)
                {
                    if (item.BuffetID == 1)
                    {
                        bil.price += bil.Child * item.BPrice;
                    }
                    else if (item.BuffetID == 2)
                    {
                        bil.price += bil.Adult * item.BPrice;
                    }
                }

                bil.TableID = ObjectId.Parse(Session["tabID"].ToString());
                bil.BillPassword = bil.GenPassword();
                this.BillCollection.Save(bil);

                //List<Table> TabList = this.TableCollection.FindAll().ToList<Table>();
                var query = Query.EQ("_id", ObjectId.Parse(Session["tabID"].ToString()));
                var update = Update<Table>.Set(e => e.Available, false);
                this.TableCollection.Update(query, update);

                return Redirect("/Admin?res=true");
            }
            else
            {
                return Redirect("/Admin/Login");
            }
        }

        public ActionResult deleteOrder(string id)
        {
            int am = this.OrderDetailCollection.FindOneById(ObjectId.Parse(id)).Amount;
            am += this.ProductCollection.FindOneById(this.OrderDetailCollection.FindOneById(ObjectId.Parse(id)).PID).Remain;
            
            var query = Query.EQ("_id", this.OrderDetailCollection.FindOneById(ObjectId.Parse(id)).PID );
            var update = Update<Product>.Set(e => e.Remain, am);
            this.ProductCollection.Update(query, update);

            query = Query.EQ("_id", ObjectId.Parse(id));
            this.OrderDetailCollection.Remove(query);
            
            query = Query.EQ("DetailID._id", ObjectId.Parse(id));
            var updateBill = Update.Pull("DetailID", new BsonDocument(){{ "_id", ObjectId.Parse(id) }});
            this.BillCollection.Update(query, updateBill);

            return Redirect("/Admin/Order");
        }

        public ActionResult serveOrder(string id)
        {
            
            var query = Query.EQ("DetailID._id", ObjectId.Parse(id));
            float price = this.BillCollection.FindOne(query).price;
            price += this.ProductCollection.FindOneById(this.OrderDetailCollection.FindOneById(ObjectId.Parse(id)).PID).PPrice;
            var updateBill = Update<Bill>.Set(e => e.price, price);
            this.BillCollection.Update(query, updateBill);

            query = Query.EQ("_id", ObjectId.Parse(id));
            var updateOD = Update<OrderDetail>.Set(e => e.Status, "Serve");
            this.OrderDetailCollection.Update(query, updateOD);

            return Redirect("/Admin/Order");
        }

        public ActionResult EditPrice()
        {
            if (Session["id"] != null)
            {
                List<Employee> EmList = this.EmployeeCollection.FindAll().ToList<Employee>();
                foreach (var item in EmList)
                {
                    //Console.WriteLine(item.EmFirstName);
                    if (item.id.Equals(Session["id"]))
                    {
                        //Console.WriteLine(item.EmFirstName);
                        ViewBag.EmFN = item.EmFirstName;
                        ViewBag.EmLN = item.EmLastName;
                        ViewBag.EmPos = item.Position;
                    }
                }
                ViewBag.Employee = EmList;
                
                return View();
            }
            else
            {
                return Redirect("/Admin/Login");
            }
        }
        // EditPrice
        [HttpPost]
        public ActionResult EditPrice(BuffetPrice BP)
        {

            if (Session["id"] != null)
            {
                
                var query = Query.EQ("BuffetID", BP.BuffetID);
                var update = Update<BuffetPrice>.Set(e => e.BPrice, BP.BPrice);
                this.BuffetPriceCollection.Update(query, update);
                return Redirect("/Admin/EditPrice");
            }
            else
            {
                return Redirect("/Admin/EditPrice");
            }
        }

    }
}