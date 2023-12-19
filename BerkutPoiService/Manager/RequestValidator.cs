using System.Globalization;
using BerkutPoiService.Interfaces;
using BerkutPoiService.Models;
using Microsoft.AspNetCore.Http;

namespace BerkutPoiService.Manager
{
    public class RequestValidator : IRequestValidator
    {
        public CoordinateValidationResult ValidateCoordinates(HttpRequest req)
        {
            var result = new CoordinateValidationResult();

            if (!req.Query.ContainsKey("lat") || !req.Query.ContainsKey("long"))
            {
                result.IsValid = false;
                result.ErrorMessage = "Missing required parameters: 'lat' and 'long'.";
                return result;
            }

            string latStr = req.Query["lat"];
            string longStr = req.Query["long"];

            if (!double.TryParse(latStr, NumberStyles.Number, CultureInfo.InvariantCulture, out double lat) || !double.TryParse(longStr, NumberStyles.Number, CultureInfo.InvariantCulture, out double lon))
            {
                result.IsValid = false;
                result.ErrorMessage = "Invalid format for coordinates";
                return result;
            }

            if (lat < -90 || lat > 90 || lon < -180 || lon > 180)
            {
                result.IsValid = false;
                result.ErrorMessage = "Coordinates out of range. Latitude must be between -90 and 90, and longitude must be between -180 and 180.";
                return result;
            }

            result.IsValid = true;
            result.Latitude = lat;
            result.Longitude = lon;
            return result;
        }
    }
}
