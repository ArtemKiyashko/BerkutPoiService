using BerkutPoiService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerkutPoiService.Interfaces
{
    public interface IGeoHashService
    {
        string ConvertToGeoHash(double latitude, double longitude);
    }
}
