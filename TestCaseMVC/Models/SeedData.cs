using System;
using System.Linq;
using TestCaseMVC.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TestCaseMVC.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new TestCaseMVCContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<TestCaseMVCContext>>()))
            {
                // Look for any movies.
                if (context.Project.Any())
                {
                    return;   // DB has been seeded
                }

                context.Project.AddRange(
                    new Project
                    {
                        Name = "PLACE",
                        Author = "sanweiliti",
                        Stargazers = 5,
                        Watchers = 32,
                        Url = $"https://github.com/sanweiliti/PLACE"

                    },

                    new Project
                    {
                        Name = "kubescape",
                        Author = "armosec",
                        Stargazers = 4,
                        Watchers = 1000,
                        Url = "https://github.com/armosec/kubescape"
                    },

                    new Project
                    {
                        Name = "ToDo_project",
                        Author = "lexastav",
                        Stargazers = 0,
                        Watchers = 0,
                        Url = "https://github.com/lexastav/ToDo_project"
                    }


                );
                context.SaveChanges();
            }
        }
    }
}

