﻿using BerkutPoiService.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BerkutPoiService.Interfaces
{
    public interface IStorageService
    {
        Task<List<PoiResponse>> GetNearestPointsAsync(double latitude, double longitude);
        Task AddPointOfInterestAsync(PoiSaveRequest poiSaveRequest);
    }
}
