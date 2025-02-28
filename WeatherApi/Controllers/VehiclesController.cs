using LinqSamples.Data;
using Microsoft.AspNetCore.Mvc;
using WeatherApi.Contracts;
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
        public IEnumerable<VehicleDto> Get()
        {
            return vehiclesService.GetAll().Select(e => e.ToDto());
        }

        // GET api/<VehiclesController>/5
        [HttpGet("{id}")]
        public VehicleDto? Get(Guid id)
        {
            return vehiclesService.GetById(id).ToDto();
        }

        // POST api/<VehiclesController>
        [HttpPost]
        public void Post([FromBody] VehicleDto dto)
        {
            vehiclesService.Create(dto.ToEntity());
        }

        // PUT api/<VehiclesController>/5
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody] VehicleDto dto)
        {
            vehiclesService.Update(id, dto.ToEntity());
        }

        // DELETE api/<VehiclesController>/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            vehiclesService.Delete(id);
        }
    }
}
