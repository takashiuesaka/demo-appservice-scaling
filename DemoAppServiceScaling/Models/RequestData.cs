using Newtonsoft.Json;

namespace DemoAppServiceScaling.Models
{
    public class RequestData
    {
        public static string ParitionKey = "/MachineName";

        [JsonProperty("id")]
        public string Id { get; set; }
        
        public DateTime AccessDateTime { get; set; }

        public string MachineName { get; set; }

    }
}
