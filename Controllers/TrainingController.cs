using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using PetCare_system.Models;

namespace PetCare_system.Controllers
{
    public class TrainingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Training
        public ActionResult Index()
        {
            var trainings = db.Trainings.Include(t => t.Pet).Include(t => t.User).Include(t => t.TrainingType);
            return View(trainings.ToList());
        }

        // GET: Training/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Training training = db.Trainings.Include(t => t.Pet)
                                            .Include(t => t.User)
                                            .Include(t => t.TrainingType)
                                            .FirstOrDefault(t => t.TrainingId == id);

            if (training == null)
                return HttpNotFound();

            return View(training);
        }

      
            public ActionResult Create()
            {
                // Get pets with owner information
                var pets = db.pets.Include(p => p.User)
                                  .Select(p => new {
                                      p.Id,
                                      DisplayName = p.Name + " (" + p.User.FirstName + " " + p.User.LastName + ")"
                                  });

                ViewBag.PetId = new SelectList(pets, "Id", "DisplayName");
                ViewBag.TrainingTypeId = new SelectList(db.TrainingTypes.OrderBy(t => t.Name), "TrainingTypeId", "Name");

                // Set default date to today
                var model = new Training
                {
                    TrainingDate = DateTime.Today,
                    TrainingStatus = "Scheduled" // Default status
                };

                return View(model);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Create(Training training)
            {
                if (ModelState.IsValid)
                {
                    // Set created date and default values if needed
                    training.TrainingDate = DateTime.Now;

                    db.Trainings.Add(training);
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Training session created successfully!";
                    return RedirectToAction("Details", new { id = training.TrainingId });
                }

                // Repopulate dropdowns if validation fails
                var pets = db.pets.Include(p => p.User)
                                  .Select(p => new {
                                      p.Id,
                                      DisplayName = p.Name + " (" + p.User.FirstName + " " + p.User.LastName + ")"
                                  });

                ViewBag.PetId = new SelectList(pets, "Id", "DisplayName", training.PetId);
                ViewBag.TrainingTypeId = new SelectList(db.TrainingTypes.OrderBy(t => t.Name), "TrainingTypeId", "Name", training.TrainingTypeId);

                return View(training);
            }

            // ... other actions remain similar but with improved error handling
        
        // GET: Training/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Training training = db.Trainings.Find(id);
            if (training == null)
                return HttpNotFound();

            ViewBag.PetId = new SelectList(db.pets, "Id", "Name", training.PetId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", training.UserId);
            ViewBag.TrainingTypeId = new SelectList(db.TrainingTypes, "TrainingTypeId", "Name", training.TrainingTypeId);
            return View(training);
        }

        // POST: Training/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TrainingId,Id,UserId,TrainingTypeId,TrainingDate,TrainingDuration,TrainingCost,TrainingLocation,TrainingStatus,ProgressNotes,NextSessionDate,IsGroupSession,PetBehaviorBefore,PetBehaviorAfter")] Training training)
        {
            if (ModelState.IsValid)
            {
                db.Entry(training).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PetId = new SelectList(db.pets, "Id", "Name", training.PetId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", training.UserId);
            ViewBag.TrainingTypeId = new SelectList(db.TrainingTypes, "TrainingTypeId", "Name", training.TrainingTypeId);
            return View(training);
        }

        // GET: Training/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Training training = db.Trainings.Include(t => t.Pet)
                                            .Include(t => t.User)
                                            .Include(t => t.TrainingType)
                                            .FirstOrDefault(t => t.TrainingId == id);
            if (training == null)
                return HttpNotFound();

            return View(training);
        }

        // POST: Training/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Training training = db.Trainings.Find(id);
            db.Trainings.Remove(training);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
     
        public async Task<JsonResult> GetTrainingTypeDetails(int id)
        {
            var type = await db.TrainingTypes.FindAsync(id);
            if (type != null)
            {
                return Json(new
                {
                    Name = type.Name,
                    DefaultDuration = type.DefaultDuration,
                    DefaultCost = type.DefaultCost
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { }, JsonRequestBehavior.AllowGet);  // Return an empty response if not found
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}
