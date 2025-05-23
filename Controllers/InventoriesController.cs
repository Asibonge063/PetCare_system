
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PetCare_system.Models;

namespace PetCare_system.Controllers
{
    public class InventoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Inventories
        public ActionResult Index()
        {
            return View(db.Inventory.ToList());
        }

        // GET: Inventories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventory.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // GET: Inventories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inventories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InventoryId,MedicationName,InventoryType,InventoryQuantity,MedDiscription")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                db.Inventory.Add(inventory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(inventory);
        }

        // GET: Inventories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventory.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // POST: Inventories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InventoryId,MedicationName,InventoryType,InventoryQuantity,MedDiscription")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inventory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(inventory);
        }

        // GET: Inventories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = db.Inventory.Find(id);
            if (inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // POST: Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inventory inventory = db.Inventory.Find(id);
            db.Inventory.Remove(inventory);
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

        // GET: Inventories/AddStock/5
        public ActionResult AddStock(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var inventory = db.Inventory.FirstOrDefault(i => i.InventoryId == id);
            if (inventory == null)
                return HttpNotFound();

            return View(inventory);
        }

        // POST: Inventories/AddStock/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStock(int id, int quantityToAdd)
        {
            var inventory = db.Inventory.FirstOrDefault(i => i.InventoryId == id);
            if (inventory == null)
                return HttpNotFound();

            // LINQ-based update
            inventory.InventoryQuantity = inventory.InventoryQuantity + quantityToAdd;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Inventories/SubtractStock/5
        public ActionResult SubtractStock(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var inventory = db.Inventory.FirstOrDefault(i => i.InventoryId == id);
            if (inventory == null)
                return HttpNotFound();

            return View(inventory);
        }

        // POST: Inventories/SubtractStock/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubtractStock(int id, int quantityToSubtract)
        {
            var inventory = db.Inventory.FirstOrDefault(i => i.InventoryId == id);
            if (inventory == null)
                return HttpNotFound();

            if (quantityToSubtract > inventory.InventoryQuantity)
            {
                ModelState.AddModelError("", "Cannot subtract more than available.");
                return View(inventory);
            }

            // LINQ-based update
            inventory.InventoryQuantity = inventory.InventoryQuantity - quantityToSubtract;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}