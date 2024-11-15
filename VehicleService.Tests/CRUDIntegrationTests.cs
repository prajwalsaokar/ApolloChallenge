using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VehicleService.DAL.Models;
using VehicleService.DAL;
using Xunit;

public class CRUDIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CRUDIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task TestAddVehicleEndpoint()
    {
        var newVehicle = new Vehicle
        {
            VIN = "3HGCM82633A123456",
            ManufacturerName = "Ford",
            ModelName = "Focus",
            Description = "Compact car",
            HorsePower = 150,
            ModelYear = 2022,
            PurchasePrice = 25000m,
            FuelType = "Gasoline"
        };

        var response = await _client.PostAsJsonAsync("/vehicle", newVehicle);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
        Assert.Contains("/vehicle/3HGCM82633A123456", response.Headers.Location.ToString());
    }

    [Fact]
    public async Task TestGetAllVehiclesEndpoint()
    {
        var response = await _client.GetAsync("/vehicle");
        response.EnsureSuccessStatusCode();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task TestGetVehicleByIdEndpoint()
    {
        var response = await _client.GetAsync("/vehicle/1HGCM82633A123456");
        response.EnsureSuccessStatusCode();

        var vehicle = await response.Content.ReadFromJsonAsync<Vehicle>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(vehicle);
        Assert.Equal("1HGCM82633A123456", vehicle.VIN);
    }

    [Fact]
    public async Task TestUpdateVehicleEndpoint()
    {
        var updatedVehicle = new Vehicle
        {
            VIN = "1HGCM82633A123456",
            ManufacturerName = "Updated Manufacturer",
            ModelName = "Updated Model",
            Description = "Updated description",
            HorsePower = 200,
            ModelYear = 2023,
            PurchasePrice = 30000m,
            FuelType = "Electric"
        };

        var response = await _client.PutAsJsonAsync("/vehicle/1HGCM82633A123456", updatedVehicle);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task TestDeleteVehicleEndpoint()
    {
        var response = await _client.DeleteAsync("/vehicle/1HGCM82633A123456");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<VehicleDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add SQLite in-memory database
            services.AddDbContext<VehicleDbContext>(options =>
            {
                options.UseSqlite("DataSource=:memory:");
            });

            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<VehicleDbContext>();
                db.Database.OpenConnection(); 
                db.Database.EnsureCreated();  
            }
        });
    }
}
