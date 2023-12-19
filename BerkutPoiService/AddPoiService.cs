using BerkutPoiService.Interfaces;
using BerkutPoiService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BerkutPoiService.Options;
using Microsoft.Extensions.Options;
using BerkutPoiService.Manager;

namespace BerkutPoiService
{
    public class AddPoiService
    {
        private readonly IStorageService _storageService;
        private readonly IGeoHashService _geoHashService;
        private readonly IRequestValidator _requestValidator;
        private readonly int _partitionKeyLength;

        public AddPoiService(IStorageService storageService, IGeoHashService geoHashService,
            IRequestValidator requestValidator, IOptions<StorageServiceOptions> options)
        {
            _storageService = storageService;
            _geoHashService = geoHashService;
            _requestValidator = requestValidator;
            _partitionKeyLength = options.Value.GeoHashPartitionKeyLength;
        }

        [FunctionName("AddPointOfInterest")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            var validationResult = _requestValidator.ValidateCoordinates(req);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.ErrorMessage);
            }

            string geoHash = _geoHashService.ConvertToGeoHash(validationResult.Latitude, validationResult.Longitude);

            string poiName = req.Query["name"];
            string poiContent = req.Query["content"];

            if (string.IsNullOrWhiteSpace(poiName) || string.IsNullOrWhiteSpace(poiContent))
            {
                return new BadRequestObjectResult("Name and content are required.");
            }

            var poi = new PointOfInterest
            {
                Lat = validationResult.Latitude,
                Long = validationResult.Longitude,
                Name = poiName,
                Content = poiContent,
                GeoHash = geoHash,
                PartitionKey = geoHash.Substring(0, _partitionKeyLength),
                RowKey = geoHash.Substring(_partitionKeyLength)
            };

            await _storageService.AddPointOfInterestAsync(poi);

            return new OkObjectResult(poi);
        }
    }
}
