using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using UrbanMediMarts.Models;

namespace UrbanMediMarts.Controllers
{
    public class CustomersController : Controller
    {
        private UrbanMediMartEntities db = new UrbanMediMartEntities();

        // GET: Customers
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(Customer cs)
        {
            using (var Context = new UrbanMediMartEntities())
            {
                bool isValid = Context.Customers.Any(x => x.Email == cs.Email && x.Password == cs.Password);
                if (isValid)
                {
                    FormsAuthentication.SetAuthCookie(cs.Email,false);
                    return RedirectToAction("Index","Home");
                }
                ModelState.AddModelError("", "Invalid Email or password /you are Not resistered");
            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        public ActionResult SignIn()
        {

            return View();
        }
        [HttpPost]
        public ActionResult SignIn(Customer cs)
        {
            using (var context = new UrbanMediMartEntities())
            {
                context.Customers.Add(cs);
                context.SaveChanges();
            }
                return RedirectToAction("Login");
        }
        // GET: Customers/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerID,Name,Email,Password,Address,City,Country,PhoneNumber,ZipCode,RegistrationDate")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5 [Authorize]
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,Name,Email,Password,Address,City,Country,PhoneNumber,ZipCode,RegistrationDate")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }
        [Authorize]
        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
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
