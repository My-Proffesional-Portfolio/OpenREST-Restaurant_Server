using OpenRestRestaurant_core.Backend.Utils;
using OpenRestRestaurant_core.Backend.Utils.Interfaces;
using OpenRestRestaurant_core.Infrastructure.Services;
using OpenRestRestaurant_data.DataAccess;
using OpenRestRestaurant_infrastructure.Repositories.Interfaces;
using OpenRestRestaurant_models;
using OpenRestRestaurant_models.DTOs.Auth;
using OpenRestRestaurant_models.Exceptions;
using OpenRestRestaurant_models.Requests.CompanyRestaurant;
using OpenRestRestaurant_models.Responses.CompanyRestaurants;

namespace OpenRestRestaurant_core.Backend.Services
{
    public class RestaurantCompanyService : IRestaurantCompanyService
    {
        private readonly IRestaurantCompanyRepository _restaurantCompanyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRestaurantStaffRepository _staffRepository;
        private readonly TransactionManager _tManager;
        private readonly OpenRestRestaurantDbContext _dbContext;
        private readonly IApiCallerUtil _apiCallerUtil;
        private readonly AuthURLValue _authURLValue;
        private readonly ITokenUtilHelper _tokenHelper;

        public RestaurantCompanyService(IRestaurantCompanyRepository restaurantCompanyRepo,
            IUserRepository userRepository, IRestaurantStaffRepository staffRepository,
            TransactionManager tmanager, OpenRestRestaurantDbContext dbContext,
            IApiCallerUtil apiCallerUtil, AuthURLValue authURL, ITokenUtilHelper tokenHelper)
        {
            _restaurantCompanyRepository = restaurantCompanyRepo;
            _userRepository = userRepository;
            _staffRepository = staffRepository;
            _tManager = tmanager;
            _dbContext = dbContext;
            _apiCallerUtil = apiCallerUtil;
            _authURLValue = authURL;
            _tokenHelper = tokenHelper;
        }

        public async Task<NewCompanyRestaurantResponseModel> AddRestaurantCompany(NewCompanyRestaurantModel newRestaurant)
        {

            var userInDB = _userRepository.FindByExpresion(w => w.UserName == newRestaurant.UserName).FirstOrDefault();

            if (userInDB != null)
                throw new UserIssueException("User already exist");


            var restaurantCompany = new RestaurantCompany()
            {
                Id = Guid.NewGuid(),
                FiscalAddress = newRestaurant.FiscalAddress,
                IsActive = true,
                CompanyName = newRestaurant.CompanyName,
                FiscalId = newRestaurant.FiscalId,
                LegalOwner = newRestaurant.LegalOwner,
                CreationDate = DateTime.Now,
            };

            var passwordAndSalt = await _apiCallerUtil.CallApiAsync<EncryptResponseDTO>
               (_authURLValue.UrlValue, "encryptPassword?password=" + newRestaurant.Password, RestSharp.Method.Get, null, null);


            var user = new User()
            {
                Id = Guid.NewGuid(),
                UserName = newRestaurant.UserName,
                IsActive = true,
                Email = newRestaurant.Email,
                HashedPassword = passwordAndSalt.PasswordHash,
                Salt = Guid.Parse(passwordAndSalt.SaltValue),
                CreationDate = DateTime.Now,

            };

            var staff = new RestaurantStaff()
            {
                Id = Guid.NewGuid(),
                IsActive = true,
                Name = newRestaurant.Name,
                LastName = newRestaurant.LastName,
                SurName = newRestaurant.SurName,
                PersonalEmail = newRestaurant.PersonalEmail,
                PersonalPhone = newRestaurant.PersonalPhone,
                Address = "xxxxx",
                FiscalId = "xxxxx",
                EmployeeNumber = 1000,
                Ssn = "AAAAAAAAAAA",
                RestaurantCompanyId = restaurantCompany.Id,
                UserId = user.Id,
                CreationDate = DateTime.Now,
                EmployeeType = 0,
            };

            Action transactionRestaurant = async () =>
            {
                await _restaurantCompanyRepository.AddAsync(restaurantCompany);
                await _userRepository.AddAsync(user);
                await _staffRepository.AddAsync(staff);

            };

            await _tManager.RunTransaction(transactionRestaurant);
            await _dbContext.SaveChangesAsync();

            return new NewCompanyRestaurantResponseModel
            {
                RestaurantID = restaurantCompany.Id,
                RestaurantNumber = restaurantCompany.RestaurantNumber,
                CompanyName = restaurantCompany.CompanyName
            };
        }

        public Guid GetRestaurantIdFromToken(string bearerToken)
        {
            var tokenDecoded = _tokenHelper.GetTokenDataByStringValue(bearerToken);

            var restaurantID = tokenDecoded.Claims.Where(W => W.Type == "restaurantID").FirstOrDefault().Value;
            return Guid.Parse(restaurantID.ToUpper());
        }

        public int GetEmployeeTypeFromToken(string bearerToken)
        {
            var tokenDecoded = _tokenHelper.GetTokenDataByStringValue(bearerToken);

            var restaurantID = tokenDecoded.Claims.Where(W => W.Type == "employeeType").FirstOrDefault().Value;
            return int.Parse(restaurantID);
        }
    }
}
