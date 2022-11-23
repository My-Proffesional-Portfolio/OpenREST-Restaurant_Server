using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_models.Exceptions
{
    public class UserIssueException : Exception
    {
        public UserIssueException(string message) : base(message)
        {

        }
    }
}
