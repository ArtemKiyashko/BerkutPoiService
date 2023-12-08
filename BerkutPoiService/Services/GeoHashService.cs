using BerkutPoiService.Interfaces;
using BerkutPoiService.Models;
using GeoHash.NetCore.GeoCoords;
using GeoHash.NetCore.Utilities.Decoders;
using GeoHash.NetCore.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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