using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleService.DAL.Interfaces;
using VehicleService.DAL.Models;
using VehicleService.DAL.Repositories;
namespace VehicleService.Controllers
{
    [ApiController]
    [Route("vehicle")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleController(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetAllVehicles()
        {
            var vehicles = await _vehicleRepository.GetAllVehicles();
            return Ok(vehicles);
        }

        [HttpPost]
        public async Task<ActionResult<Vehicle>> CreateVehicle([FromBody] Vehicle vehicle)
        {
            if (vehicle == null)
            {
                return BadRequest("Vehicle data is invalid.");
            }

            await _vehicleRepository.AddVehicle(vehicle);
            return CreatedAtAction(nameof(GetVehicleById), new { vin = vehicle.VIN }, vehicle);
        }

        [HttpGet("{vin}")]
        public async Task<ActionResult<Vehicle>> GetVehicleById(string vin)
        {
            var vehicle = await _vehicleRepository.GetVehicleById(vin);
            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);
        }

        [HttpPut("{vin}")]
        public async Task<IActionResult> UpdateVehicle(string vin, [FromBody] Vehicle updatedVehicle)
        {
            if (updatedVehicle == null || vin != updatedVehicle.VIN)
            {
                return BadRequest("Vehicle data is invalid or VIN mismatch.");
            }

            await _vehicleRepository.UpdateVehicle(vin, updatedVehicle);
            return Ok("Vehicle updated successfully.");
        }

        [HttpDelete("{vin}")]
        public async Task<IActionResult> DeleteVehicle(string vin)
        {
            await _vehicleRepository.DeleteVehicle(vin);
            return NoContent();
        }
    }
}
