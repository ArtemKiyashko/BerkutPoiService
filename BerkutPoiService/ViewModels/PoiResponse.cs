namespace BerkutPoiService.ViewModels
{
	public class PoiResponse
	{
        public double Lat { get; set; }
        public double Long { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string GeoHash { get; set; }
        public string Assets { get; set; }
    }
}

