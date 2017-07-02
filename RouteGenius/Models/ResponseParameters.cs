using System.Collections.Generic;
using Newtonsoft.Json;

namespace RouteGenius.Models
{
    public class ResponseParameters
    {
        [JsonProperty("result")]
        public List<Result> Result { get; set; }
    }

    public class Result
    {
        [JsonProperty("coordinates")]
        public List<LatLng> Coordinates { get; set; }
        
        [JsonProperty("distanceInMeters")]
        public double DistanceInMeters { get; set; }
        
        [JsonProperty("thumbnailUrl")]
        public string ThumbnailUrl { get; set; }

        [JsonProperty("uniqueId")]
        public string UniqueId { get; set; }
    }
}