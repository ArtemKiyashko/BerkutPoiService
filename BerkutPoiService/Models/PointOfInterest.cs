using Azure;
using Azure.Data.Tables;
using System;

namespace BerkutPoiService.Models
{
    public class PointOfInterest : ITableEntity
    {
        public double Lat { get; set; }
        public double Long { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string GeoHash { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string Assets { get; set; }
    }
}