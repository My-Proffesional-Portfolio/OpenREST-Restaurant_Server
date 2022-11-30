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
using OpenRestRestaurant_core.Infrastructure.Services;
using OpenRestRestaurant_models.Responses.Account;

namespace OpenRestRestaurant_core.Backend.Services
{
    public class RestaurantStaffService : IRestaurantStaffService
    {

        private readonly IRestaurantCompanyService _restaurantSC;
        private readonly IUserService _userService;
        private readonly TransactionManager _tManager;
        private readonly IRestaurantStaffRepository _staffRepository;
        private readonly OpenRestRestaurantDbContext _dbContext;

        public RestaurantStaffService(IRestaurantCompanyService restaurantSC, IUserService userService,
            TransactionManager tManager, IRestaurantStaffRepository staffRepository, OpenRestRestaurantDbContext dbContext)
        {
            _restaurantSC = restaurantSC;
            _userService = userService;
            _tManager = tManager;
            _staffRepository = staffRepository;
            _dbContext = dbContext;
        }
        public async Task<NewStaffEmployeeResponseModel> AddUserToRestaurantCompany(NewStaffUserModel newUserStaff, string token)
        {

            var employeeType = _restaurantSC.GetEmployeeTypeFromToken(token);

            if (employeeType != 0)
            {
                throw new Exception("This employee cannot add new user");
            }

            var restaurantID = _restaurantSC.GetRestaurantIdFromToken(token);
            var newUser = await _userService.SaveUser(newUserStaff, saveOnCall: false);


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
                RestaurantCompanyId = restaurantID
            };

            Action transactionRestaurant = async () =>
            {
                await _staffRepository.AddAsync(newRestaurantStaff);
            };

            await _tManager.RunTransaction(transactionRestaurant);
            await _dbContext.SaveChangesAsync();

            return new NewStaffEmployeeResponseModel
            {
                RestaurantStaffId = newRestaurantStaff.Id,
                UserID = newUser.Id
            };
        }
    }
}
