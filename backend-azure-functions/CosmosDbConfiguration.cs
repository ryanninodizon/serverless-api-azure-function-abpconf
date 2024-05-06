using System;

namespace AbpConf.ServerlessAPI
{
    public class CosmosDbConfiguration
    {
        public string EndpointUri { get; }
        public string PrimaryKey { get; }
        public string DatabaseName { get; }
        public string ContainerName { get; }

        public CosmosDbConfiguration(string endpointUri, string primaryKey, string databaseName, string containerName)
        {
            EndpointUri = endpointUri;
            PrimaryKey = primaryKey;
            DatabaseName = databaseName;
            ContainerName = containerName;
        }

        public static CosmosDbConfiguration GetConfiguration()
        {
            string endpointUri = Environment.GetEnvironmentVariable("CosmosDbEndpointUri");
            string primaryKey = Environment.GetEnvironmentVariable("CosmosDbPrimaryKey");
            string databaseName = "abp-conf-db";
            string containerName = "abp-db-container";
            return new CosmosDbConfiguration(endpointUri, primaryKey, databaseName, containerName);
        }
    }
}
