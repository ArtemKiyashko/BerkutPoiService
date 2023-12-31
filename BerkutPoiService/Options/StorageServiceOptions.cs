﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerkutPoiService.Options
{
    public class StorageServiceOptions
    {
        public string StorageAccountConnectionString { get; set; }
        public string TableName { get; set; }
        public int GeoHashPartitionKeyLength { get; set; }
        public int GeoHashSearchLength { get; set; }
        public int GeoHashLength { get; set; }
    }
}
