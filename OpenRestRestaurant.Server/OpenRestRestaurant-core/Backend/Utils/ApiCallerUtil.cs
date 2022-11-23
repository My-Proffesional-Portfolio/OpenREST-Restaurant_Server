using OpenRestRestaurant_core.Backend.Utils.Interfaces;
using OpenRestRestaurant_models.Exceptions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRestRestaurant_core.Backend.Utils
{
    public class ApiCallerUtil : IApiCallerUtil
    {
        public async Task<T> CallApiAsync<T>(string baseUrl, string clientRequest, Method method = Method.Get, object body = null, List<KeyValuePair<string, string>> headers = null)
        {

            try
            {
                var client = new RestClient(baseUrl);
                var request = new RestRequest(clientRequest, method);

                if (body != null)
                    request.AddBody(body);

                if (headers != null || headers?.Count > 0)
                {
                    request.AddHeader("Content-Type", "application/json");
                    foreach (var h in headers)
                    {
                        request.AddHeader(h.Key, h.Value);
                    }
                }

                var result = await client.ExecuteAsync<T>(request);
                return result.Data;
            }
            catch (Exception ex)
            {
                throw new NetworkCommunicationException("An error has ocurred while attempting to receive or make external API request: " + ex.Message);
            }


        }
    }
}
