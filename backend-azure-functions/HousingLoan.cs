using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AbpConf.ServerlessAPI
{
    public class HousingLoan
    {
        private readonly ILogger<HousingLoan> _logger;

        public HousingLoan(ILogger<HousingLoan> logger)
        {
            _logger = logger;
        }

        [Function("HousingLoan")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
