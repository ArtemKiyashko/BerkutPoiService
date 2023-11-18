using System.Threading.Tasks;
using BerkutPoiService.Interfaces;
using BerkutPoiService.Models;
using GeoHash.NetCore.Utilities.Encoders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;


namespace BerkutPoiService
{
    public class PoiService
    {
        private readonly IGeoHashService _geoHashService;
        private readonly IStorageService _storageService;
        private readonly IRequestValidator _requestValidator;

        public PoiService(IGeoHashService geoHashService, IStorageService storageService, IRequestValidator requestValidator)
        {
            _geoHashService = geoHashService;
            _storageService = storageService;
            _requestValidator = requestValidator;
        }

        [FunctionName("FindNearestPoint")]
        public async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            //var test = new PointOfInterest
            //{
            //    Lat = (decimal)lat,
            //    Long = (decimal)lon
            //};

            //return new OkObjectResult(test);

            var validationResult = _requestValidator.ValidateCoordinates(req);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.ErrorMessage);
            }

            string geoHash = _geoHashService.ConvertToGeoHash(validationResult.Latitude, validationResult.Longitude);

            PointOfInterest poi = await _storageService.GetNearestPointAsync(geoHash);

            if (poi == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(poi);
        }
    }
}