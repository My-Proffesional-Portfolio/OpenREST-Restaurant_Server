using Microsoft.AspNetCore.Mvc;
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
    public class TestControllers
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
        private IRestaurantTableRepository _tableRepo;
        private NewCompanyRestaurantModel _integrationRestaurant;
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
            _tableRepo = new RestaurantTableRepository(_context);

            var authURL = new AuthURLValue() { UrlValue = _urlAuthApiValue };
            var apiCaller = new ApiCallerUtil();

            _restaurantSC = new RestaurantCompanyService(_restaurantCompanyRepo, _userRepo,
                _staffRepo, new TransactionManager(_context), _context, apiCaller, authURL, new TokenUtilHelper());

            _accountSC = new AccountService(_userRepo, authURL, apiCaller, _staffRepo);

            var integrationUUID = Guid.NewGuid();

            _userNameDesired = "[integration-test.openrestaurant]user-AccountController" + integrationUUID;
            _passwordDesired = "[integration-test.openrestaurant]password-AccountController" + integrationUUID;

            _integrationRestaurant = new NewCompanyRestaurantModel()
            {
                CompanyName = "[integration-test.openrestaurant-AccountController]" + integrationUUID + "Restaurant[|*--TEST--*|]",
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

            _accountController = new AccountController(_restaurantSC, _authURLValue, _staffSC, _accountSC);

        }

        [TestMethod]
        public void TestingControllers()
        {

            var newIntegratedRestaurant = _accountController.Post(_integrationRestaurant).GetAwaiter().GetResult();

            var okObjNewRestaurant = newIntegratedRestaurant as OkObjectResult;
            var valueResponseNewRestaurant = okObjNewRestaurant.Value;

            var tokenResult = _accountController.login(_userNameDesired, _passwordDesired).GetAwaiter().GetResult();

            var okObjLogin = tokenResult as OkObjectResult;
            var valueResponseLogin = okObjLogin.Value;

            Assert.IsTrue(okObjNewRestaurant.StatusCode == 200);
            Assert.IsNotNull(valueResponseNewRestaurant);

            Assert.IsTrue(okObjLogin.StatusCode == 200);
            Assert.IsNotNull(valueResponseLogin);



        }

    }
}
