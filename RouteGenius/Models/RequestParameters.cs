using System.Collections.Generic;
using Newtonsoft.Json;

namespace RouteGenius.Models
{
    public class RequestParameters
    {
        [JsonProperty("startLocation")]
        public Coordinates StartLocation { get; set; }
        
        [JsonProperty("lengthInMeters")]
        public int LengthInMeters { get; set; }
        
        [JsonProperty("travelHeading")]
        public int TravelHeading { get; set; }
        
        [JsonProperty("travelDirection")]
        public int TravelDirection { get; set; }
        
        [JsonProperty("unit")]
        public string Unit { get; set; }
        
        [JsonProperty("routeType")]
        public string RouteType { get; set; }
        
        [JsonProperty("locale")]
        public string Locale { get; set; }
        
        [JsonProperty("avoids")]
        public List<string> Avoids { get; set; }
        
        [JsonProperty("cyclingRoadFactor")]
        public double CyclingRoadFactor { get; set; }
        
        [JsonProperty("roadGradeStrategy")]
        public string RoadGradeStrategy { get; set; }
        
        [JsonProperty("drivingStyle")]
        public int DrivingStyle { get; set; }
        
        [JsonProperty("numberOfItems")]
        public int NumberOfItems { get; set; }
    }
    
    public class Coordinates
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }
        
        [JsonProperty("lng")]
        public double Lng { get; set; }
    }
    
    public class RequestLocation
    {
        [JsonProperty("latLng")]
        public Coordinates LatLng { get; set; }
    }

    public class RequestOptions
    {
        [JsonProperty("avoids")]
        public List<string> Avoids { get; set; }

        [JsonProperty("avoidTimedConditions")]
        public bool AvoidTimedConditions { get; set; }

        [JsonProperty("doReverseGeocode")]
        public bool DoReverseGeocode { get; set; }

        [JsonProperty("shapeFormat")]
        public string ShapeFormat { get; set; }

        [JsonProperty("generalize")]
        public int Generalize { get; set; }

        [JsonProperty("routeType")]
        public string RouteType { get; set; }

        [JsonProperty("timeType")]
        public int TimeType { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }
        
        [JsonProperty("narrativeType")]
        public string NarrativeType { get; set; }

        [JsonProperty("enhancedNarrative")]
        public bool EnhancedNarrative { get; set; }

        [JsonProperty("drivingStyle")]
        public int DrivingStyle { get; set; }

        [JsonProperty("highwayEfficiency")]
        public double HighwayEfficiency { get; set; }
        
        [JsonProperty("cyclingRoadFactor")]
        public double CyclingRoadFactor { get; set; }
        
        [JsonProperty("roadGradeStrategy")]
        public string RoadGradeStrategy { get; set; }
    }

    public class DirectionsRequest
    {
        [JsonProperty("locations")]
        public List<RequestLocation> Locations { get; set; }

        [JsonProperty("options")]
        public RequestOptions Options { get; set; }
    }
    
    public class RouteLocationObject
    {
        public Coordinates Location { get; set; }
    }
}