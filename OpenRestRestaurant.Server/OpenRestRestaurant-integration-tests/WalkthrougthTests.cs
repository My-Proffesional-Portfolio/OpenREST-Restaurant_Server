using OpenRestRestaurant_api.Controllers;
using OpenRestRestaurant_core.Infrastructure.Services;
using OpenRestRestaurant_data.DataAccess;
using OpenRestRestaurant_infrastructure.Repositories.Interfaces;
using OpenRestRestaurant_models.Requests.CompanyRestaurant;
using OpenRestRestaurant_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenRestRestaurant_core.Backend.Services;
using OpenRestRestaurant_core.Backend.Utils.Interfaces;
using OpenRestRestaurant_core.Backend.Utils;
using OpenRestRestaurant_infrastructure.Repositories;
using OpenRestRestaurant_integration_tests.Models;
using Microsoft.AspNetCore.Http;
using OpenRestRestaurant_models.Responses.Account;
using OpenRestRestaurant_models.Requests.Staff;
using OpenRestRestaurant_models.Catalogs;
using OpenRestRestaurant_models.Requests.RestaurantLocation;
using OpenRestRestaurant_models.Requests.RestaurantTable;

namespace OpenRestRestaurant_integration_tests
{

    [TestClass]
    public class WalkthrougthTests
    {
        private IRestaurantCompanyService _restaurantSC;
        private IRestaurantStaffService _staffSC;
        private AuthURLValue _authURLValue;
        private IAccountService _accountSC;
        private TransactionManager _tmanager;
        private IUserService _userSC;
        private IRestaurantLocationService _locationSC;
        private OpenRestRestaurantDbContext _context;
        private string _urlAuthApiValue;
        private IUserRepository _userRepo;
        private IRestaurantCompanyRepository _restaurantCompanyRepo;
        private IRestaurantStaffRepository _staffRepo;
        private IRestaurantLocationRepository _locationRepo;
        private IRestaurantTableRepository _tableRepo;
        private NewCompanyRestaurantModel _integrationRestaurant;
        private List<NewCompanyRestaurantModel> _listNewRestaurants;
        private string _userNameDesired;
        private string _passwordDesired;
        private AccountController _accountController;
        private LocationsController _locationController;

