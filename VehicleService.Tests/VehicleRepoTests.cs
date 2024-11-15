// VehicleRepositoryTests.cs
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleService.DAL;
using VehicleService.DAL.Interfaces;
using VehicleService.DAL.Models;
using VehicleService.DAL.Repositories;
using Xunit;

public class VehicleRepositoryTests
{
    private DbContextOptions<VehicleDbContext> GetInMemoryDbContextOptions()
    {
        return new DbContextOptionsBuilder<VehicleDbContext>()
            .UseInMemoryDatabase(databaseName: "VehicleDatabase")
            .Options;
    }

    private VehicleDbContext GetInMemoryDbContext()
    {
        var options = GetInMemoryDbContextOptions();
        return new VehicleDbContext(options);
    }

    [Fact]
    public async Task GetAllVehicles_ShouldReturnAllVehicles()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        context.Vehicles.AddRange(new List<Vehicle>
        {
            new Vehicle { VIN = "1", ManufacturerName = "Toyota", ModelName = "Corolla", HorsePower = 132, ModelYear = 2020, PurchasePrice = 20000, FuelType = "Gasoline" },
            new Vehicle { VIN = "2", ManufacturerName = "Honda", ModelName = "Civic", HorsePower = 158, ModelYear = 2021, PurchasePrice = 22000, FuelType = "Gasoline" }
        });
        await context.SaveChangesAsync();

        var repository = new VehicleRepository(context);

        // Act
        var vehicles = await repository.GetAllVehicles();

        // Assert
        Assert.Equal(2, vehicles.Count());
    }

    [Fact]
    public async Task GetVehicleById_ShouldReturnCorrectVehicle()
    {
        var context = GetInMemoryDbContext();
        context.Vehicles.Add(new Vehicle { VIN = "1", ManufacturerName = "Toyota", ModelName = "Corolla", HorsePower = 132, ModelYear = 2020, PurchasePrice = 20000, FuelType = "Gasoline" });
        await context.SaveChangesAsync();

        var repository = new VehicleRepository(context);

        var vehicle = await repository.GetVehicleById("1");

        Assert.NotNull(vehicle);
        Assert.Equal("Toyota", vehicle.ManufacturerName);
    }

    [Fact]
    public async Task AddVehicle_ShouldAddVehicle()
    {
        var context = GetInMemoryDbContext();
        var repository = new VehicleRepository(context);
        var vehicle = new Vehicle { VIN = "1", ManufacturerName = "Toyota", ModelName = "Corolla", HorsePower = 132, ModelYear = 2020, PurchasePrice = 20000, FuelType = "Gasoline" };

        await repository.AddVehicle(vehicle);
        var addedVehicle = await context.Vehicles.FindAsync("1");

        Assert.NotNull(addedVehicle);
        Assert.Equal("Toyota", addedVehicle.ManufacturerName);
    }

    [Fact]
    public async Task DeleteVehicle_ShouldRemoveVehicle()
    {
        var context = GetInMemoryDbContext();
        context.Vehicles.Add(new Vehicle { VIN = "1", ManufacturerName = "Toyota", ModelName = "Corolla", HorsePower = 132, ModelYear = 2020, PurchasePrice = 20000, FuelType = "Gasoline" });
        await context.SaveChangesAsync();

        var repository = new VehicleRepository(context);
        await repository.DeleteVehicle("1");
        var deletedVehicle = await context.Vehicles.FindAsync("1");
        Assert.Null(deletedVehicle);
    }

    [Fact]
    public async Task UpdateVehicle_ShouldUpdateVehicle()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        context.Vehicles.Add(new Vehicle { VIN = "1", ManufacturerName = "Toyota", ModelName = "Corolla", HorsePower = 132, ModelYear = 2020, PurchasePrice = 20000, FuelType = "Gasoline" });
        await context.SaveChangesAsync();
        var repository = new VehicleRepository(context);
        var updatedVehicle = new Vehicle { VIN = "1", ManufacturerName = "Honda", ModelName = "Civic", HorsePower = 158, ModelYear = 2021, PurchasePrice = 22000, FuelType = "Gasoline" };
        await repository.UpdateVehicle("1", updatedVehicle);
        var vehicle = await context.Vehicles.FindAsync("1");
        Assert.NotNull(vehicle);
        Assert.Equal("Honda", vehicle.ManufacturerName);
    }
}