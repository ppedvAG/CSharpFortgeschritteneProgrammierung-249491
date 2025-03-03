using BusinessLogic.Contracts;
using Microsoft.AspNetCore.Mvc;
using WeatherApi.Mappers;
using WeatherApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WeatherApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehiclesService vehiclesService;

        public VehiclesController(IVehiclesService vehiclesService)
        {
            this.vehiclesService = vehiclesService;
        }

        // GET: api/<VehiclesController>
        [HttpGet]
        public IActionResult Get()
        {
            var result = vehiclesService.GetAll().Select(e => e.ToDto());
            return Ok(result);
        }

        // GET api/<VehiclesController>/5
        [HttpGet("{id}")]
        public ActionResult<VehicleDto?> Get(Guid id)
        {
            var result = vehiclesService.GetById(id);
            if (result != null)
            {
                //return new OkObjectResult(result);
                return Ok(result.ToDto()); // 200
            }

            return NotFound(); // 404
        }

        // POST api/<VehiclesController>
        [HttpPost]
        public IActionResult Post([FromBody] VehicleDto dto)
        {
            string id = vehiclesService.Create(dto.ToEntity());
            return Ok(id); // 200
        }

        // PUT api/<VehiclesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] VehicleDto dto)
        {
            var success = vehiclesService.Update(id, dto.ToEntity());
            if (!success)
            {
                return NotFound(); // 404
            }
            return NoContent(); // 204
        }

        // DELETE api/<VehiclesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var success = vehiclesService.Delete(id);
            if (!success)
            {
                return NotFound(); // 404
            }
            return NoContent(); // 204
        }
    }
}
