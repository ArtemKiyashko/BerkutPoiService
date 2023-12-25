using BerkutPoiService.Interfaces;
using BerkutPoiService.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BerkutPoiService.Services
{
    public class StorageService : IStorageService
    {
        private readonly IPoiRepository _poiRepository;
        private readonly IGeoHashService _geoHashService;

        public StorageService(
            IPoiRepository poiRepository,
            IGeoHashService geoHashService)
        {
            _poiRepository = poiRepository;
            _geoHashService = geoHashService;
        }

        public async Task<List<PoiResponse>> GetNearestPointsAsync(double latitude, double longitude)
        {
            string geoHash = _geoHashService.ConvertToGeoHash(latitude, longitude);
            return await _poiRepository.GetNearestPointsAsync(geoHash);
        }

        public async Task AddPointOfInterestAsync(PoiSaveRequest poiSaveRequest)
        {
            await _poiRepository.AddPointOfInterestAsync(poiSaveRequest);
        }
    }
}