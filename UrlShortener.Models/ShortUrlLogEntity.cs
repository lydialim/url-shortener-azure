using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace UrlShortner.Models
{
    public class ShortUrlLogEntity : TableEntity
    {
        public ShortUrlLogEntity(string shortCode, string userAgent, string clientIp)
        {
            this.PartitionKey = shortCode;
            this.RowKey = Guid.NewGuid().ToString();
            this.UserAgent = userAgent;
            this.ClientIp = clientIp ?? string.Empty;
        }

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
