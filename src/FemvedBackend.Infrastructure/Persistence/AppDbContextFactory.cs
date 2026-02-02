using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FemvedBackend.Infrastructure.Persistence
{
    public class AppDbContextFactory
     : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseNpgsql(
                "Host=localhost;Port=5432;Database=FemVed;Username=postgres;Password=admin"
            );

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
