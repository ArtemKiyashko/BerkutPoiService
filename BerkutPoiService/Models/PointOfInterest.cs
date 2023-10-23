using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerkutPoiService.Models
{
    public class PointOfInterest
    {
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string GeoHash { get; set; }
    }
}