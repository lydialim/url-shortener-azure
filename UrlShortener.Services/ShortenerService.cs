using System;
using System.Threading.Tasks;
using UrlShortner.Models;

namespace UrlShortener.Services
{
    public class ShortenerService
    {
        private readonly IShortenUrlRepository _shortenUrlRepository;
        private readonly IShortenUrlLogRepository _logRepository;

        public ShortenerService(
            IShortenUrlRepository shortenUrlRepository, 
            IShortenUrlLogRepository shortenUrlLogRepository)
        {
            _shortenUrlRepository = shortenUrlRepository;
            _logRepository = shortenUrlLogRepository;
        }
        
        // should be fullfill by DI
        public ShortenerService() : this(
            new ShortenUrlRepository(),
            new ShortenUrlLogRepository())
        {
        }

        /// <summary>
        /// Creates a new short code if <paramref name="urlToShorten"/> is brand new. 
        /// Otherwise it would return the existing short code
        /// </summary>
        /// <param name="urlToShorten">Long url to be shorten</param>
        /// <returns>
        /// Item1 - shortCode
        /// Item2 - error message if any
        /// </returns>
        public async Task<Tuple<string, string>> CreateShortUrlAsync(string urlToShorten)
        {
            // check if this is a valid url 
            Uri uriResult;
            if (!Uri.TryCreate(urlToShorten, UriKind.Absolute, out uriResult))
            {
                return new Tuple<string, string>(null, "Invalid Uri");
            }

            // accepts http|s only
            if (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps)
            {
                return new Tuple<string, string>(null, "Invalid Scheme");
            }

            string shortCode = _shortenUrlRepository.FindShortCode(urlToShorten);
            if (!string.IsNullOrEmpty(shortCode))
            {
                return new Tuple<string, string>(shortCode, null);
            }

            shortCode = StringGenerator.Generate();
            await _shortenUrlRepository.SaveAsync(shortCode, urlToShorten);

            return new Tuple<string, string>(shortCode, null);
        }

        /// <summary>
        /// Retrieve the long url based on <paramref name="shortCode"/>
        /// </summary>
        /// <param name="shortCode">Short Code</param>
        /// <returns>The long url if a match is found, otherwise NULL</returns>
        public async Task<String> GetLongUrlAsync(string shortCode)
        {
            return await _shortenUrlRepository.GetLongUrlAsync(shortCode);
        }

        /// <summary>
        /// Logs a visit for the shortcode for analytics purpose
        /// </summary>
        /// <param name="shortCode">Short Code</param>
        /// <param name="userAgent">The requester's User Agent</param>
        /// <param name="clientIp">The client IP address</param>
        /// <returns></returns>
        public async Task LogView(string shortCode, string userAgent, string clientIp)
        {
            var newEntity = new ShortUrlLogEntity(shortCode, userAgent, clientIp);

            await _logRepository.SaveAsync(newEntity);
        }
    }
}