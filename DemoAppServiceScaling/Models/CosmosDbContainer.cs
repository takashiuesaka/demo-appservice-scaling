using Microsoft.Azure.Cosmos;

namespace DemoAppServiceScaling.Models
{
    public class CosmosDbContainer
    {
        private CosmosDbInfo CosmosDbInfo { get; }

        private Container? Container { get; set; }

        public CosmosDbContainer(CosmosDbInfo cosmosDbInfo)
        {
            this.CosmosDbInfo = cosmosDbInfo;
        }

        public async Task<Container> Get()
        {
            if (this.Container == null)
            {
                this.Container = await this.Create();
            }

            return this.Container;

        }

        public async Task<Container> Create()
        {
            var client = new CosmosClient(this.CosmosDbInfo.Account, this.CosmosDbInfo.Key);
            DatabaseResponse databaseResponse = await client.CreateDatabaseIfNotExistsAsync(this.CosmosDbInfo.DatabaseName);
            var containerResponse = await databaseResponse.Database.CreateContainerIfNotExistsAsync(this.CosmosDbInfo.ContainerName, RequestData.ParitionKey);

            return containerResponse.Container;
        }

        public async Task Delete()
        {
            if (this.Container == null) return;

            await this.Container.DeleteContainerAsync();
            this.Container = null;
        }
    }
}
