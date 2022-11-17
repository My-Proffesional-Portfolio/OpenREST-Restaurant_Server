using OpenRestRestaurant_data.DataAccess;
using OpenRestRestaurant_infrastructure.Repositories.Interfaces;
using OpenRestRestaurant_models.Requests.RestaurantLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_core.Backend.Services
{
    public class RestaurantLocationService
    {

        private readonly OpenRestRestaurantDbContext _dbContext;
        private readonly RestaurantCompanySerivce _restaurantSC;
        private readonly IUserRepository _userRepository;
        private readonly IRestaurantLocationRepository _locationRepository;

        public RestaurantLocationService(OpenRestRestaurantDbContext dbContext, RestaurantCompanySerivce restaurantSC,
            IUserRepository userRepository, IRestaurantLocationRepository locationRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _restaurantSC = restaurantSC;
            _locationRepository = locationRepository;
        }

        public async Task<object> AddNewLocationToRestaurant(NewRestaurantLocationModel newLocation, string token)
        {

            var employeeType = _restaurantSC.GetEmployeeTypeFromToken(token);

            if (employeeType != 0)
                throw new Exception("User cannot add location, need escalation");

            var restaurantID = _restaurantSC.GetRestaurantIdFromToken(token);

            var desiredManagerUser = await _userRepository.GetByIdAsync(newLocation.ManagerUserID);

            if (desiredManagerUser != null)
            {
                throw new Exception("Provided user Id for manager not found");
            }

            var newRestaurantLocation = new RestaurantLocation()
            {
                Id = Guid.NewGuid(),
                IsActive = true,
                LocationAddress = newLocation.LocationAddress,
                CreationDate = DateTime.Now,
                FiscalId = newLocation.FiscalID,
                LocationEmail = newLocation.LocationEmail,
                LocationPhone = newLocation.LocationPhone,
                ManagerId = desiredManagerUser.Id,
                LocationName = newLocation.LocationAlias,
                RestaurantCompanyId = restaurantID,
            };

            await _locationRepository.AddAsync(newRestaurantLocation);
            _dbContext.SaveChanges();

            return new
            {
                locationID = newRestaurantLocation.Id,
                locationName = newRestaurantLocation.LocationName
            };

        }
    }
}