        [TestInitialize]
        public void Setup()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));

            var root = builder.Build();
            var sampleConnectionString = root.GetConnectionString("DefaultConnection");

            _urlAuthApiValue = root.GetSection("security")["authURL"];

            //https://www.thecodebuzz.com/dbcontext-mock-and-unit-test-entity-framework-net-core/
            var options = new DbContextOptionsBuilder<OpenRestRestaurantDbContext>()
                .UseSqlServer(sampleConnectionString).Options;

            _context = new OpenRestRestaurantDbContext(options);
            _userRepo = new UserRepository(_context);
            _restaurantCompanyRepo = new RestaurantCompanyRepository(_context);
            _staffRepo = new RestaurantStaffRepository(_context);
            _locationRepo = new RestaurantLocationRepository(_context);
            _tableRepo = new RestaurantTableRepository(_context);

            var authURL = new AuthURLValue() { UrlValue = _urlAuthApiValue };
            var apiCaller = new ApiCallerUtil();

            _tmanager = new TransactionManager(_context);

            _restaurantSC = new RestaurantCompanyService(_restaurantCompanyRepo, _userRepo,
                _staffRepo, _tmanager, _context, apiCaller, authURL, new TokenUtilHelper());

            _userSC = new UserService(apiCaller, authURL, _userRepo, _context);

            _staffSC = new RestaurantStaffService(_restaurantSC, _userSC, _tmanager, _staffRepo, _context);
            _accountSC = new AccountService(_userRepo, authURL, apiCaller, _staffRepo);
            _locationSC = new RestaurantLocationService(_context, _restaurantSC, _staffRepo, _locationRepo, _tmanager, _tableRepo);

            var integrationUUID = Guid.NewGuid();
            _listNewRestaurants = new List<NewCompanyRestaurantModel>();

            Root apiResponse = apiCaller.CallApiAsync<Root>("https://randomuser.me/", "api/?results=5", RestSharp.Method.Get, null, null).GetAwaiter().GetResult();


            if (apiResponse is null)
            {
                _userNameDesired = "[integration-test.openrestaurant]user-AccountController" + integrationUUID;
                _passwordDesired = "[integration-test.openrestaurant]password-AccountController" + integrationUUID;

                _integrationRestaurant = new NewCompanyRestaurantModel()
                {
                    CompanyName = "[integration-test.openrestaurant-AccountController]" + integrationUUID,
                    Name = "[integration-test.openrestaurant-AccountController]Name",
                    FiscalAddress = "[integration-test.openrestaurant-AccountController]FiscalAddress",
                    Email = "[integration-test.openrestaurant-AccountController]Email",
                    FiscalId = "[integration-test.openrestaurant-AccountController]FiscalId",
                    LastName = "[integration-test.openrestaurant-AccountController]LastName",
                    LegalOwner = "[integration-test.openrestaurant-AccountController]LegalOwner",
                    Ssn = "[integration-test.openrestaurant-AccountController]Ssn",
                    SurName = "[integration-test.openrestaurant-AccountController]Surname",
                    Password = _passwordDesired,
                    UserName = _userNameDesired,
                    PersonalEmail = "[integration-test.openrestaurant-AccountController]PersonalEmail",
                    PersonalPhone = "[integration-test.openrestaurant-AccountController]PersonalPhone"
                };

                _listNewRestaurants.Add(_integrationRestaurant);
            }

            else
            {
                foreach (var item in apiResponse.results)
                {
                    var fakeUser = item;
                    _userNameDesired = fakeUser.login.username;
                    _passwordDesired = "defaultPassword";

                    _integrationRestaurant = new NewCompanyRestaurantModel()
                    {
                        CompanyName = fakeUser.name.first + " Restaurant[|*--TEST--*|]",
                        Name = fakeUser.name.first,
                        FiscalAddress = fakeUser.location.street.name + "# " + fakeUser.location.street.number
                        + " - " + fakeUser.location.city + ", " + fakeUser.location.state,
                        Email = fakeUser.email,
                        FiscalId = Guid.NewGuid().ToString(),
                        LastName = fakeUser.name.last,
                        LegalOwner = fakeUser.name.title + ". " + fakeUser.name.first + " " + fakeUser.name.last,
                        Ssn = "123456789",
                        SurName = "Skywalker",
                        Password = _passwordDesired,
                        UserName = _userNameDesired,
                        PersonalEmail = fakeUser.email,
                        PersonalPhone = fakeUser.phone,
                    };

                    _listNewRestaurants.Add(_integrationRestaurant);
                }

            }
            _accountController = new AccountController(_restaurantSC, _authURLValue, _staffSC, _accountSC);
            _locationController = new LocationsController(_locationSC);

        }

        [TestMethod]
        public void TestingControllers()
        {
            foreach (var item in _listNewRestaurants)
            {
                try
                {
                    CreateRestaurantCompany(item);
                    SaveNewUserToken(item);
                    var staffInfo = CreateStaffEmployee(item);
                    CreateLocationsWithTables(staffInfo.RestaurantStaffId);
                   
                }
                catch (Exception ex) { }
            }
        }

        private void CreateRestaurantCompany(NewCompanyRestaurantModel item)
        {
            var newIntegratedRestaurant = _accountController.Post(item).GetAwaiter().GetResult();

            var okObjNewRestaurant = newIntegratedRestaurant as OkObjectResult;
            var valueResponseNewRestaurant = okObjNewRestaurant.Value;

            Assert.IsTrue(okObjNewRestaurant.StatusCode == 200);
            Assert.IsNotNull(valueResponseNewRestaurant);
        }

        private void SaveNewUserToken (NewCompanyRestaurantModel item)
        {
            var tokenResult = _accountController.login(item.UserName, item.Password).GetAwaiter().GetResult();

            var httpContext = new DefaultHttpContext();
            var okObjLogin = tokenResult as OkObjectResult;
            var valueResponseLogin = (LoginResponseModel)okObjLogin.Value;

            httpContext.Request.Headers["Authorization"] = "Bearer " + valueResponseLogin.token;

            _accountController.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            _locationController.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

            Assert.IsTrue(okObjLogin.StatusCode == 200);
            Assert.IsNotNull(valueResponseLogin);
        }

        private NewStaffEmployeeResponseModel CreateStaffEmployee(NewCompanyRestaurantModel item)
        {
            var newStaffUser = new NewStaffUserModel()
            {
                Name = "ITesting[Name]",
                Address = "ITesting[Address]",
                FiscalId = "ITesting[FiscalId]",
                LastName = "ITesting[LastName]",
                NewEmployeeType = EmployeeType.LocationManager,
                UserName = item.UserName + Guid.NewGuid(),
                Password = _passwordDesired,
                PersonalEmail = "ITmail",
                PersonalPhone = "000000000123",
                SurName = "Simpson",
                Ssn = "fakeSSN123",
                UserEmail = "fakemail@me.com"
            };

            var newUserStaff = _accountController.Post(newStaffUser).GetAwaiter().GetResult();
            var okObjNewStaff = newUserStaff as OkObjectResult;
            var valueResponseNewStaff = (NewStaffEmployeeResponseModel)okObjNewStaff.Value;

            return valueResponseNewStaff;
        }

        private void CreateLocationsWithTables(Guid staffID)
        {
            var newLocationWithTables = new NewRestaurantLocationModel()
            {
                FiscalID = "ITesting[FiscalId]",
                LocationAddress = "ITesting[LocationAddress]",
                LocationAlias = "ITTesting location restaurant",
                LocationEmail = "ITTestingmail@mail.com",
                LocationPhone = "ITTestng 214234456",
                ManagerStaffUserID = staffID,
                Tables = new List<LocationTableModel>()
                        {
                            new LocationTableModel()
                            {
                                TableCapacity = 6,
                                TableNumber = "1A"
                            },
                            new LocationTableModel()
                            {
                                TableCapacity = 4,
                                TableNumber = "2A"
                            },
                            new LocationTableModel()
                            {
                                TableCapacity = 6,
                                TableNumber = "3A"
                            }
                        }
            };

            var locationTables = _locationController.Post(newLocationWithTables).GetAwaiter().GetResult();

            var locationTablesObjOk = locationTables as OkObjectResult;
            //var valueLocationTables =  ()

        }
    }
}
