using BerkutPoiService.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BerkutPoiService.Interfaces
{
	public interface IPoiRepository
	{
        Task<List<PoiResponse>> GetNearestPointsAsync(string geoHash);
        Task AddPointOfInterestAsync(PoiSaveRequest poiSaveRequest);
    }
}

