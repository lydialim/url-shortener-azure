using Microsoft.Azure.WebJobs.Host;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UrlShortener.Services;

namespace UrlShortener.Functions
{
    public class CreateHttpTrigger
    {
        public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // Get request body
            dynamic data = await req.Content.ReadAsAsync<object>();

            string longUrl = data?.url;
            if (string.IsNullOrEmpty(longUrl))
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            string connectionString = ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString;
            var service = new ShortenerService(connectionString);
            var result = await service.CreateShortUrlAsync(longUrl);

            return result.Item1 == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, result.Item2)
                : req.CreateResponse(HttpStatusCode.Created, result.Item1);
        }
    }
}