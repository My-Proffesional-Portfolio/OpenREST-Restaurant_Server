using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_models.DTOs.Auth
{
    public class BearerAuthResponseDTO
    {
        public string tokenAudience { get; set; }
        public string privateKey { get; set; }
        public string tokenIssuer { get; set; }
    }
}
