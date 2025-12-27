using car_marketplace_api_3tier.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace car_marketplace_api_3tier
{
    [ApiController]
    [Route("[controller]")]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        // GET /cars (public endpoint)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Car>>> Get()
        {
            var cars = await _carService.GetCarsAsync();
            return Ok(cars);
        }

        // GET /cars/1 (public endpoint)
        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> Get(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return Ok(car);
        }

        // POST /cars (Requires Owner role)
        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<ActionResult<Car>> Post([FromBody] Car newCar)
        {
            var addedCar = await _carService.AddNewCarAsync(newCar);
            return CreatedAtAction(nameof(Get), new { id = addedCar.Id }, addedCar);
        }

        // PUT /cars/1 (Requires Owner role)
        [HttpPut("{id}")]
        [Authorize(Roles = "Owner")]
        public async Task<ActionResult> Put(int id, [FromBody] Car updatedCar)
        {
            updatedCar.Id = id;
            var result = await _carService.UpdateExistingCarAsync(updatedCar);
            if (result == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE /cars/1 (Requires Owner role)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Owner")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _carService.DeleteCarByIdAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}