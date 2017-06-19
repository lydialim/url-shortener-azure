
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortner.Models;

namespace UrlShortener.Services
{
    internal class ShortenUrlRepository : IShortenUrlRepository
    {
        private const string TableName = "shortenUrl";

        private readonly CloudStorageAccount _storageAccount;
        private readonly CloudTableClient _tableClient;

        public ShortenUrlRepository(string connectionString)
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

        /// <summary>
        /// Save the short url mapping
        /// </summary>
        /// <param name="shortCode">Shorten url code</param>
        /// <param name="longUrl">Long Url</param>
        /// <returns>Etag of the execution result</returns>
        public async Task<string> SaveAsync(string shortCode, string longUrl)
        {
            var newEntity = new ShortUrlEntity(shortCode, longUrl);

            // Create the TableOperation object that inserts the entity.
            TableOperation insertOperation = TableOperation.Insert(newEntity);

            var table = _tableClient.GetTableReference(TableName);

            // Execute the insert operation.
            var result = await table.ExecuteAsync(insertOperation);
            return result.Etag;
        }

        /// <summary>
        /// Finds the actual long url by the shortcode key
        /// </summary>
        /// <param name="shortCode">The shorten url code</param>
        /// <returns><see cref="ShortUrlEntity"/></returns>
        public async Task<ShortUrlEntity> FindLongUrlAsync(string shortCode)
        {
            // Create a retrieve operation that takes a entity.
            var retrieveOperation = TableOperation.Retrieve<ShortUrlEntity>(shortCode, string.Empty);

            var table = _tableClient.GetTableReference(TableName);

            // Execute the retrieve operation.
            var retrievedResult = await table.ExecuteAsync(retrieveOperation);
            if (retrievedResult == null)
            {
                return null;
            }

            var entity = retrievedResult.Result as ShortUrlEntity;
            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="longUrl"></param>
        /// <returns></returns>
        public string FindShortCode(string longUrl)
        {
            // Define the query, and select only the RowKey property.
            var projectionQuery = new TableQuery<DynamicTableEntity>()
                                                .Select(new string[] { "PartitionKey" })
                                                .Where(TableQuery.GenerateFilterCondition("LongUrl", QueryComparisons.Equal, longUrl));

            // Define an entity resolver to work with the entity after retrieval.
            EntityResolver<string> resolver = (pk, rk, ts, props, etag) => pk;

            var table = _tableClient.GetTableReference(TableName);

            string shortCode = table.ExecuteQuery(projectionQuery, resolver)?.FirstOrDefault();
            return shortCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lastXDays"></param>
        /// <returns></returns>
        public IEnumerable<ShortUrlEntity> GetAllShortCodes(int lastXDays = 7)
        {
            // Construct the query operation for all short codes in the last X days
            var query = new TableQuery<ShortUrlEntity>()
                                        .Where(TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.LessThanOrEqual, DateTime.UtcNow.AddDays(lastXDays)));

            var table = _tableClient.GetTableReference(TableName);
            return table.ExecuteQuery(query);
        }
    }
}
