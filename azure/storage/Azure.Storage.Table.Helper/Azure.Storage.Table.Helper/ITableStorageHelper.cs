using Azure.Data.Tables;

namespace Azure.Storage.Table.Helper
{
    /// <summary>
    /// Defines methods for interacting with Azure Table Storage.
    /// </summary>
    public interface ITableStorageHelper
    {
        /// <summary>
        /// Asynchronously retrieves a single entity from Azure Table Storage.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="partitionKey">The partition key of the entity.</param>
        /// <param name="rowKey">The row key of the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the retrieved entity or null if not found.</returns>
        Task<T?> GetEntityAsync<T>(string tableName, string partitionKey, string rowKey) where T : class, ITableEntity, new();

        /// <summary>
        /// Asynchronously retrieves an entity from Azure Table Storage by email.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="partitionKey">The partition key of the entity.</param>
        /// <param name="email">The email of the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the retrieved entity or null if not found.</returns>
        Task<T?> GetEntityByEmailAsync<T>(string tableName, string partitionKey, string email) where T : class, ITableEntity, new();

        /// <summary>
        /// Asynchronously retrieves all entities from a table in Azure Table Storage that match the specified partition key.
        /// </summary>
        /// <typeparam name="T">The type of the entities.</typeparam>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="partitionKey">The partition key to filter by.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of retrieved entities.</returns>
        Task<List<T>> GetEntitiesAsync<T>(string? tableName, string? partitionKey) where T : class, ITableEntity, new();

        /// <summary>
        /// Asynchronously retrieves the latest entity from a table in Azure Table Storage.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="tableName">The name of the table.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the retrieved entity or null if not found.</returns>
        Task<T?> GetEntityAsync<T>(string? tableName) where T : class, ITableEntity, new();

        /// <summary>
        /// Asynchronously retrieves all entities from a table in Azure Table Storage that match the specified filter.
        /// </summary>
        /// <typeparam name="T">The type of the entities.</typeparam>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="filter">The filter to apply.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of retrieved entities.</returns>
        Task<List<T>> GetEntitiesAsync<T>(string? tableName, FormattableString filter) where T : class, ITableEntity, new();

        /// <summary>
        /// Asynchronously inserts or merges an entity in Azure Table Storage.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="entity">The entity to upsert.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the Azure response.</returns>
        Task<Response?> UpsertEntityAsync<T>(string? tableName, T entity) where T : class, ITableEntity, new();

        /// <summary>
        /// Asynchronously deletes a single entity from Azure Table Storage.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <param name="partitionKey">The partition key of the entity.</param>
        /// <param name="rowKey">The row key of the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the Azure response.</returns>
        Task<Response> DeleteEntityAsync(string? tableName, string partitionKey, string rowKey);
    }
}