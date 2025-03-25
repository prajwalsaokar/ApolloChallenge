using Microsoft.EntityFrameworkCore;
using VehicleService.DAL.Models;

namespace VehicleService.DAL
{
    public class VehicleDbContext: DbContext
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Vehicle> SoldVehicles { get; set; }

        public string DbPath { get; }

        public VehicleDbContext(DbContextOptions<VehicleDbContext> options) : base(options)
        {
            var basePath = Directory.GetCurrentDirectory(); 
            var dataFolder = Path.Combine(basePath, "Database");
            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            DbPath = Path.Combine(dataFolder, "vehicles.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlite($"Data Source={DbPath}");
            }
        }
    }
}
    