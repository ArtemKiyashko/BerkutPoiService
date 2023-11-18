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

        public StorageService(TableServiceClient tableServiceClient, 
            IOptions<StorageServiceOptions> options)
        {
            _tableName = options.Value.TableName;

            _tableClient = tableServiceClient.GetTableClient(_tableName);
            _tableClient.CreateIfNotExists();
        }

        public async Task<PointOfInterest> GetNearestPointAsync(string geoHash)
        {
            string filter = TableQuery.GenerateFilterCondition(
                "PartitionKey", QueryComparisons.GreaterThanOrEqual, geoHash);

            var query = _tableClient.QueryAsync<PointOfInterest>(filter: filter).AsPages();

            await foreach (var page in query)
            {
                foreach (var poi in page.Values)
                {
                    return poi;
                }
            }
            return null;
        }
    }
}