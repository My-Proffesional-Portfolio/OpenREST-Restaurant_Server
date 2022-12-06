using Microsoft.AspNetCore.Mvc;

namespace OpenRestRestaurant_api.Controllers
{
    public class BaseApiController : ControllerBase
    {

        protected string GetBearerTokenFromHeader()
        {
            var tokenHeader = HttpContext.Request.Headers["Authorization"];
            var bearerToken = tokenHeader.FirstOrDefault();
            var token = bearerToken?.Split("Bearer ")[1];

            return token;
        }
    }
}
