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
        
        [JsonProperty("distance")]
        public double Distance { get; set; }
        
        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }
    }
}