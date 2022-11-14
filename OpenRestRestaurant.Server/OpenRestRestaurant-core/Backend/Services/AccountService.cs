﻿using Microsoft.EntityFrameworkCore;
using OpenRestRestaurant_core.Backend.Utils.Interfaces;
using OpenRestRestaurant_infrastructure.Repositories;
using OpenRestRestaurant_infrastructure.Repositories.Interfaces;
using OpenRestRestaurant_models;
using OpenRestRestaurant_models.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_core.Backend.Services
{
    public class AccountService
    {
        public readonly IUserRepository _userRepository;
        private readonly AuthURLValue _authURLValue;
        private readonly IApiCallerUtil _apiCallerUtil;

        public AccountService(IUserRepository userRepository, AuthURLValue authURLValue, IApiCallerUtil apiCallerUtil)
        {
            _userRepository = userRepository;
            _authURLValue = authURLValue;
            _apiCallerUtil = apiCallerUtil;
        }

        public async Task<string> Login(string userName, string password)
        {
            var user = _userRepository.FindByExpresion(w => w.UserName == userName).Include(i => i.RestaurantStaffs)
                .FirstOrDefault();

            if (user == null)
            {
                throw new Exception("User not found in database");
            }

            var decryptPassword = await _apiCallerUtil.CallApiAsync<DecryptedPasswordResponseDTO>
               (_authURLValue.UrlValue, "decryptPassword?hashedPassword=" + user.HashedPassword + "&salt=" + user.Salt, RestSharp.Method.Get, null, null);

            if (decryptPassword.isError is not false)
            {
                throw new Exception("An error occured trying to decrypt password " + decryptPassword.technicalMessage);
            }

            if (decryptPassword.PlainPassword != password)
                throw new Exception("Provided password does not match with user");

            var tokenData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("userID", user.Id.ToString()),
                new KeyValuePair<string, string>("restaurantID", user.RestaurantStaffs.FirstOrDefault().RestaurantCompanyId.ToString()),
                new KeyValuePair<string, string>("employeeType", user.RestaurantStaffs.FirstOrDefault().EmployeeType.ToString()),
                new KeyValuePair<string, string>("clientKey", "AB6B-B4C8BFEAD9B2"),
            };
            var authToken = await _apiCallerUtil.CallApiAsync<TokenResponseDTO>
             (_authURLValue.UrlValue, "generateToken", RestSharp.Method.Post, tokenData, null);

            return authToken.token;
        }

       


    }
}
