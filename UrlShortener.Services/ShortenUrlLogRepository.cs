using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using UrlShortner.Models;

namespace UrlShortener.Services
{
    internal class ShortenUrlLogRepository : IShortenUrlLogRepository
    {
        private const string ConnectionStringKey = "StorageConnectionString";
        private const string TableName = "shortenUrlLog";

        private readonly CloudStorageAccount _storageAccount;
        private readonly CloudTableClient _tableClient;

        public ShortenUrlLogRepository()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[ConnectionStringKey]?.ConnectionString;

            // if connection string is null, attempt to use default value
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = CloudConfigurationManager.GetSetting(ConnectionStringKey);
            }

            // Parse the connection string and return a reference to the storage account.
            _storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create the table client.
            _tableClient = _storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            var table = _tableClient.GetTableReference(TableName);

            // Create the table if it doesn't exist.
            table.CreateIfNotExists();
        }

        public IEnumerable<ShortUrlLogEntity> GetVisitsLastXDays(int lastXDays = 3)
        {
            var query = new TableQuery<ShortUrlLogEntity>()
                                        .Where(TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.LessThanOrEqual, DateTime.UtcNow.AddDays(lastXDays)));

            var table = _tableClient.GetTableReference(TableName);
            return table.ExecuteQuery(query);
        }

        /// <summary>
        /// Save the <paramref name="logEntity"/> into log table
        /// </summary>
        /// <param name="logEntity"></param>
        /// <returns></returns>
        public async Task<string> SaveAsync(ShortUrlLogEntity logEntity)
        {
            // Create the TableOperation object that inserts the entity.
            TableOperation insertOperation = TableOperation.Insert(logEntity);

            var table = _tableClient.GetTableReference(TableName);

            // Execute the insert operation.
            var result = await table.ExecuteAsync(insertOperation);
            return result.Etag;
        }
    }
}
