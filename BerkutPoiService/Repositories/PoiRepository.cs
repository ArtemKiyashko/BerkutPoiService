using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Data.Tables;
using BerkutPoiService.Interfaces;
using BerkutPoiService.Models;
using BerkutPoiService.Options;
using BerkutPoiService.ViewModels;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Table;

namespace BerkutPoiService.Repositories
{
	public class PoiRepository : IPoiRepository
	{
        private readonly TableClient _tableClient;
        public readonly StorageServiceOptions _options;
        private readonly IMapper _mapper;

        public PoiRepository(
            TableServiceClient tableServiceClient,
            IOptions<StorageServiceOptions> options,
            IMapper mapper)
		{
            _options = options.Value;
            _tableClient = tableServiceClient.GetTableClient(_options.TableName);
            _tableClient.CreateIfNotExists();
            _mapper = mapper;
        }

        public async Task AddPointOfInterestAsync(PoiSaveRequest poiSaveRequest)
        {
            var poi = _mapper.Map<PointOfInterest>(poiSaveRequest);
            await _tableClient.AddEntityAsync(poi);
        }

        public async Task<List<PoiResponse>> GetNearestPointsAsync(string geoHash)
        {
            var partitionKey = geoHash.Substring(0, Math.Min(_options.GeoHashPartitionKeyLength, geoHash.Length));
            string startRowKey = geoHash.Length > _options.GeoHashPartitionKeyLength
                ? geoHash.Substring(_options.GeoHashPartitionKeyLength, Math.Min(_options.GeoHashSearchLength - _options.GeoHashPartitionKeyLength, geoHash.Length - _options.GeoHashPartitionKeyLength))
                : string.Empty;
            string endRowKey = startRowKey + new string('~', _options.GeoHashLength - startRowKey.Length);

            string partitionFilter = TableQuery.GenerateFilterCondition(
                "PartitionKey", QueryComparisons.Equal, partitionKey);
            string startRowKeyFilter = TableQuery.GenerateFilterCondition(
                "RowKey", QueryComparisons.GreaterThanOrEqual, startRowKey);
            string endRowKeyFilter = TableQuery.GenerateFilterCondition(
                "RowKey", QueryComparisons.LessThan, endRowKey);
            string disabledFilter = "Disabled eq false";

            string combinedFilter = TableQuery.CombineFilters(
                TableQuery.CombineFilters(
                    TableQuery.CombineFilters(partitionFilter, TableOperators.And, startRowKeyFilter),
                    TableOperators.And, endRowKeyFilter),
                TableOperators.And,
                disabledFilter);

            var query = _tableClient.QueryAsync<PointOfInterest>(filter: combinedFilter);

            var results = new List<PoiResponse>();

            await foreach (var poi in query)
            {
                results.Add(_mapper.Map<PoiResponse>(poi));
            }

            return results;
        }
    }
}

