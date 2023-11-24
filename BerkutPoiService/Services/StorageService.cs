﻿using Azure.Data.Tables;
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
        private readonly int _partitionKeyLength;

        public StorageService(TableServiceClient tableServiceClient, 
            IOptions<StorageServiceOptions> options)
        {
            _tableName = options.Value.TableName;
            _geoHashSearchLength = options.Value.GeoHashSearchLength;
            _partitionKeyLength = options.Value.GeoHashPartitionKeyLength;

            _tableClient = tableServiceClient.GetTableClient(_tableName);
            _tableClient.CreateIfNotExists();
        }

        public async Task<List<PointOfInterest>> GetNearestPointsAsync(string geoHash)
        {
            var partitionKey = geoHash.Substring(0, Math.Min(_partitionKeyLength, geoHash.Length));
            string startRowKey = geoHash.Length > _partitionKeyLength 
                ? geoHash.Substring(_partitionKeyLength, Math.Min(_geoHashSearchLength - _partitionKeyLength, geoHash.Length - _partitionKeyLength)) 
                : string.Empty;

            string partitionFilter = TableQuery.GenerateFilterCondition(
                "PartitionKey", QueryComparisons.Equal, partitionKey);
            string rowKeyFilter = string.IsNullOrEmpty(startRowKey) 
                ? null 
                : TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThanOrEqual, startRowKey);

            string combinedFilter = rowKeyFilter != null
                ? TableQuery.CombineFilters(partitionFilter, TableOperators.And, rowKeyFilter)
                : partitionFilter;

            var query = _tableClient.QueryAsync<PointOfInterest>(filter: combinedFilter);

            var results = new List<PointOfInterest>();

            await foreach (var poi in query)
            {
                results.Add(poi);
            }

            return results;
        }
    }
}