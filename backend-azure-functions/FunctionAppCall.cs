using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Cosmos;
using System.Net;

namespace AbpConf.ServerlessAPI
{
    public class FunctionAppCall
    {
        private static readonly CosmosClient cosmosClient = new CosmosClient(CosmosDbConfiguration.EndpointUri, CosmosDbConfiguration.PrimaryKey);
        [Function("FunctionAppCall")]
        public async Task<HttpResponseData> CosmosDbCrud(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", "put", "delete", Route = "items/{id?}")] 
            HttpRequestData req,
            string id)
        {
             Container container = cosmosClient.GetContainer(CosmosDbConfiguration.DatabaseName, CosmosDbConfiguration.ContainerName);

            switch (req.Method)
            {
                case "GET":
                     if(id is null){
                        return await CosmosDbOperations.ReadItemAsync(req, container); //Get all result
                     }else{
                        return await CosmosDbOperations.ReadItemAsync(req, container,id);
                     }                    
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