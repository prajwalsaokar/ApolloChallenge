using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleService.DAL;
using VehicleService.DAL.Models;
using VehicleService.DAL.Repositories;
using Xunit;

public class VehicleRepositoryTests : IDisposable
{
    private VehicleDbContext _context;
    private DbContextOptions<VehicleDbContext> GetInMemoryDbContextOptions()
    {
        return new DbContextOptionsBuilder<VehicleDbContext>()
            .UseInMemoryDatabase(databaseName: "VehicleDatabase")
            .Options;
    }

    private VehicleDbContext GetInMemoryDbContext()
    {
        if (_context != null)
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        var options = GetInMemoryDbContextOptions();
        _context = new VehicleDbContext(options);
        return _context;
    }
    public void Dispose()
    {
        if (_context != null)
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            _context = null;
        }
    }
    [Fact]
    public async Task TestGetAllVehicles()
    {
        var context = GetInMemoryDbContext();
        context.Vehicles.AddRange(new List<Vehicle>
        {
            new Vehicle { VIN = "1HGCM82633A123456", ManufacturerName = "Toyota", ModelName = "Corolla", Description = "Compact car", HorsePower = 132, ModelYear = 2020, PurchasePrice = 20000m, FuelType = "Gasoline" },
            new Vehicle { VIN = "2HGCM82633A123456", ManufacturerName = "Honda", ModelName = "Civic", Description = "Sedan", HorsePower = 158, ModelYear = 2021, PurchasePrice = 22000m, FuelType = "Gasoline" }
        });
        await context.SaveChangesAsync();

        var repository = new VehicleRepository(context);
        var vehicles = await repository.GetAllVehicles();

        Assert.Equal(2, vehicles.Count());
    }

    [Fact]
    public async Task TestGetVehicleById()
    {
        var context = GetInMemoryDbContext();
        context.Vehicles.Add(new Vehicle { VIN = "1HGCM82633A123456", ManufacturerName = "Toyota", ModelName = "Corolla", Description = "Compact car", HorsePower = 132, ModelYear = 2020, PurchasePrice = 20000m, FuelType = "Gasoline" });
        await context.SaveChangesAsync();

        var repository = new VehicleRepository(context);
        var vehicle = await repository.GetVehicleById("1HGCM82633A123456");

        Assert.NotNull(vehicle);
        Assert.Equal("Toyota", vehicle.ManufacturerName);
    }

    [Fact]
    public async Task TestAddVehicle()
    {
        var context = GetInMemoryDbContext();
        var repository = new VehicleRepository(context);
        var vehicle = new Vehicle { VIN = "1HGCM82633A123456", ManufacturerName = "Toyota", ModelName = "Corolla", Description = "Compact car", HorsePower = 132, ModelYear = 2020, PurchasePrice = 20000m, FuelType = "Gasoline" };

        await repository.AddVehicle(vehicle);
        var addedVehicle = await context.Vehicles.FindAsync("1HGCM82633A123456");

        Assert.NotNull(addedVehicle);
        Assert.Equal("Toyota", addedVehicle.ManufacturerName);
    }

    [Fact]
    public async Task TestDeleteVehicle()
    {
        var context = GetInMemoryDbContext();
        context.Vehicles.Add(new Vehicle { VIN = "1HGCM82633A123456", ManufacturerName = "Toyota", ModelName = "Corolla", Description = "Compact car", HorsePower = 132, ModelYear = 2020, PurchasePrice = 20000m, FuelType = "Gasoline" });
        await context.SaveChangesAsync();

        var repository = new VehicleRepository(context);
        await repository.DeleteVehicle("1HGCM82633A123456");
        var deletedVehicle = await context.Vehicles.FindAsync("1HGCM82633A123456");

        Assert.Null(deletedVehicle);
    }

    [Fact]
    public async Task TestUpdateVehicle()
    {
        var context = GetInMemoryDbContext();
        context.Vehicles.Add(new Vehicle { VIN = "1HGCM82633A123456", ManufacturerName = "Toyota", ModelName = "Corolla", Description = "Compact car", HorsePower = 132, ModelYear = 2020, PurchasePrice = 20000m, FuelType = "Gasoline" });
        await context.SaveChangesAsync();

        var repository = new VehicleRepository(context);
        var updatedVehicle = new Vehicle { VIN = "1HGCM82633A123456", ManufacturerName = "Honda", ModelName = "Civic", Description = "Updated description", HorsePower = 158, ModelYear = 2021, PurchasePrice = 22000m, FuelType = "Gasoline" };

        await repository.UpdateVehicle("1HGCM82633A123456", updatedVehicle);
        var vehicle = await context.Vehicles.FindAsync("1HGCM82633A123456");

        Assert.NotNull(vehicle);
        Assert.Equal("Honda", vehicle.ManufacturerName);
        Assert.Equal("Updated description", vehicle.Description);
    }
}
