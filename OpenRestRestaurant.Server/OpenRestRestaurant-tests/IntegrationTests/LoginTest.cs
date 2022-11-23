using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenRestRestaurant_api.Controllers;
using OpenRestRestaurant_core.Backend.Services;
using OpenRestRestaurant_core.Backend.Utils;
using OpenRestRestaurant_core.Backend.Utils.Interfaces;
using OpenRestRestaurant_core.Infrastructure.Services;
using OpenRestRestaurant_data.DataAccess;
using OpenRestRestaurant_infrastructure.Repositories;
using OpenRestRestaurant_infrastructure.Repositories.Interfaces;
using OpenRestRestaurant_models;
using OpenRestRestaurant_models.Requests.CompanyRestaurant;

namespace OpenRestRestaurant_tests.IntegrationTests
{
    [TestClass]
    public class LoginTest
    {
        private OpenRestRestaurantDbContext _context;
        private string _urlAuthApiValue;
        private IUserRepository _userRepo;
        private IRestaurantCompanyRepository _restaurantCompanyRepo;
        private IRestaurantStaffRepository _staffRepo;
        private string _userNameDesired;
        private string _passwordDesired;
        private IAccountService _accountSC;
        private IRestaurantCompanyService _restaurantCompanyService;
        private IRestaurantStaffService _restaurantStaffService;
        private NewCompanyRestaurantModel _integrationRestaurant;

        [TestInitialize]
        public void SetUp()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));

            var root = builder.Build();
            var sampleConnectionString = root.GetConnectionString("DefaultConnection");

            _urlAuthApiValue = root.GetSection("security")["authURL"];


            //https://www.thecodebuzz.com/dbcontext-mock-and-unit-test-entity-framework-net-core/
            var options = new DbContextOptionsBuilder<OpenRestRestaurantDbContext>()
                .UseSqlServer(sampleConnectionString).Options;

            var authURL = new AuthURLValue() { UrlValue = _urlAuthApiValue };
            var apiCaller = new ApiCallerUtil();

            _context = new OpenRestRestaurantDbContext(options);
            _userRepo = new UserRepository(_context);
            _restaurantCompanyRepo = new RestaurantCompanyRepository(_context);
            _staffRepo = new RestaurantStaffRepository(_context);


            _userNameDesired = "[integration-test.openrestaurant]user" + Guid.NewGuid();
            _passwordDesired = "[integration-test.openrestaurant]password" + Guid.NewGuid();


            _integrationRestaurant = new NewCompanyRestaurantModel()
            {
                CompanyName = "[integration-test.openrestaurant]CompanyName",
                Name = "[integration-test.openrestaurant]Name",
                FiscalAddress = "[integration-test.openrestaurant]FiscalAddress",
                Email = "[integration-test.openrestaurant]Email",
                FiscalId = "[integration-test.openrestaurant]FiscalId",
                LastName = "[integration-test.openrestaurant]LastName",
                LegalOwner = "[integration-test.openrestaurant]LegalOwner",
                Ssn = "[integration-test.openrestaurant]Ssn",
                SurName = "[integration-test.openrestaurant]Surname",
                Password = _passwordDesired,
                UserName = _userNameDesired,
                PersonalEmail = "[integration-test.openrestaurant]PersonalEmail",
                PersonalPhone = "[integration-test.openrestaurant]PersonalPhone"
            };


            _restaurantCompanyService = new RestaurantCompanyService(_restaurantCompanyRepo, _userRepo,
                _staffRepo, new TransactionManager(_context), _context, apiCaller, authURL, new TokenUtilHelper());

            _accountSC = new AccountService(_userRepo, authURL, apiCaller);
        }

        [TestMethod]
        public void CreateAndLogin()
        {
            
            var newIntegrationRestaurant = _restaurantCompanyService.AddRestaurantCompany(_integrationRestaurant).GetAwaiter().GetResult();
            var token = _accountSC.Login(_userNameDesired, _passwordDesired).GetAwaiter().GetResult();

            Assert.IsTrue(!string.IsNullOrWhiteSpace(token));
        }
    }
}