
using Microsoft.AspNetCore.Mvc;

namespace CareerTrack.Controllers
{
    public class SettingsController : Controller
    {
        // GET: Settings
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}