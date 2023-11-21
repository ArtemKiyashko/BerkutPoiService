using Azure;
using Azure.Data.Tables;
using BerkutPoiService.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerkutPoiService.Models
{
    public class PointOfInterest : ITableEntity
    {
        private string _geoHash;
        private readonly int _partitionKeyLength;

        public PointOfInterest(IOptions<StorageServiceOptions> options)
        {
            _partitionKeyLength = options.Value.GeoHashPartitionKeyLength;
        }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string GeoHash 
        {
            get { return _geoHash; }
            set
            {
                _geoHash = value;
                PartitionKey = value?.Substring(0, _partitionKeyLength);
            }
        }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}