using CareerTrack.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CareerTrack.Data.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(CareerTrackDbContext context)
        {
            // Avoid inserting duplicate seed records.
            if (await context.Companies.AnyAsync())
                return;

            // Seed companies
            var companies = new List<Company>
            {
                new() { Name = "Microsoft", Industry = "Technology", Location = "Redmond, WA" },
                new() { Name = "Google", Industry = "Technology", Location = "Mountain View, CA" },
                new() { Name = "Amazon", Industry = "Technology", Location = "Seattle, WA" },
                new() { Name = "Apple", Industry = "Technology", Location = "Cupertino, CA" },
                new() { Name = "IBM", Industry = "Technology", Location = "Armonk, NY" },
                new() { Name = "NVIDIA", Industry = "Semiconductors", Location = "Santa Clara, CA" },
                new() { Name = "Meta", Industry = "Technology", Location = "Menlo Park, CA" },
                new() { Name = "Adobe", Industry = "Software", Location = "San Jose, CA" },
                new() { Name = "Oracle", Industry = "Software", Location = "Austin, TX" },
                new() { Name = "Salesforce", Industry = "Software", Location = "San Francisco, CA" }
            };

            context.Companies.AddRange(companies);

            // Seed application statuses
            var statuses = new List<ApplicationStatus>
            {
                new() { Name = "Saved", Description = "Job saved for later" },
                new() { Name = "Applied", Description = "Application submitted" },
                new() { Name = "Under Review", Description = "Application being reviewed" },
                new() { Name = "Interview", Description = "Interview process started" },
                new() { Name = "Offer", Description = "Job offer received" },
                new() { Name = "Rejected", Description = "Application rejected" },
                new() { Name = "Withdrawn", Description = "Application withdrawn" }
            };

            context.ApplicationStatuses.AddRange(statuses);

            await context.SaveChangesAsync();

            // Seed job applications
            var applications = new List<JobApplication>
            {
                new() { JobTitle = "Software Engineer", CompanyId = companies[0].Id, ApplicationStatusId = statuses[1].Id, DateApplied = new DateTime(2026, 10, 1) },
                new() { JobTitle = "Data Scientist", CompanyId = companies[1].Id, ApplicationStatusId = statuses[2].Id, DateApplied = new DateTime(2026, 10, 2) },
                new() { JobTitle = "Backend Developer", CompanyId = companies[2].Id, ApplicationStatusId = statuses[3].Id, DateApplied = new DateTime(2026, 10, 3) },
                new() { JobTitle = "iOS Developer", CompanyId = companies[3].Id, ApplicationStatusId = statuses[1].Id, DateApplied = new DateTime(2026, 10, 4) },
                new() { JobTitle = "Cloud Engineer", CompanyId = companies[4].Id, ApplicationStatusId = statuses[5].Id, DateApplied = new DateTime(2026, 10, 5) },
                new() { JobTitle = "Machine Learning Engineer", CompanyId = companies[5].Id, ApplicationStatusId = statuses[3].Id, DateApplied = new DateTime(2026, 10, 6) },
                new() { JobTitle = "Frontend Developer", CompanyId = companies[6].Id, ApplicationStatusId = statuses[1].Id, DateApplied = new DateTime(2026, 10, 7) },
                new() { JobTitle = "Full Stack Developer", CompanyId = companies[7].Id, ApplicationStatusId = statuses[4].Id, DateApplied = new DateTime(2026, 10, 8) },
                new() { JobTitle = "Database Engineer", CompanyId = companies[8].Id, ApplicationStatusId = statuses[2].Id, DateApplied = new DateTime(2026, 10, 9) },
                new() { JobTitle = "Software Developer", CompanyId = companies[9].Id, ApplicationStatusId = statuses[1].Id, DateApplied = new DateTime(2026, 10, 10) },
                new() { JobTitle = "AI Engineer", CompanyId = companies[0].Id, ApplicationStatusId = statuses[3].Id, DateApplied = new DateTime(2026, 10, 11) },
                new() { JobTitle = "DevOps Engineer", CompanyId = companies[2].Id, ApplicationStatusId = statuses[5].Id, DateApplied = new DateTime(2026, 10, 12) },
                new() { JobTitle = "Research Engineer", CompanyId = companies[5].Id, ApplicationStatusId = statuses[2].Id, DateApplied = new DateTime(2026, 10, 13) },
                new() { JobTitle = "Systems Engineer", CompanyId = companies[4].Id, ApplicationStatusId = statuses[1].Id, DateApplied = new DateTime(2026, 10, 14) },
                new() { JobTitle = "Security Engineer", CompanyId = companies[1].Id, ApplicationStatusId = statuses[6].Id, DateApplied = new DateTime(2026, 10, 15) }
            };

            context.JobApplications.AddRange(applications);
            await context.SaveChangesAsync();

            // Seed interviews
            context.Interviews.AddRange(
                new Interview { InterviewType = "Phone Screening", InterviewDate = new DateTime(2026, 10, 20, 10, 0, 0), JobApplicationId = applications[2].Id, Outcome = "Passed" },
                new Interview { InterviewType = "Technical Interview", InterviewDate = new DateTime(2026, 10, 25, 14, 0, 0), JobApplicationId = applications[2].Id, Outcome = "Pending" },
                new Interview { InterviewType = "Technical Interview", InterviewDate = new DateTime(2026, 10, 27, 11, 0, 0), JobApplicationId = applications[5].Id, Outcome = "Pending" },
                new Interview { InterviewType = "HR Interview", InterviewDate = new DateTime(2026, 10, 28, 9, 0, 0), JobApplicationId = applications[7].Id, Outcome = "Passed" },
                new Interview { InterviewType = "Final Interview", InterviewDate = new DateTime(2026, 10, 30, 15, 0, 0), JobApplicationId = applications[10].Id, Outcome = "Pending" }
            );

            // Seed contacts
            context.Contacts.AddRange(
                new Contact { FullName = "Sarah Johnson", Email = "sarah@example.com", JobTitle = "Recruiter", CompanyId = companies[0].Id },
                new Contact { FullName = "Michael Brown", Email = "michael@example.com", JobTitle = "Hiring Manager", CompanyId = companies[2].Id },
                new Contact { FullName = "Emily Davis", Email = "emily@example.com", JobTitle = "Technical Recruiter", CompanyId = companies[5].Id }
            );

            await context.SaveChangesAsync();
        }
    }
}

