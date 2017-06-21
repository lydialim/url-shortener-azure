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

            // analytics
            string clientIp = GetClientIpAddress(req);
            await service.LogView(shortCode, longUrl, req.Headers.UserAgent.ToString(), clientIp);

            var response = req.CreateResponse(HttpStatusCode.Redirect);
            response.Headers.Add("Location", longUrl);
            return response;
        }

        private static string GetClientIpAddress(HttpRequestMessage req)
        {
            string clientIp = null;
            if (req.Headers.Contains("X-Forwarded-For"))
            {
                clientIp = req.Headers.GetValues("X-Forwarded-For").First();
            }

            if (string.IsNullOrEmpty(clientIp) && req.Headers.Contains("REMOTE_ADDR"))
            {
                clientIp = req.Headers.GetValues("REMOTE_ADDR").First();
            }

            if (string.IsNullOrEmpty(clientIp))
            {
                return string.Empty;
            }

            return clientIp.Split(new char[] { ':' })[0];
        }
    }
}