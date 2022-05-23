using DemoAppServiceScaling.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System.Diagnostics;

namespace DemoAppServiceScaling.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private CosmosDbContainer CosmosDbContainer { get; }

        public HomeController(
            CosmosDbContainer cosmosDbContainer,
            ILogger<HomeController> logger)
        {
            this.CosmosDbContainer = cosmosDbContainer;
            _logger = logger;
        }


        [HttpGet("/")]
        public async Task<IActionResult> Index()
        {
            Container container = await this.CosmosDbContainer.Get();

            var query = container.GetItemQueryIterator<RequestData>(new QueryDefinition("Select * from c"));
            List<RequestData> requestedList = new List<RequestData>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                requestedList.AddRange(response.ToList());
            }

            var requestSummaryList = requestedList.GroupBy(x => x.MachineName).Select(x => new RequestSummary{ MachineName = x.Key, Count = x.Count()}).ToList();

            return View(requestSummaryList);
        }

        [HttpGet("applyload")]
        public async Task<IActionResult> ApplyLoad()
        {
            var item = new RequestData() { Id = Guid.NewGuid().ToString(), MachineName = Environment.MachineName, AccessDateTime = DateTime.UtcNow };

            Container container = await this.CosmosDbContainer.Get();
            await container.CreateItemAsync(item, new PartitionKey(item.MachineName));

            // 負荷
            var bytes = new byte[1024 * 1024];
            for (var i = 0; i < 1024 * 1024; i++)
            {
                bytes[i] = (byte)i;
            }

            return await this.Index();
        }

        [HttpGet("/deleteallresults")]
        public async Task<IActionResult> Delete()
        {
            await this.CosmosDbContainer.Delete();
            await this.CosmosDbContainer.Create();

            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}