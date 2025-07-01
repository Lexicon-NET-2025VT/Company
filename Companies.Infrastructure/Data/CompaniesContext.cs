using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Companies.Infrastructure.Data
{
    public class CompaniesContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public CompaniesContext (DbContextOptions<CompaniesContext> options)
            : base(options)
        {
        }

        // public DbSet<Company> Companies { get; set; } = default!;
        public DbSet<Company> Companies => Set<Company>();

        // public DbSet<ApplicationUser> Employees { get; set; } = default!;
    }
}
