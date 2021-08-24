using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestCaseMVC.Models;

namespace TestCaseMVC.Data
{
    public class TestCaseMVCContext : DbContext
    {
        public TestCaseMVCContext (DbContextOptions<TestCaseMVCContext> options)
            : base(options)
        {
        }

        public DbSet<TestCaseMVC.Models.Project> Project { get; set; }
    }
}
