using BerkutPoiService.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerkutPoiService.Interfaces
{
    public interface IRequestValidator
    {
        CoordinateValidationResult ValidateCoordinates(HttpRequest req);
    }
}
