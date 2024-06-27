# Dewiride.Azure.Storage.Table.Helper

![NuGet Version](https://img.shields.io/nuget/v/Dewiride.Azure.Storage.Table.Helper)
![NuGet Downloads](https://img.shields.io/nuget/dt/Dewiride.Azure.Storage.Table.Helper)

`Dewiride.Azure.Storage.Table.Helper` is a .NET library that provides helper methods and an interface for interacting with Azure Table Storage.

## Features

- Retrieve single or multiple entities from Azure Table Storage.
- Query entities by partition key, row key, or email.
- Insert or merge entities into Azure Table Storage.
- Delete entities from Azure Table Storage.
- Easy-to-use asynchronous methods.

## Installation

You can install the package via NuGet:

```bash
dotnet add package Dewiride.Azure.Storage.Table.Helper
```

Or via the NuGet Package Manager in Visual Studio.

## Usage

### Step 1: Configure Dependencies

Ensure you have the necessary NuGet packages installed:

- `Azure.Data.Tables`
- `Microsoft.Extensions.Logging.Abstractions`

### Step 2: Create a TableServiceClient

First, create an instance of `TableServiceClient` and configure logging.

```csharp
using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

var serviceClient = new TableServiceClient("<Your_Connection_String>");
var logger = NullLogger<TableStorageHelper>.Instance;
```

### Step 3: Initialize the TableStorageHelper

Create an instance of `TableStorageHelper` using the `TableServiceClient` and `ILogger`.

```csharp
using Azure.Storage.Table.Helper;

var tableStorageHelper = new TableStorageHelper(logger, serviceClient);
```

Initializing the `TableStorageHelper` from a DI container is recommended.

```csharp
builder.Services.AddAzureClients(clientBuilder =>
            {
                clientBuilder.AddTableServiceClient(builder.Configuration["ConnectionString"] ?? throw new NullReferenceException("Table Storage not configured."));
            });

builder.Services.AddSingleton<ITableStorageHelper, TableStorageHelper>();
```

Usage in a service:

```csharp
public class MyService
{
	private readonly ITableStorageHelper _tableStorageHelper;

	public MyService(ITableStorageHelper tableStorageHelper)
	{
		_tableStorageHelper = tableStorageHelper;
	}

	public async Task<MyEntity> GetEntityAsync(string partitionKey, string rowKey)
	{
		return await _tableStorageHelper.GetEntityAsync<MyEntity>("TableName", partitionKey, rowKey);
	}
}
```

### Step 4: Use the Helper Methods

You can now use the helper methods to interact with Azure Table Storage.

#### Retrieve a Single Entity

```csharp
var entity = await tableStorageHelper.GetEntityAsync<MyEntity>("TableName", "PartitionKey", "RowKey");
```

#### Retrieve Entities by Email

```csharp
var entity = await tableStorageHelper.GetEntityByEmailAsync<MyEntity>("TableName", "PartitionKey", "email@example.com");
```

#### Retrieve All Entities by Partition Key

```csharp
var entities = await tableStorageHelper.GetEntitiesAsync<MyEntity>("TableName", "PartitionKey");
```

#### Insert or Merge an Entity

```csharp
var entity = new MyEntity
{
    PartitionKey = "PartitionKey",
    RowKey = "RowKey",
    Property = "Value"
};

var response = await tableStorageHelper.UpsertEntityAsync("TableName", entity);
```

#### Delete an Entity

```csharp
var response = await tableStorageHelper.DeleteEntityAsync("TableName", "PartitionKey", "RowKey");
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

## Contact

For more information or support, please contact Jagdish Kumawat at [support@dewiride.com](mailto:support@dewiride.com).

## Acknowledgments

- [Azure Data Tables](https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/tables/Azure.Data.Tables)
- [Microsoft.Extensions.Logging.Abstractions](https://github.com/dotnet/extensions)