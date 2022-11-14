﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenRestRestaurant_core.Backend.Services;
using OpenRestRestaurant_models;
using OpenRestRestaurant_models.Requests.CompanyRestaurant;
using OpenRestRestaurant_models.Requests.Staff;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OpenRestRestaurant_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        // GET: api/<AccountController>
        private readonly RestaurantCompanySerivce _restaurantSC;
        private readonly RestaurantStaffService _staffSC;
        private readonly AuthURLValue _authURLValue;
        private readonly AccountService _accountSC;
        public AccountController(RestaurantCompanySerivce restaurantSC, AuthURLValue authURL, 
            RestaurantStaffService staffSC, AccountService accountSC)
        {
            _restaurantSC = restaurantSC;
            _authURLValue = authURL;
            _staffSC = staffSC;
            _accountSC = accountSC;
        }

        [Route("login")]
        [HttpGet()]
        public async Task<string> login(string userName, string password)
        {
            var loginResult = await _accountSC.Login(userName, password);
            return loginResult;
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
