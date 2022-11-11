using Microsoft.AspNetCore.Mvc;
using OpenRestRestaurant_core.Backend.Services;
using OpenRestRestaurant_models;
using OpenRestRestaurant_models.Requests.CompanyRestaurant;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OpenRestRestaurant_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        // GET: api/<AccountController>
        private readonly RestaurantCompanySerivce _restaurantSC;
        private readonly AuthURLValue _authURLValue;
        public AccountController(RestaurantCompanySerivce restaurantSC, AuthURLValue authURL)
        {
            _restaurantSC = restaurantSC;
            _authURLValue = authURL;
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AccountController>
        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> Post([FromBody] NewCompanyRestaurantModel restaurant)
        {
            var result = await _restaurantSC.AddRestaurantCompany(restaurant);
            return Ok(result);
        }

        // PUT api/<AccountController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
