using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace UrlShortner.Models
{
    public class ShortUrlLogEntity : TableEntity
    {
        public ShortUrlLogEntity(string shortCode, string longUrl, string userAgent, string clientIp)
        {
            this.PartitionKey = shortCode;
            this.RowKey = Guid.NewGuid().ToString();
            this.LongUrl = LongUrl;
            this.UserAgent = userAgent;
            this.ClientIp = clientIp ?? string.Empty;
        }

        public ShortUrlLogEntity()
        {

        }

        public string LongUrl { get; set; }

        /// <summary>
        /// Requester user agent
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Requester IP address
        /// </summary>
        public string ClientIp { get; set; }
    }
}
