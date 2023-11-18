using Azure;
using Azure.Data.Tables;
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
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string GeoHash 
        {
            get { return PartitionKey; }
            set { PartitionKey = value; }
        }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}