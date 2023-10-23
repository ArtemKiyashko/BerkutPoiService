using System.Threading.Tasks;
using BerkutPoiService.Interfaces;
using BerkutPoiService.Models;
using GeoHash.NetCore.Utilities.Encoders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;


namespace BerkutPoiService
{
    public class PoiService
    {
        private readonly IGeoHashService _geoHashService;
        private readonly IStorageService _storageService;

        public PoiService(IGeoHashService geoHashService, IStorageService storageService)
        {
            _geoHashService = geoHashService;
            _storageService = storageService;
        }

        [FunctionName("FindNearestPoint")]
        public async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] [FromQuery] GpsRequest gpsRequest)
        {
            var geoHash = _geoHashService.ConvertToGeoHash(gpsRequest);
            var nearestPoint = await _storageService.GetNearestPointAsync(geoHash);

            if (nearestPoint == null)
            {
                return new NotFoundResult();
            }

            var decodedCoordinates = _geoHashService.DecodeGeoHash(nearestPoint.GeoHash);

            var response = new
            {
                Latitude = decodedCoordinates.Item1,
                Longitude = decodedCoordinates.Item2,
                Name = nearestPoint.Name,
                Content = nearestPoint.Content
            };

            return new OkObjectResult(response);
        }
    }
}