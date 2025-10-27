using SimpleNewsPortal.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace SimpleNewsPortal.Data
{
 

        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            { }

            public DbSet<News> News { get; set; }
        }
    }

