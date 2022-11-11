using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_core.Backend.Utils.Interfaces
{
    public interface IApiCallerUtil
    {
        Task<T> CallApiAsync<T>(string baseUrl, string clientRequest, Method method = Method.Get, object body = null, List<KeyValuePair<string, string>> headers = null);
    }
}
