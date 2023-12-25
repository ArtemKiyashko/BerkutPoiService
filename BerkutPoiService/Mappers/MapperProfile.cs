using AutoMapper;
using BerkutPoiService.Models;
using BerkutPoiService.ViewModels;

namespace BerkutPoiService.Mappers
{
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			CreateMap<PoiSaveRequest, PointOfInterest>().ConvertUsing<SaveRequestToModelConverter>();
            CreateMap<PointOfInterest, PoiResponse>();
        }
	}
}

