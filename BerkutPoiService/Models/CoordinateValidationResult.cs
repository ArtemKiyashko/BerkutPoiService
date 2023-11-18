using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerkutPoiService.Models
{
    public class CoordinateValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
