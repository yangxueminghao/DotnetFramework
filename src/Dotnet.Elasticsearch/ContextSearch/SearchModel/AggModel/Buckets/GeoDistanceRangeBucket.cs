using Newtonsoft.Json;

namespace Dotnet.Elasticsearch.ContextSearch.SearchModel.AggModel.Buckets
{
	public class GeoDistanceRangeBucket : BaseBucket
	{
		[JsonProperty("key")]
		public object Key { get; set; }

		[JsonProperty("from")]
		public uint From { get; set; }
	
		[JsonProperty("to")]
		public uint To { get; set; }
	}
}