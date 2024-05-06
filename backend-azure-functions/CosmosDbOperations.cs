using System.Net;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;

namespace AbpConf.ServerlessAPI
{
    public static class CosmosDbOperations
    {
        public static async Task<HttpResponseData> CreateItemAsync(HttpRequestData req, Container container)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            await container.CreateItemAsync(data, new PartitionKey(data?.id.ToString()));
            var response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteStringAsync("Item was added successfully");
            return response;
        }
        public static async Task<HttpResponseData> ReadItemAsync(HttpRequestData req, Container container, string id)
        {
            try
            {
                ItemResponse<dynamic> responseItem = await container.ReadItemAsync<dynamic>(id, new PartitionKey(id));
                var response = req.CreateResponse(HttpStatusCode.OK);
                string responseData = JsonConvert.SerializeObject(responseItem.Resource);
                await response.WriteStringAsync(responseData);
                return response;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                var response = req.CreateResponse(HttpStatusCode.NotFound);
                await response.WriteStringAsync("Item does not exist");
                return response;
            }
        }
        public static async Task<HttpResponseData> ReadItemAsync(HttpRequestData req, Container container)
        {
            try
            {
                List<dynamic> returnedArray = new List<dynamic>();
                FeedIterator<dynamic> feedIterator = container.GetItemQueryIterator<dynamic>();                
                while (feedIterator.HasMoreResults)
                {
                    foreach (dynamic item in await feedIterator.ReadNextAsync())
                    {
                        returnedArray.Add(item);
                    }
                }                
                var response = req.CreateResponse(HttpStatusCode.OK);
                string responsestring = JsonConvert.SerializeObject(returnedArray);
                await response.WriteStringAsync(responsestring);
                return response;
            }
            catch (CosmosException e)
            {
                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync($"Error occurred: {e.Message}");
                return errorResponse;
            }
        }
        public static async Task<HttpResponseData> UpdateItemAsync(HttpRequestData req, Container container, string id)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            await container.UpsertItemAsync(data, new PartitionKey(id));
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync("Item was successfully updated");
            return response;
        }
        public static async Task<HttpResponseData> DeleteItemAsync(HttpRequestData req, Container container, string id)
        {
            await container.DeleteItemAsync<dynamic>(id, new PartitionKey(id));
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync("successfully deleted");
            return response;
        }
    }
}
