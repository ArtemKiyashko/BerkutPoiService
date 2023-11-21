using Azure.Data.Tables;
using BerkutPoiService.Interfaces;
using BerkutPoiService.Models;
using BerkutPoiService.Options;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerkutPoiService.Services
{
    public class StorageService : IStorageService
    {
        private readonly TableClient _tableClient;
        private readonly string _tableName;
        private readonly int _geoHashSearchLength;

        public StorageService(TableServiceClient tableServiceClient, 
            IOptions<StorageServiceOptions> options)
        {
            _tableName = options.Value.TableName;
            _geoHashSearchLength = options.Value.GeoHashSearchLength;

            _tableClient = tableServiceClient.GetTableClient(_tableName);
            _tableClient.CreateIfNotExists();
        }

        public async Task<List<PointOfInterest>> GetNearestPointsAsync(string geoHash)
        {
            var searchGeoHash = geoHash.Substring(0, Math.Min(_geoHashSearchLength, geoHash.Length));

            string filter = TableQuery.GenerateFilterCondition(
                "PartitionKey", QueryComparisons.GreaterThanOrEqual, searchGeoHash);

            var query = _tableClient.QueryAsync<PointOfInterest>(filter: filter);

            var results = new List<PointOfInterest>();

            await foreach (var poi in query)
            {
                results.Add(poi);
            }

            return results;
        }
    }
}