using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UrlShortener.Services;

namespace UrlShortener.Functions
{
    public class CreateHttpTrigger
    {
        public static async Task<HttpResponseMessage> Run(HttpRequestMessage req)
        {
            // Get request body
            dynamic data = await req.Content.ReadAsAsync<object>();

            string longUrl = data?.url;
            if (string.IsNullOrWhiteSpace(longUrl))
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }
            
            var service = new ShortenerService();
            var result = await service.CreateShortUrlAsync(longUrl.Trim());

            return result.Item1 == null
                ? req.CreateErrorResponse(HttpStatusCode.BadRequest, result.Item2)
                : req.CreateResponse(HttpStatusCode.Created, result.Item1);
        }
    }
}