using OpenRestRestaurant_data.DataAccess;
using OpenRestRestaurant_models.DTOs.Auth;
using OpenRestRestaurant_models;
using OpenRestRestaurant_models.Requests.Staff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using OpenRestRestaurant_core.Backend.Utils;
using OpenRestRestaurant_infrastructure.Repositories.Interfaces;

namespace OpenRestRestaurant_core.Backend.Services
{
    public class RestaurantStaffService
    {

        private readonly RestaurantCompanySerivce _restaurantSC;
        private readonly UserService _userService;
        private readonly TransactionManager _tManager;
        private readonly IRestaurantStaffRepository _staffRepository;
        private readonly OpenRestRestaurantDbContext _dbContext;

        public RestaurantStaffService(RestaurantCompanySerivce restaurantSC, UserService userService,
            TransactionManager tManager, IRestaurantStaffRepository staffRepository, OpenRestRestaurantDbContext dbContext)
        {
            _restaurantSC = restaurantSC;
            _userService = userService;
            _tManager = tManager;
            _staffRepository = staffRepository;
            _dbContext = dbContext;
        }
        public async Task<object> AddUserToRestaurantCompany (NewStaffUserModel newUserStaff, string token)
        {

            var employeeType = _restaurantSC.GetEmployeeTypeFromToken(token);

            if (employeeType != 0)
            {
                throw new Exception("This employee cannot add new user");
            }

            var restaurantID = _restaurantSC.GetRestaurantIdFromToken(token);
            var newUser =await _userService.SaveUser(newUserStaff, saveOnCall: false);


            var newRestaurantStaff = new RestaurantStaff()
            {
                Id = Guid.NewGuid(),
                IsActive = true,
                CreationDate = DateTime.Now,
                Address = newUserStaff.Address,
                EmployeeNumber = 2000,
                FiscalId = newUserStaff.FiscalId,
                Name = newUserStaff.Name,
                LastName = newUserStaff.LastName,
                SurName = newUserStaff.SurName,
                PersonalEmail = newUserStaff.PersonalEmail,
                PersonalPhone = newUserStaff.PersonalPhone,
                Ssn = newUserStaff.Ssn,
                UserId = newUser.Id,
                EmployeeType = newUserStaff.NewEmployeeType,
                RestaurantLocationId = newUserStaff.LocationId,
                RestaurantCompanyId =  restaurantID
            };

            Action transactionRestaurant = async () =>
            {
                await _staffRepository.AddAsync(newRestaurantStaff);
            };

            await _tManager.RunTransaction(transactionRestaurant);
            await _dbContext.SaveChangesAsync();

            return new { restaurantStaffId = newRestaurantStaff.Id, userID = newUser.Id};
        }
    }
}
