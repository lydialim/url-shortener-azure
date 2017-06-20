using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UrlShortener.Services;

namespace UrlShortener.Functions
{
    public class RedirectHttpTrigger
    {
        public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, string shortCode)
        {
            var service = new ShortenerService();

            string longUrl = await service.GetLongUrlAsync(shortCode);
            if (string.IsNullOrEmpty(longUrl))
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            string clientIp = null;
            if (req.Headers.Contains("X-Forwarded-For"))
            {
                clientIp = req.Headers.GetValues("X-Forwarded-For").FirstOrDefault‌​();
            }

            // analytics
            await service.LogView(shortCode, req.Headers.UserAgent.ToString(), clientIp);

            var response = req.CreateResponse(HttpStatusCode.Redirect);
            response.Headers.Add("Location", longUrl);
            return response;
        }
    }
}