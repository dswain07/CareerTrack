
using CareerTrack.Data.Models;

namespace CareerTrack.Models
{
    public class DashboardViewModel
    {
        public int TotalCompanies { get; set; }

        public int TotalApplications { get; set; }

        public int TotalInterviews { get; set; }

        public int TotalOffers { get; set; }

        public List<JobApplication> RecentApplications { get; set; }
            = new List<JobApplication>();
    }
}
