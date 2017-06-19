using Microsoft.WindowsAzure.Storage.Table;

namespace UrlShortner.Models
{
    public class ShortUrlLogEntity : TableEntity
    {
        public ShortUrlLogEntity(string shortCode, string userAgent, string clientIp)
        {
            this.RowKey = shortCode;
            this.UserAgent = userAgent;
            this.ClientIp = clientIp;
        }

        /// <summary>
        /// 
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClientIp { get; set; }
    }
}
