using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Services;

namespace UrlShortener.Functions
{
    public class HistoryHttpTrigger
    {
        public static async Task<HttpResponseMessage> Run(HttpRequestMessage req)
        {
            var service = new ShortenerService();

            var data = service.GetLastXDaysUrlHistory();
            if (data == null)
            {
                return req.CreateResponse(HttpStatusCode.OK);
            }

            string json = JsonConvert.SerializeObject(data,
                                                      new JsonSerializerSettings
                                                      {
                                                          ContractResolver = new CamelCasePropertyNamesContractResolver()
                                                      });

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }
    }
}