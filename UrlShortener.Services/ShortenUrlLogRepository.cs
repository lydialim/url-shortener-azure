using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;
using UrlShortner.Models;

namespace UrlShortener.Services
{
    internal class ShortenUrlLogRepository : IShortenUrlLogRepository
    {
        private const string TableName = "shortenUrlLog";

        private readonly CloudStorageAccount _storageAccount;
        private readonly CloudTableClient _tableClient;

        public ShortenUrlLogRepository(string connectionString)
        {
            // Parse the connection string and return a reference to the storage account.
            _storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create the table client.
            _tableClient = _storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            var table = _tableClient.GetTableReference(TableName);

            // Create the table if it doesn't exist.
            table.CreateIfNotExists();
        }

        public async Task<string> SaveAsync(string shortCode, string userAgent, string clientIp)
        {
            var newEntity = new ShortUrlLogEntity(shortCode, userAgent, clientIp);

            // Create the TableOperation object that inserts the entity.
            TableOperation insertOperation = TableOperation.Insert(newEntity);

            var table = _tableClient.GetTableReference(TableName);

            // Execute the insert operation.
            var result = await table.ExecuteAsync(insertOperation);
            return result.Etag;
        }
    }
}
