using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using System;
using System.Net;

namespace AbpConf.ServerlessAPI
{
    public class FunctionAppCall
    {
        private static readonly CosmosDbConfiguration cosmosConfig = CosmosDbConfiguration.GetConfiguration();
        private static readonly CosmosClient cosmosClient = new CosmosClient(cosmosConfig.EndpointUri, cosmosConfig.PrimaryKey);

        [Function("CosmosDbCrud")]
        public async Task<HttpResponseData> CosmosDbCrud(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", "put", "delete", Route = "items/{id?}")] 
            HttpRequestData req,
            string id)
        {
             Container container = cosmosClient.GetContainer(cosmosConfig.DatabaseName, cosmosConfig.ContainerName);

            switch (req.Method)
            {
                case "GET":
                    return await CosmosDbOperations.ReadItemAsync(req, container, id);
                case "POST":
                    return await CosmosDbOperations.CreateItemAsync(req, container);
                case "PUT":
                    return await CosmosDbOperations.UpdateItemAsync(req, container, id);
                case "DELETE":
                    return await CosmosDbOperations.DeleteItemAsync(req, container, id);
                default:
                    var response = req.CreateResponse(HttpStatusCode.BadRequest);
                    await response.WriteStringAsync("BadRequest: Unsupported request method.");
                    return response;
            }
        }
    }
}