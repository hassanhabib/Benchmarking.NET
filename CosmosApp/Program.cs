using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace CosmosApp
{
    class Program
    {
        static void Main(string[] args)
        {

            var connectionString = "DefaultEndpointsProtocol=https;AccountName=studentscosmos;AccountKey=97H9pQoeT8TinyafUNqAzfRQn8Y8PksUph92AwzmkoCjkyAKlrLhEK4ftELRfn157QW5NCK7r6sQeY8aqW63Zg==;TableEndpoint=https://studentscosmos.table.cosmos.azure.com:443/;";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("TablesDb");

            TableQuery<Student> tableQuery = new TableQuery<Student>()
                .Where(
                    TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "1"),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition("Name", QueryComparisons.Equal, "Hassan")
                ));

            var results = table.ExecuteQuerySegmentedAsync<Student>(tableQuery, null).Result;

            Console.ReadKey();
        }

        class Student : ITableEntity
        {
            public string Name { get; set; }
            public int Score { get; set; }
            public string PartitionKey { get; set; }
            public string RowKey { get; set; }

            public DateTimeOffset Timestamp { get; set; }
            public string ETag { get; set; }

            public void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
            {
                throw new NotImplementedException();
            }

            public IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
            {
                throw new NotImplementedException();
            }
        }
    }
}
