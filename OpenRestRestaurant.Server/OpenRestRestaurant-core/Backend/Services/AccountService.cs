using Microsoft.EntityFrameworkCore;
using OpenRestRestaurant_core.Backend.Utils.Interfaces;
using OpenRestRestaurant_core.Infrastructure.Services;
using OpenRestRestaurant_infrastructure.Repositories;
using OpenRestRestaurant_infrastructure.Repositories.Interfaces;
using OpenRestRestaurant_models;
using OpenRestRestaurant_models.Catalogs;
using OpenRestRestaurant_models.DTOs.Auth;
using OpenRestRestaurant_models.Exceptions;
using OpenRestRestaurant_models.Responses.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_core.Backend.Services
{
    public class AccountService : IAccountService
    {
        public readonly IUserRepository _userRepository;
        public readonly IRestaurantStaffRepository _restaurantStaffRepository;
        private readonly AuthURLValue _authURLValue;
        private readonly IApiCallerUtil _apiCallerUtil;

        public AccountService(IUserRepository userRepository, AuthURLValue authURLValue, IApiCallerUtil apiCallerUtil, IRestaurantStaffRepository restaurantStaffRepository)
        {
            _userRepository = userRepository;
            _authURLValue = authURLValue;
            _apiCallerUtil = apiCallerUtil;
            _restaurantStaffRepository = restaurantStaffRepository;
        }

        public async Task<LoginResponseModel> Login(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
                throw new MissingRequestParamsException("Username or password not privided");

            var user = _userRepository.FindByExpresion(w => w.UserName == userName).Include(i => i.RestaurantStaffs)
                .FirstOrDefault();

            if (user == null)
            {
                throw new UserIssueException("User not found in database");
            }

            var decryptPassword = await _apiCallerUtil.CallApiAsync<DecryptedPasswordResponseDTO>
               (_authURLValue.UrlValue, "decryptPassword?hashedPassword=" + user.HashedPassword + "&salt=" + user.Salt, RestSharp.Method.Get, null, null);

            if (decryptPassword.isError is not false)
            {
                throw new AuthorizationException("An error occured trying to decrypt password " + decryptPassword.technicalMessage);
            }

            if (decryptPassword.PlainPassword != password)
                throw new AuthorizationException("Provided password does not match with user");

            var tokenData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("userID", user.Id.ToString()),
                new KeyValuePair<string, string>("restaurantID", user.RestaurantStaffs.FirstOrDefault().RestaurantCompanyId.ToString()),
                new KeyValuePair<string, string>("employeeType", user.RestaurantStaffs.FirstOrDefault().EmployeeType.ToString()),
                new KeyValuePair<string, string>("clientKey", "AB6B-B4C8BFEAD9B2"),
            };
            var authToken = await _apiCallerUtil.CallApiAsync<TokenResponseDTO>
             (_authURLValue.UrlValue, "generateToken", RestSharp.Method.Post, tokenData, null);

            if (authToken == null || string.IsNullOrWhiteSpace(authToken.token))
                throw new NetworkCommunicationException("An error has ocurred when trying to retrieve user token");

            var staffPersonal = _restaurantStaffRepository.FindByExpresion(w => w.UserId == user.Id).FirstOrDefault();

            return new LoginResponseModel
            {
                token = authToken.token,
                userName = user.UserName,
                staffPersonal = staffPersonal.Name + " " + staffPersonal.LastName,
                restaurantID = staffPersonal.RestaurantCompanyId,
                employeeType = staffPersonal.EmployeeType,
                userID = user.Id,
            };
        }

    }
}
