using Microsoft.EntityFrameworkCore;
using VehicleService.DAL.Models;
using VehicleService.DAL.Interfaces;

namespace VehicleService.DAL.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VehicleDbContext _context;
        
        public VehicleRepository(VehicleDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Vehicle>> GetAllVehicles()
        {
            return await _context.Vehicles.ToListAsync();
        }

        public async Task<Vehicle> GetVehicleById(string vin)
        {
            vin = vin.ToUpper();
            return await _context.Vehicles.FirstOrDefaultAsync(v => v.VIN == vin);
        }

        public async Task AddVehicle(Vehicle vehicle)
        {
            vehicle.VIN = vehicle.VIN.ToUpper();
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteVehicle(string vin)
        {
            vin = vin.ToUpper();
            var vehicleToDelete = await _context.Vehicles.FirstOrDefaultAsync(v => v.VIN == vin);
            if (vehicleToDelete != null)
            {
                _context.Vehicles.Remove(vehicleToDelete);
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateVehicle(string vin, Vehicle updatedVehicle)
        {
            vin = vin.ToUpper();
            var existingVehicle = await _context.Vehicles.FindAsync(vin);
            if (existingVehicle != null)
            {
                _context.Entry(existingVehicle).CurrentValues.SetValues(updatedVehicle);
                await _context.SaveChangesAsync();
            }
        }  

        public async Task<Vehicle> SellVehicle(string vin)
        {
            vin = vin.ToUpper();
            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.VIN == vin);
            if (vehicle == null)
            {
                return null;
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SoldVehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
            
            return vehicle;
        }

        public async Task<IEnumerable<Vehicle>> GetAllSoldVehicles()
        {
            return await _context.SoldVehicles.ToListAsync();
        }
    }
}