using System.Collections.Generic;
using System.Threading.Tasks;
using UrlShortner.Models;

namespace UrlShortener.Services
{
    public interface IShortenUrlRepository
    {
        Task<ShortUrlEntity> FindLongUrlAsync(string shortCode);

        Task<string> SaveAsync(string shortCode, string longUrl);

        string FindShortCode(string longUrl);

        IEnumerable<ShortUrlEntity> GetAllShortCodes(int lastXDays = 7);
    }
}