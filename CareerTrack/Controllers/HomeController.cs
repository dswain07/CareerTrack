using CareerTrack.Data.Data;
using CareerTrack.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CareerTrack.Controllers
{
    public class HomeController : Controller
    {
        private readonly CareerTrackDbContext _context;

        public HomeController(CareerTrackDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel
            {
                TotalCompanies = await _context.Companies.CountAsync(),

                TotalApplications = await _context.JobApplications.CountAsync(),

                TotalInterviews = await _context.Interviews.CountAsync(),

                TotalOffers = await _context.JobApplications
                    .CountAsync(j =>
                        j.ApplicationStatus != null &&
                        j.ApplicationStatus.Name == "Offer"),

                RecentApplications = await _context.JobApplications
                    .Include(j => j.Company)
                    .Include(j => j.ApplicationStatus)
                    .AsNoTracking()
                    .OrderByDescending(j => j.DateApplied)
                    .ThenByDescending(j => j.Id)
                    .Take(5)
                    .ToListAsync()
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id
                    ?? HttpContext.TraceIdentifier
            });
        }
    }
}
