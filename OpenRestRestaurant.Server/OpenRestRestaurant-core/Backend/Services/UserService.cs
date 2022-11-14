using OpenRestRestaurant_data.DataAccess;
using OpenRestRestaurant_models.DTOs.Auth;
using OpenRestRestaurant_models;
using OpenRestRestaurant_models.Requests.Staff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenRestRestaurant_core.Backend.Utils.Interfaces;
using OpenRestRestaurant_infrastructure.Repositories.Interfaces;

namespace OpenRestRestaurant_core.Backend.Services
{
    public class UserService
    {
        private readonly IApiCallerUtil _apiCallerUtil;
        private readonly AuthURLValue _authURLValue;
        private readonly IUserRepository _userRepository;
        private readonly OpenRestRestaurantDbContext _dbContext;
        public UserService(IApiCallerUtil apiCallerUtil, AuthURLValue authURLValue, IUserRepository userRepository, OpenRestRestaurantDbContext userContext)
        {
            _apiCallerUtil = apiCallerUtil;
            _authURLValue = authURLValue;
            _userRepository = userRepository;
            _dbContext = userContext;
        }

        public async Task<User> SaveUser(NewStaffUserModel userModel, bool saveOnCall = false)
        {
            var passwordAndSalt = await _apiCallerUtil.CallApiAsync<EncryptResponseDTO>
             (_authURLValue.UrlValue, "encryptPassword?password=" + userModel.Password, RestSharp.Method.Get, null, null);

            var newUser = new User()
            {
                IsActive = true,
                Id = Guid.NewGuid(),
                CreationDate = DateTime.Now,
                Email = "email.address@gmail.com",
                UserName = userModel.UserName,
                HashedPassword = passwordAndSalt.PasswordHash,
                Salt = Guid.Parse(passwordAndSalt.SaltValue),
            };

            await _userRepository.AddAsync(newUser);

            if (saveOnCall)
            {
                await _dbContext.SaveChangesAsync();
            }

            return newUser;
        }
    }
}
