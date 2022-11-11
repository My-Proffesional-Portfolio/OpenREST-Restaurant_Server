using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_models.DTOs.Auth
{
    public class EncryptResponseDTO
    {
        public string PasswordHash { get; set; }
        public string SaltValue { get; set; }
    }
}
