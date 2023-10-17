using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace BerkutPoiService
{
    public class PoiService
    {
        [FunctionName("nearest")]
        public async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] GpsRequest gpsRequest)
        {
            return new OkObjectResult();
        }
    }
}

