using System.Threading.Tasks;
using UrlShortner.Models;

namespace UrlShortener.Services
{
    public interface IShortenUrlLogRepository
    {
        Task<string> SaveAsync(ShortUrlLogEntity logEntity);
    }
}