using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BerkutPoiService.Interfaces;
using BerkutPoiService.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace BerkutPoiService
{
    public class PoiApi
    {
        private readonly IStorageService _storageService;
        private readonly IRequestValidator _requestValidator;

        public PoiApi(
            IStorageService storageService, 
            IRequestValidator requestValidator)
        {
            _storageService = storageService;
            _requestValidator = requestValidator;
        }

        [FunctionName("FindNearestPoint")]
        public async Task<IActionResult> Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {

            var validationResult = _requestValidator.ValidateCoordinates(req);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.ErrorMessage);
            }

            List<PoiResponse> pois = await _storageService.GetNearestPointsAsync(validationResult.Latitude, validationResult.Longitude);

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

                await _storageService.AddPointOfInterestAsync(poiRequest);

                return new OkObjectResult(poiRequest);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}