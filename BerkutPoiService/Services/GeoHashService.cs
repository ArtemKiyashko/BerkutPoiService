using BerkutPoiService.Interfaces;
using GeoHash.NetCore.GeoCoords;
using GeoHash.NetCore.Utilities.Encoders;

namespace BerkutPoiService.Services
{
    public class GeoHashService : IGeoHashService
    {
        public string ConvertToGeoHash(double latitude, double longitude)
        {
            var encoder = new GeoHashEncoder<string>();
            var geoCoordinate = new GeoCoordinate(latitude, longitude);
            return encoder.Encode(geoCoordinate);
        }
    }
}