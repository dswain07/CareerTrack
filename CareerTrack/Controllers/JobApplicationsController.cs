using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CareerTrack.Data.Data;
using CareerTrack.Data.Models;

namespace CareerTrack.Controllers
{
    public class JobApplicationsController : Controller
    {
        private readonly CareerTrackDbContext _context;

        public JobApplicationsController(CareerTrackDbContext context)
        {
            _context = context;
        }

        // GET: JobApplications
        public async Task<IActionResult> Index(string? jobTitle, string? location)
        {
            var jobApplications = _context.JobApplications
                .Include(j => j.ApplicationStatus)
                .Include(j => j.Company)
                .AsQueryable();

            // Case-insensitive search by Job Title
            if (!string.IsNullOrWhiteSpace(jobTitle))
            {
                jobApplications = jobApplications
                    .Where(j => EF.Functions.Like(
                        j.JobTitle, $"%{jobTitle}%"));
            }

            // Case-insensitive search by Location
            if (!string.IsNullOrWhiteSpace(location))
            {
                jobApplications = jobApplications
                    .Where(j => j.Location != null &&
                        EF.Functions.Like(
                            j.Location, $"%{location}%"));
            }

            // Preserve search values
            ViewData["JobTitleSearch"] = jobTitle;
            ViewData["LocationSearch"] = location;

            return View(await jobApplications.ToListAsync());
        }

        // GET: JobApplications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _context.JobApplications
                .Include(j => j.ApplicationStatus)
                .Include(j => j.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobApplication == null)
            {
                return NotFound();
            }

            return View(jobApplication);
        }

        // GET: JobApplications/Create
        public IActionResult Create()
        {
            ViewData["ApplicationStatusId"] = new SelectList(_context.ApplicationStatuses, "Id", "Name");
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name");
            return View();
        }

        // POST: JobApplications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,JobTitle,DateApplied,Location,JobUrl,Salary,Notes,CompanyId,ApplicationStatusId")] JobApplication jobApplication)
        {
            foreach (var entry in ModelState)
            {
                foreach (var error in entry.Value.Errors)
                {
                    Console.WriteLine($"Validation Error - {entry.Key}: {error.ErrorMessage}");
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(jobApplication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationStatusId"] = new SelectList(_context.ApplicationStatuses, "Id", "Name", jobApplication.ApplicationStatusId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", jobApplication.CompanyId);
            return View(jobApplication);
        }

        // GET: JobApplications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _context.JobApplications.FindAsync(id);
            if (jobApplication == null)
            {
                return NotFound();
            }
            ViewData["ApplicationStatusId"] = new SelectList(_context.ApplicationStatuses, "Id", "Name", jobApplication.ApplicationStatusId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", jobApplication.CompanyId);
            return View(jobApplication);
        }

        // POST: JobApplications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,JobTitle,DateApplied,Location,JobUrl,Salary,Notes,CompanyId,ApplicationStatusId")] JobApplication jobApplication)
        {
            if (id != jobApplication.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobApplicationExists(jobApplication.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationStatusId"] = new SelectList(_context.ApplicationStatuses, "Id", "Name", jobApplication.ApplicationStatusId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", jobApplication.CompanyId);
            return View(jobApplication);
        }

        // GET: JobApplications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _context.JobApplications
                .Include(j => j.ApplicationStatus)
                .Include(j => j.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobApplication == null)
            {
                return NotFound();
            }

            return View(jobApplication);
        }

        // POST: JobApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobApplication = await _context.JobApplications.FindAsync(id);
            if (jobApplication != null)
            {
                _context.JobApplications.Remove(jobApplication);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobApplicationExists(int id)
        {
            return _context.JobApplications.Any(e => e.Id == id);
        }
    }
}
