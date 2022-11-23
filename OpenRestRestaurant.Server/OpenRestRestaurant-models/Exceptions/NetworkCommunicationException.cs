using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_models.Exceptions
{
    public class NetworkCommunicationException : Exception
    {
        public NetworkCommunicationException(string message) : base (message)
        {

        }
    }
}
