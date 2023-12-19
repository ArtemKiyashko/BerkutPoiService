﻿using BerkutPoiService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerkutPoiService.Interfaces
{
    public interface IStorageService
    {
        Task<List<PointOfInterest>> GetNearestPointsAsync(string geohash);
        Task AddPointOfInterestAsync(PointOfInterest poi);
    }
}
