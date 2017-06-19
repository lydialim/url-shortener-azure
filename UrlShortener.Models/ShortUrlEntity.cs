
using Microsoft.WindowsAzure.Storage.Table;

namespace UrlShortner.Models
{
    public class ShortUrlEntity : TableEntity
    {
        public ShortUrlEntity(string shortCode, string longUrl)
        {
            this.PartitionKey = shortCode;
            this.RowKey = string.Empty;
            this.LongUrl = longUrl;
        }

        public ShortUrlEntity()
        {

        }

        /// <summary>
        /// The long url
        /// </summary>
        public string LongUrl { get; set; }
    }
}
