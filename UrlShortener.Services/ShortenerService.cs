using System;
using System.Threading.Tasks;

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

        public ShortenerService(string connectionString) : this(
            new ShortenUrlRepository(connectionString),
            new ShortenUrlLogRepository(connectionString))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlToShorten"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="shortCode"></param>
        /// <returns></returns>
        public async Task<String> GetLongUrlAsync(string shortCode)
        {
            return await Task.FromResult("GetLongUrlAsync");
        }
    }
}