using CareerTrack.Data.Data;
using CareerTrack.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CareerTrack.Controllers
{
    public class AdminController : Controller
    {
        private readonly CareerTrackDbContext _context;

        public AdminController(CareerTrackDbContext context)
        {
            _context = context;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            var statuses = await _context.ApplicationStatuses
                .AsNoTracking()
                .OrderBy(s => s.Name)
                .ToListAsync();

            return View(statuses);
        }

        // GET: Admin/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name,Description")] ApplicationStatus status)
        {
            if (!ModelState.IsValid)
            {
                return View(status);
            }

            _context.ApplicationStatuses.Add(status);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Application status created successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var status = await _context.ApplicationStatuses
                .FindAsync(id.Value);

            if (status == null)
            {
                return NotFound();
            }

            return View(status);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Name,Description")] ApplicationStatus input)
        {
            if (id != input.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(input);
            }

            var status = await _context.ApplicationStatuses
                .FindAsync(id);

            if (status == null)
            {
                return NotFound();
            }

            status.Name = input.Name;
            status.Description = input.Description;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Application status updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var status = await _context.ApplicationStatuses
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id.Value);

            if (status == null)
            {
                return NotFound();
            }

            return View(status);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var status = await _context.ApplicationStatuses
                .FindAsync(id);

            if (status == null)
            {
                return NotFound();
            }

            // Prevent deleting statuses used by job applications.
            bool isInUse = await _context.JobApplications
                .AnyAsync(j => j.ApplicationStatusId == id);

            if (isInUse)
            {
                TempData["ErrorMessage"] =
                    "Cannot delete this status because it is used by existing job applications.";

                return RedirectToAction(nameof(Index));
            }

            _context.ApplicationStatuses.Remove(status);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] =
                    "Unable to delete this status because it is referenced by another record.";

                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] =
                "Application status deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
