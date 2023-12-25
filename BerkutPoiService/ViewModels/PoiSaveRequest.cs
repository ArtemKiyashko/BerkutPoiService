using System;
using System.ComponentModel.DataAnnotations;

namespace BerkutPoiService.ViewModels
{
    public class PoiSaveRequest
    {
        [Required]
        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90.")]
        public double? Lat { get; set; }

        [Required]
        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180.")]
        public double? Long { get; set; }

        [Required]
        public string Name { get; set; }

        public string Content { get; set; }

        public string Assets { get; set; }
    }
}
