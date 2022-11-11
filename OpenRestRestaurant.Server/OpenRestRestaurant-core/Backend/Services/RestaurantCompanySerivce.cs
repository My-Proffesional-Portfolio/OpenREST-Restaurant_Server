﻿using OpenRestRestaurant_core.Backend.Utils;
using OpenRestRestaurant_core.Backend.Utils.Interfaces;
using OpenRestRestaurant_data.DataAccess;
using OpenRestRestaurant_infrastructure.Repositories.Interfaces;
using OpenRestRestaurant_models;
using OpenRestRestaurant_models.DTOs.Auth;
using OpenRestRestaurant_models.Requests.CompanyRestaurant;

namespace OpenRestRestaurant_core.Backend.Services
{
    public class RestaurantCompanySerivce
    {
        private readonly IRestaurantCompanyRepository _restaurantCompanyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRestaurantStaffRepository _staffRepository;
        private readonly TransactionManager _tManager;
        private readonly OpenRestRestaurantDbContext _dbContext;
        private readonly IApiCallerUtil _apiCallerUtil;
        private readonly AuthURLValue _authURLValue;

        public RestaurantCompanySerivce(IRestaurantCompanyRepository restaurantCompanyRepo,
            IUserRepository userRepository, IRestaurantStaffRepository staffRepository,
            TransactionManager tmanager, OpenRestRestaurantDbContext dbContext,
            IApiCallerUtil apiCallerUtil, AuthURLValue authURL)
        {
            _restaurantCompanyRepository = restaurantCompanyRepo;
            _userRepository = userRepository;
            _staffRepository = staffRepository;
            _tManager = tmanager;
            _dbContext = dbContext;
            _apiCallerUtil = apiCallerUtil;
            _authURLValue = authURL;

        }

        public async Task<object> AddRestaurantCompany(NewCompanyRestaurantModel newRestaurant)
        {

            var restaurantCompany = new RestaurantCompany()
            {
                Id = Guid.NewGuid(),
                FiscalAddress = newRestaurant.FiscalAddress,
                IsActive = true,
                CompanyName = newRestaurant.CompanyName,
                FiscalId = newRestaurant.FiscalId,
                LegalOwner = newRestaurant.LegalOwner,
                CreationDate = DateTime.Now,
            };

            var passwordAndSalt = await _apiCallerUtil.CallApiAsync<EncryptResponseDTO>
               (_authURLValue.UrlValue, "encryptPassword?password=" + newRestaurant.Password, RestSharp.Method.Get, null, null);


            var user = new User()
            {
                Id = Guid.NewGuid(),
                UserName = newRestaurant.UserName,
                IsActive = true,
                Email = newRestaurant.Email,
                HashedPassword = passwordAndSalt.PasswordHash,
                Salt = Guid.Parse(passwordAndSalt.SaltValue),
                CreationDate = DateTime.Now,

            };

            var staff = new RestaurantStaff()
            {
                Id = Guid.NewGuid(),
                IsActive = true,
                Name = newRestaurant.Name,
                LastName = newRestaurant.LastName,
                SurName = newRestaurant.SurName,
                PersonalEmail = newRestaurant.PersonalEmail,
                PersonalPhone = newRestaurant.PersonalPhone,
                Address = "xxxxx",
                FiscalId = "xxxxx",
                EmployeeNumber = 1000,
                Ssn = "AAAAAAAAAAA",
                RestaurantCompanyId = restaurantCompany.Id,
                UserId = user.Id,
                CreationDate = DateTime.Now
            };

            Action transactionRestaurant = async () =>
            {
                await _restaurantCompanyRepository.AddAsync(restaurantCompany);
                await _userRepository.AddAsync(user);
                await _staffRepository.AddAsync(staff);

            };

            await _tManager.RunTransaction(transactionRestaurant);
            await _dbContext.SaveChangesAsync();

            return new { restaurantID = restaurantCompany.Id, restaurantCompany.RestaurantNumber, restaurantCompany.CompanyName };
        }
    }
}