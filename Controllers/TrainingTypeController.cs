using System.Linq;
using System.Net;
using System.Web.Mvc;
using PetCare_system.Models;
using System.Data.Entity;

namespace PetCare_system.Controllers
{
    public class TrainingTypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainingTypeController()
        {
            _context = new ApplicationDbContext();
        }

        // Dispose the context when the controller is disposed
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: TrainingType
        public ActionResult Index()
        {
            var trainingTypes = _context.TrainingTypes.ToList();
            return View(trainingTypes);
        }

        // GET: TrainingType/Details/5
        public ActionResult Details(int id)
        {
            var trainingType = _context.TrainingTypes.SingleOrDefault(t => t.TrainingTypeId == id);
            if (trainingType == null)
            {
                return HttpNotFound();
            }

            return View(trainingType);
        }

        // GET: TrainingType/Create
        public ActionResult Create()
        {
            return View(new TrainingType());
        }

        // POST: TrainingType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TrainingType trainingType)
        {
            if (ModelState.IsValid)
            {
                _context.TrainingTypes.Add(trainingType);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trainingType);
        }

        // GET: TrainingType/Edit/5
        public ActionResult Edit(int id)
        {
            var trainingType = _context.TrainingTypes.SingleOrDefault(t => t.TrainingTypeId == id);
            if (trainingType == null)
            {
                return HttpNotFound();
            }

            return View(trainingType);
        }

        // POST: TrainingType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TrainingType trainingType)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(trainingType).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(trainingType);
        }

        // GET: TrainingType/Delete/5
        public ActionResult Delete(int id)
        {
            var trainingType = _context.TrainingTypes.SingleOrDefault(t => t.TrainingTypeId == id);
            if (trainingType == null)
            {
                return HttpNotFound();
            }

            return View(trainingType);
        }

        // POST: TrainingType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var trainingType = _context.TrainingTypes.SingleOrDefault(t => t.TrainingTypeId == id);
            if (trainingType == null)
            {
                return HttpNotFound();
            }

            _context.TrainingTypes.Remove(trainingType);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
