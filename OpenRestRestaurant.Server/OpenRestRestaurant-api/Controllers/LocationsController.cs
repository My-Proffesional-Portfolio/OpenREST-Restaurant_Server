using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenRestRestaurant_core.Infrastructure.Services;
using OpenRestRestaurant_models.Requests.RestaurantLocation;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OpenRestRestaurant_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LocationsController : ControllerBase
    {
        // GET: api/<LocationsController>
        private readonly IRestaurantLocationService _locationSC;
        public LocationsController(IRestaurantLocationService locationSC)
        {
            _locationSC =  locationSC;
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<LocationsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<LocationsController>
        [HttpPost]
        
        public async Task<IActionResult> Post([FromBody] NewRestaurantLocationModel location)
        {
            var tokenHeader = HttpContext.Request.Headers["Authorization"];
            var bearerToken = tokenHeader.FirstOrDefault();
            var token = bearerToken?.Split("Bearer ")[1];
            var newLocation = await _locationSC.AddNewLocationToRestaurant(location, token);
            return Ok(newLocation);
        }

        // PUT api/<LocationsController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<LocationsController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
