using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenRestRestaurant_api.Filters;
using OpenRestRestaurant_core.Backend.Services;
using OpenRestRestaurant_core.Infrastructure.Services;
using OpenRestRestaurant_models;
using OpenRestRestaurant_models.Requests.CompanyRestaurant;
using OpenRestRestaurant_models.Requests.Staff;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OpenRestRestaurant_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AutomaticExceptionHandler]
    public class AccountController : ControllerBase
    {
        // GET: api/<AccountController>
        private readonly IRestaurantCompanyService _restaurantSC;
        private readonly IRestaurantStaffService _staffSC;
        private readonly AuthURLValue _authURLValue;
        private readonly IAccountService _accountSC;
        public AccountController(IRestaurantCompanyService restaurantSC, AuthURLValue authURL, 
            IRestaurantStaffService staffSC, IAccountService accountSC)
        {
            _restaurantSC = restaurantSC;
            _authURLValue = authURL;
            _staffSC = staffSC;
            _accountSC = accountSC;
        }

        [Route("login")]
        [HttpGet()]
        public async Task<IActionResult> login(string userName, string password)
        {
            var loginResult = await _accountSC.Login(userName, password);
            return Ok(loginResult);
        }
  
        // POST api/<AccountController>
        [HttpPost]
        [Route("signup")]
        
        public async Task<IActionResult> Post([FromBody] NewCompanyRestaurantModel restaurant)
        {
            var result = await _restaurantSC.AddRestaurantCompany(restaurant);
            return Ok(result);
        }

        [HttpPost]
        [Route("newUser")]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] NewStaffUserModel user)
        {
            var tokenHeader = HttpContext.Request.Headers["Authorization"];
            var bearerToken = tokenHeader.FirstOrDefault();
            var token = bearerToken?.Split("Bearer ")[1];

            var result = await _staffSC.AddUserToRestaurantCompany(user, token);
            return Ok(result);
        }
    }
}
