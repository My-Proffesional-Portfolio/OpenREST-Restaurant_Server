using Microsoft.AspNetCore.Mvc;
using Moq;
using OpenRestRestaurant_api.Controllers;
using OpenRestRestaurant_core.Infrastructure.Services;
using OpenRestRestaurant_models;
using OpenRestRestaurant_models.Requests.CompanyRestaurant;
using OpenRestRestaurant_models.Responses.CompanyRestaurants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_tests.OpenRestRestaurant_api
{

    [TestClass]
    public class AccountControllerTests
    {
        private Mock<IRestaurantCompanyService> _restaurantMockSC;
        private Mock<IRestaurantStaffService> _staffMockSC;
        private Mock<AuthURLValue> _authURLMockValue;
        private Mock<IAccountService> _accountMockSC;
        private AccountController _accountController;

        [TestInitialize]
        public void Setup()
        {
            _restaurantMockSC = new Mock<IRestaurantCompanyService>();
            _staffMockSC = new Mock<IRestaurantStaffService>();
            _authURLMockValue = new Mock<AuthURLValue>();
            _accountMockSC = new Mock<IAccountService>();

            _authURLMockValue.Setup(s => s.UrlValue).Returns("http://mocktestauthapi.com/auth/api");

            _accountController = new AccountController(_restaurantMockSC.Object, _authURLMockValue.Object,
                _staffMockSC.Object, _accountMockSC.Object);

        }

        [TestMethod]
        public void PostNewCompanyRestaurantTest()
        {
            _restaurantMockSC.Setup(s => s.AddRestaurantCompany(It.IsAny<NewCompanyRestaurantModel>()))
                .ReturnsAsync(new NewCompanyRestaurantResponseModel()
                {
                    RestaurantID = Guid.NewGuid(),
                    CompanyName = "RestaurantMock",
                    RestaurantNumber = 1000
                });

            var result = _accountController.Post(new NewCompanyRestaurantModel()).GetAwaiter().GetResult();

            var objResult = result as OkObjectResult;

            Assert.AreEqual(objResult.StatusCode, 200);
        }

        [TestMethod]
        public void LoginTest()
        {
            _accountMockSC.Setup(s => s.Login(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("eymdba_mockedToken");

            var result = _accountController.login("mockedUser", "mockedPassword").GetAwaiter().GetResult();

            var objResult = result as OkObjectResult;

            Assert.AreEqual(objResult.StatusCode, 200);
        }
    }
}
