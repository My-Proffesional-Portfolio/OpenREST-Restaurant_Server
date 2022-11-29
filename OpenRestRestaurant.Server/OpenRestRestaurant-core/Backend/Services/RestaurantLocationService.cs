using OpenRestRestaurant_core.Backend.Utils;
using OpenRestRestaurant_core.Infrastructure.Services;
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
    public class RestaurantLocationService : IRestaurantLocationService
    {

        private readonly OpenRestRestaurantDbContext _dbContext;
        private readonly IRestaurantCompanyService _restaurantSC;
        private readonly IUserRepository _userRepository;
        private readonly IRestaurantLocationRepository _locationRepository;
        private readonly IRestaurantTableRepository _tablesRepository;
        private readonly TransactionManager _tmanager;

        public RestaurantLocationService(OpenRestRestaurantDbContext dbContext, IRestaurantCompanyService restaurantSC,
            IUserRepository userRepository, IRestaurantLocationRepository locationRepository, TransactionManager transactionManager,
            IRestaurantTableRepository tablesRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _restaurantSC = restaurantSC;
            _locationRepository = locationRepository;
            _tmanager = transactionManager;
            _tablesRepository = tablesRepository;
        }

        public async Task<object> AddNewLocationToRestaurant(NewRestaurantLocationModel newLocation, string token)
        {
            List<RestaurantTable> restaurantTablesDB = new List<RestaurantTable>();
            var employeeType = _restaurantSC.GetEmployeeTypeFromToken(token);

            if (employeeType != 0)
                throw new Exception("User cannot add location, need escalation");

            var restaurantID = _restaurantSC.GetRestaurantIdFromToken(token);

            var desiredManagerUser = await _userRepository.GetByIdAsync(newLocation.ManagerUserID);

            if (desiredManagerUser == null)
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

            if (newLocation.Tables != null && newLocation.Tables.Count > 0)
            {
                foreach ( var table in newLocation.Tables )
                {
                    var newTable = new RestaurantTable();
                    newTable.Id = Guid.NewGuid();
                    newTable.CreationDate = DateTime.Now;
                    newTable.TableNumber =  table.TableNumber;
                    newTable.RestaurantLocationId = newRestaurantLocation.Id;
                    newTable.IsActive = true;
                    newTable.TableCapacity = table.TableCapacity;
                    newTable.UicoordLocation = "";
                    restaurantTablesDB.Add(newTable);
                }
                
            }

            await _tmanager.RunTransaction(async () =>
            {
                await _locationRepository.AddAsync(newRestaurantLocation);
                await _tablesRepository.AddRangeAsync(restaurantTablesDB);
            });

            _dbContext.SaveChanges();

            return new
            {
                locationID = newRestaurantLocation.Id,
                locationName = newRestaurantLocation.LocationName
            };

        }
    }
}
