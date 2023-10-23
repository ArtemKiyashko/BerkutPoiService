using GeoHash.NetCore.GeoCoords;

namespace BerkutPoiService.Models
{
    public class GpsRequest
    {
        public double Lat { get; set; }
        public double Long { get; set; }

        public GeoCoordinate ToGeoCoordinate()
        {
            return new GeoCoordinate(Lat, Long);
        }
    }
}