using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Models.Entities;

namespace Companies.Infrastructure.Data
{
    public class CompaniesContext : DbContext
    {
        public CompaniesContext (DbContextOptions<CompaniesContext> options)
            : base(options)
        {
        }

        // public DbSet<Company> Companies { get; set; } = default!;
        public DbSet<Company> Companies => Set<Company>();

        public DbSet<Employee> Employees { get; set; } = default!;
    }
}
