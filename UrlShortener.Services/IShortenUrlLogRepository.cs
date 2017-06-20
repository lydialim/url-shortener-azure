using System.Collections.Generic;
using System.Threading.Tasks;
using UrlShortner.Models;

namespace UrlShortener.Services
{
    public interface IShortenUrlLogRepository
    {
        IEnumerable<ShortUrlLogEntity> GetVisitsLastXDays(int lastXDays = 3);

        Task<string> SaveAsync(ShortUrlLogEntity logEntity);
    }
}