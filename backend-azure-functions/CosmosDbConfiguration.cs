namespace AbpConf.ServerlessAPI
{
    public static class CosmosDbConfiguration
    {
        public static readonly string EndpointUri = Environment.GetEnvironmentVariable("CosmosDbEndpointUri");
        public static readonly string PrimaryKey = Environment.GetEnvironmentVariable("CosmosDbPrimaryKey");
        public static readonly string DatabaseName = "abp-conf-db";
        public static readonly string ContainerName = "abp-db-container";
    }
}
