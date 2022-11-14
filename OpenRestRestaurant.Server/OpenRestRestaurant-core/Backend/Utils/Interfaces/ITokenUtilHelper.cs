using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_core.Backend.Utils.Interfaces
{
    public interface ITokenUtilHelper
    {
        JwtSecurityToken GetTokenDataByStringValue(string rawToken);
    }
}
