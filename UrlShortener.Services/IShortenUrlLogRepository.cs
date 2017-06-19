using System.Threading.Tasks;

namespace UrlShortener.Services
{
    public interface IShortenUrlLogRepository
    {
        Task<string> SaveAsync(string shortCode, string userAgent, string clientIp);
    }
}