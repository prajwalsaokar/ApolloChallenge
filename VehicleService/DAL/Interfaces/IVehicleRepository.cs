using VehicleService.DAL.Models;
namespace VehicleService.DAL.Interfaces
{

    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetAllVehicles();
        Task<Vehicle> GetVehicleById(string vin);
        Task AddVehicle(Vehicle vehicle);
        Task UpdateVehicle(string vin, Vehicle updatedVehicle);
        Task DeleteVehicle(string vin);
        Task<Vehicle> SellVehicle(string vin);
        Task<IEnumerable<Vehicle>> GetAllSoldVehicles();
    }

}