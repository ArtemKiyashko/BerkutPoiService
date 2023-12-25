using AutoMapper;
using BerkutPoiService.Interfaces;
using BerkutPoiService.Models;
using BerkutPoiService.Options;
using BerkutPoiService.ViewModels;
using Microsoft.Extensions.Options;

namespace BerkutPoiService.Mappers
{
	public class SaveRequestToModelConverter : ITypeConverter<PoiSaveRequest, PointOfInterest>
	{
        private readonly IGeoHashService _geoHashService;
        private readonly StorageServiceOptions _options;

        public SaveRequestToModelConverter(
            IGeoHashService geoHashService,
            IOptions<StorageServiceOptions> options)
		{
            _geoHashService = geoHashService;
            _options = options.Value;
        }

        public PointOfInterest Convert(PoiSaveRequest source, PointOfInterest destination, ResolutionContext context)
        {
            double latitude = source.Lat ?? 0.0;
            double longitude = source.Long ?? 0.0;

            string geoHash = _geoHashService.ConvertToGeoHash(latitude, longitude);

            destination = new PointOfInterest
            {
                Lat = latitude,
                Long = longitude,
                Name = source.Name,
                Content = source.Content,
                GeoHash = geoHash,
                PartitionKey = geoHash.Substring(0, _options.GeoHashPartitionKeyLength),
                RowKey = geoHash.Substring(_options.GeoHashPartitionKeyLength),
                Assets = source.Assets
            };

            return destination;
        }
    }
}

