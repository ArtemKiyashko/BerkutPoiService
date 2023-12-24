using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BerkutPoiService.Interfaces;
using BerkutPoiService.Models;
using BerkutPoiService.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BerkutPoiService
{
    public class PoiApi
    {
        private readonly IGeoHashService _geoHashService;
        private readonly IStorageService _storageService;
        private readonly IRequestValidator _requestValidator;
        private readonly int _partitionKeyLength;

        public PoiApi(IGeoHashService geoHashService, IStorageService storageService, 
            IRequestValidator requestValidator, IOptions<StorageServiceOptions> options)
        {
            _geoHashService = geoHashService;
            _storageService = storageService;
            _requestValidator = requestValidator;
            _partitionKeyLength = options.Value.GeoHashPartitionKeyLength;
        }

        [FunctionName("FindNearestPoint")]
        public async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {

            var validationResult = _requestValidator.ValidateCoordinates(req);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.ErrorMessage);
            }

            string geoHash = _geoHashService.ConvertToGeoHash(validationResult.Latitude, validationResult.Longitude);

            List<PointOfInterest> pois = await _storageService.GetNearestPointsAsync(geoHash);

            return new OkObjectResult(pois);
        }

        [FunctionName("AddPointOfInterest")]
        public async Task<IActionResult> Save(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] PoiSaveRequest poiRequest)
        {
            if (poiRequest == null)
            {
                return new BadRequestObjectResult("Invalid request data.");
            }

            try
            {
                if (poiRequest.Lat == null || poiRequest.Long == null)
                {
                    return new BadRequestObjectResult("Latitude and Longitude are required.");
                }

                double latitude = poiRequest.Lat ?? 0.0;
                double longitude = poiRequest.Long ?? 0.0;

                string geoHash = _geoHashService.ConvertToGeoHash(latitude, longitude);

                var poi = new PointOfInterest
                {
                    Lat = latitude,
                    Long = longitude,
                    Name = poiRequest.Name,
                    Content = poiRequest.Content,
                    GeoHash = geoHash,
                    PartitionKey = geoHash.Substring(0, _partitionKeyLength),
                    RowKey = geoHash.Substring(_partitionKeyLength)
                };

                await _storageService.AddPointOfInterestAsync(poi);

                return new OkObjectResult(poi);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}