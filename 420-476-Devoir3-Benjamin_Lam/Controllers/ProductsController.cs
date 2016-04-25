using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _420_476_Devoir3_Benjamin_Lam;
using System.IO;
using System.Drawing;

namespace _420_476_Devoir3_Benjamin_Lam.Controllers
{
    public class ProductsController : Controller
    {
        private NorthwindEntities db = new NorthwindEntities();
        private string ImagePath = "~/Content/Photos/";

        // GET: Products
        public ActionResult Index()
        {
            if (Session["ConnectedUser"] != null)
            {
                ViewBag.ImagePath = ImagePath;
                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
                var products = db.Products.Include(p => p.Category).Include(p => p.Supplier);
                return View(products.ToList());
            }
            else {
                return RedirectToAction("Login", "Connect");
            }
        }

        [HttpPost]
        public ActionResult Index(int categoryID)
        {
            if (Session["ConnectedUser"] != null)
            {
                ViewBag.ImagePath = ImagePath;
                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
                var products = db.Products.Where(p => p.Category.CategoryID == categoryID).Include(p => p.Category).Include(p => p.Supplier);
                return View(products.ToList());
            }
            else {
                return RedirectToAction("Login", "Connect");
            }
        }
        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["ConnectedUser"] != null)
            {
                ViewBag.ImagePath = ImagePath;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Product product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                return View(product);
            }
            else {
                return RedirectToAction("Login", "Connect");
            }
        }

        // GET: Products/Create
        public ActionResult Add()
        {
            if (Session["ConnectedUser"] != null)
            {
                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
                ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
                return View();
            }
            else {
                return RedirectToAction("Login", "Connect");
            }
        }

        // POST: Products/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued,Photo")] Product product)
        {
            if (Session["ConnectedUser"] != null)
            {
                if (ModelState.IsValid)
                {
                    if (Request.Files.Count  > 0)
                    {
                        var file = Request.Files[0];
                        if (MimeMapping.GetMimeMapping(file.FileName) == "image/jpg" || MimeMapping.GetMimeMapping(file.FileName) == "image/png")
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath("~/Content/Photos/"), fileName); 
                            file.SaveAs(path);
                            product.Photo = fileName;
                        }
                        else {
                            ViewBag.PhotoValidation = "fail";
                        }
                    }
                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
                ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", product.SupplierID);
                return View(product);
            }
            else {
                return RedirectToAction("Login", "Connect");
            }
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["ConnectedUser"] != null)
            {
                ViewBag.ImagePath = ImagePath;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Product product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
                ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", product.SupplierID);
                return View(product);
            }
            else {
                return RedirectToAction("Login", "Connect");
            }
        }

        // POST: Products/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued,Photo")] Product product)
        {
            if (Session["ConnectedUser"] != null)
            {
                //var prodID = db.Products.Where(p => p.CategoryID == product.CategoryID);
                //string tempProd = (string)prodID.ToList()[0].Photo;
                //prodID = null;
                //ViewBag.ImagePath = ImagePath;
                if (ModelState.IsValid)
                {
                    if (Request.Files.Count > 0)
                    {
                        var file = Request.Files[0];
                        if (MimeMapping.GetMimeMapping(file.FileName) == "image/jpg" || MimeMapping.GetMimeMapping(file.FileName) == "image/png" || MimeMapping.GetMimeMapping(file.FileName) == "image/jpeg")
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath("~/Content/Photos/"), fileName);
                            file.SaveAs(path);
                            product.Photo = fileName;
                        }
                        else {
                            ViewBag.PhotoValidation = "fail";
                        }
                    }
                    //else if (tempProd != null)
                    //{
                    //    product.Photo = tempProd;
                    //}
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
                ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", product.SupplierID);
                return View(product);
            }
            else {
                return RedirectToAction("Login", "Connect");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
