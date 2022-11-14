using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_core.Backend.Utils.Interfaces
{
    public class TokenUtilHelper : ITokenUtilHelper
    {
        public JwtSecurityToken GetTokenDataByStringValue(string rawToken)
        {
            var token = rawToken;
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            return jwtSecurityToken;
        }
    }
}
