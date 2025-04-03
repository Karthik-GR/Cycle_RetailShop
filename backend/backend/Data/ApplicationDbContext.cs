using backend.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<ImageModel> Images { get; set; }

        public DbSet<Brand> CycleBrands { get; set; }
        public DbSet<CycleType> CycleTypes { get; set; }

    }
}