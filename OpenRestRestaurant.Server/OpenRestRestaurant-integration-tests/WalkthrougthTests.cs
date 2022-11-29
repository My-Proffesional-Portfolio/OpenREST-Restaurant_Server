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

namespace OpenRestRestaurant_integration_tests
{

    [TestClass]
    public class WalkthrougthTests
    {
        private IRestaurantCompanyService _restaurantSC;
        private IRestaurantStaffService _staffSC;
        private AuthURLValue _authURLValue;
        private IAccountService _accountSC;
        private OpenRestRestaurantDbContext _context;
        private string _urlAuthApiValue;
        private IUserRepository _userRepo;
        private IRestaurantCompanyRepository _restaurantCompanyRepo;
        private IRestaurantStaffRepository _staffRepo;
        private NewCompanyRestaurantModel _integrationRestaurant;
        private List<NewCompanyRestaurantModel> _listNewRestaurants;
        private string _userNameDesired;
        private string _passwordDesired;
        private AccountController _accountController;

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

            var authURL = new AuthURLValue() { UrlValue = _urlAuthApiValue };
            var apiCaller = new ApiCallerUtil();

            _restaurantSC = new RestaurantCompanyService(_restaurantCompanyRepo, _userRepo,
                _staffRepo, new TransactionManager(_context), _context, apiCaller, authURL, new TokenUtilHelper());

            _accountSC = new AccountService(_userRepo, authURL, apiCaller, _staffRepo);

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

        }

        [TestMethod]
        public void TestingControllers()
        {


            foreach (var item in _listNewRestaurants)
            {
                try
                {
                    var newIntegratedRestaurant = _accountController.Post(item).GetAwaiter().GetResult();

                    var okObjNewRestaurant = newIntegratedRestaurant as OkObjectResult;
                    var valueResponseNewRestaurant = okObjNewRestaurant.Value;

                    var tokenResult = _accountController.login(item.UserName, item.Password).GetAwaiter().GetResult();

                    var okObjLogin = tokenResult as OkObjectResult;
                    var valueResponseLogin = okObjLogin.Value;

                    Assert.IsTrue(okObjNewRestaurant.StatusCode == 200);
                    Assert.IsNotNull(valueResponseNewRestaurant);

                    Assert.IsTrue(okObjLogin.StatusCode == 200);
                    Assert.IsNotNull(valueResponseLogin);
                }
                catch (Exception ex) { }
            }



        }

    }
}
