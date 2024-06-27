using Azure;
using Azure.Data.Tables;
using Microsoft.Extensions.Logging;

namespace Dewiride.Azure.Storage.Table.Helper
{
    public class TableStorageHelper : ITableStorageHelper
    {
        private readonly ILogger<TableStorageHelper> _logger;
        private readonly TableServiceClient _tableServiceClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableStorageHelper"/> class.
        /// </summary>
        /// <param name="logger">The logger to use for logging information.</param>
        /// <param name="tableServiceClient">The table service client for interacting with Azure Table Storage.</param>
        public TableStorageHelper(ILogger<TableStorageHelper> logger, TableServiceClient tableServiceClient)
        {
            _logger = logger;
            _tableServiceClient = tableServiceClient;
        }

        /// <summary>
        /// Get Table Client Async and create table if not exists
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private async Task<TableClient> GetTableClientAsync(string tableName)
        {
            var tableClient = _tableServiceClient.GetTableClient(tableName);
            await tableClient.CreateIfNotExistsAsync();
            return tableClient;
        }

        /// <summary>
        /// Get a single entity from Azure Table Storage
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="partitionKey"></param>
        /// <param name="rowKey"></param>
        /// <returns></returns>
        public async Task<T?> GetEntityAsync<T>(string tableName, string partitionKey, string rowKey) where T : class, ITableEntity, new()
        {
            try
            {
                var tableClient = await GetTableClientAsync(tableName);
                return await tableClient.GetEntityAsync<T>(partitionKey: partitionKey, rowKey: rowKey);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR: Table Storage Helper Get Entity by Partition Key and Row Key --> {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Asynchronously retrieves an entity from Azure Table Storage by email.
        /// </summary>
        /// <typeparam name="T">The type of the entity to retrieve, must implement ITableEntity.</typeparam>
        /// <param name="tableName">The name of the table to query.</param>
        /// <param name="partitionKey">The partition key of the entity.</param>
        /// <param name="email">The email address to query for.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the retrieved entity or null if not found.</returns>
        /// <exception cref="Exception">Thrown when the table name is null or empty.</exception>
        public async Task<T?> GetEntityByEmailAsync<T>(string tableName, string partitionKey, string email) where T : class, ITableEntity, new()
        {
            try
            {
                if (string.IsNullOrEmpty(tableName))
                    throw new Exception("Table name cannot be null or empty");

                var tableClient = await GetTableClientAsync(tableName);

                if (!string.IsNullOrEmpty(partitionKey) && !string.IsNullOrEmpty(email))
                    return tableClient.Query<T>(filter: TableClient.CreateQueryFilter($"Email eq {email.ToLower()} and PartitionKey eq {partitionKey}")).FirstOrDefault();
                else
                    return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR: Table Storage Helper Get Entity by Email --> {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets all records from Azure Table Storage for the table name passed along with tenantId as the partition key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="partitionKey"></param>
        /// <returns></returns>
        public async Task<List<T>> GetEntitiesAsync<T>(string? tableName, string? partitionKey) where T : class, ITableEntity, new()
        {
            try
            {
                if (string.IsNullOrEmpty(tableName))
                    throw new Exception("Table name cannot be null or empty");

                var tableClient = await GetTableClientAsync(tableName);

                if (!string.IsNullOrEmpty(partitionKey))
                    return tableClient.Query<T>(filter: TableClient.CreateQueryFilter($"PartitionKey eq {partitionKey}")).ToList();
                else
                    return tableClient.Query<T>().ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR: Table Storage Helper Get Entities by Partition Key --> {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets the latest record from Azure Table Storage for the table name passed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<T?> GetEntityAsync<T>(string? tableName) where T : class, ITableEntity, new()
        {
            try
            {
                if (string.IsNullOrEmpty(tableName))
                    throw new NullReferenceException("Table name cannot be null or empty");

                var tableClient = await GetTableClientAsync(tableName);

                // return first item from ascending order on timestamp
                return tableClient.Query<T>().OrderBy(x => x.Timestamp).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR: Table Storage Helper Get Entity --> {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Gets all records from Azure Table Storage for the table name passed along with the filter passed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<T>> GetEntitiesAsync<T>(string? tableName, FormattableString filter) where T : class, ITableEntity, new()
        {
            try
            {
                if (string.IsNullOrEmpty(tableName))
                    throw new Exception("Table name cannot be null or empty");

                var tableClient = await GetTableClientAsync(tableName);
                var allData = tableClient.Query<T>(filter: TableClient.CreateQueryFilter(filter)).ToList();

                return allData;
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR: Table Storage Helper Get Entities by Filter --> {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Insert or Merge an entity into Azure Table Storage. If the entity exists, it will be replaced, else a new entity will be created.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<Response?> UpsertEntityAsync<T>(string? tableName, T entity) where T : class, ITableEntity, new()
        {
            try
            {
                if (string.IsNullOrEmpty(tableName))
                    throw new Exception("Table name cannot be null or empty");

                var tableClient = await GetTableClientAsync(tableName);
                return await tableClient.UpsertEntityAsync(entity: entity, mode: TableUpdateMode.Replace);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR: Table Storage Helper Upsert Entity --> {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Delete a single entity from Azure Table Storage by tenantId as the partition key and aadObjectId as the row key
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="partitionKey"></param>
        /// <param name="rowKey"></param>
        /// <returns></returns>
        public async Task<Response> DeleteEntityAsync(string? tableName, string partitionKey, string rowKey)
        {
            try
            {
                if (string.IsNullOrEmpty(tableName))
                    throw new Exception("Table name cannot be null or empty");

                var tableClient = await GetTableClientAsync(tableName);
                return await tableClient.DeleteEntityAsync(partitionKey: partitionKey, rowKey: rowKey);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ERROR: Table Storage Helper Delete Entity --> {ex.Message}");
                throw;
            }
        }
    }
}
