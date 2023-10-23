using BerkutPoiService.Interfaces;
using BerkutPoiService.Models;
using GeoHash.NetCore.Utilities.Decoders;
using GeoHash.NetCore.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerkutPoiService.Services
{
    public class GeoHashService : IGeoHashService
    {
        public string ConvertToGeoHash(GpsRequest gpsRequest)
        {
            var encoder = new GeoHashEncoder<string>();
            return encoder.Encode(gpsRequest.ToGeoCoordinate());
        }

        public Tuple<double, double> DecodeGeoHash(string geoHash)
        {
            var decoder = new GeoHashDecoder<string>();
            return decoder.DecodeAsTuple(geoHash);
        }
    }
}