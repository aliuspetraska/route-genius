using System.Collections.Generic;
using Newtonsoft.Json;

namespace RouteGenius.Models
{
    public class Shape
    {
        [JsonProperty("maneuverIndexes")]
        public List<int> ManeuverIndexes { get; set; }

        [JsonProperty("shapePoints")]
        public List<double> ShapePoints { get; set; }

        [JsonProperty("legIndexes")]
        public List<int> LegIndexes { get; set; }
    }

    public class BoundingBox
    {
        [JsonProperty("ul")]
        public Coordinates Ul { get; set; }

        [JsonProperty("lr")]
        public Coordinates Lr { get; set; }
    }

    public class LatLng
    {
        [JsonProperty("lng")]
        public double Lng { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }
    }

    public class Location
    {
        [JsonProperty("latLng")]
        public LatLng LatLng { get; set; }

        [JsonProperty("adminArea4")]
        public string AdminArea4 { get; set; }

        [JsonProperty("adminArea5Type")]
        public string AdminArea5Type { get; set; }

        [JsonProperty("adminArea4Type")]
        public string AdminArea4Type { get; set; }

        [JsonProperty("adminArea5")]
        public string AdminArea5 { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("adminArea1")]
        public string AdminArea1 { get; set; }

        [JsonProperty("adminArea3")]
        public string AdminArea3 { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("displayLatLng")]
        public Coordinates DisplayLatLng { get; set; }

        [JsonProperty("linkId")]
        public int LinkId { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("sideOfStreet")]
        public string SideOfStreet { get; set; }

        [JsonProperty("dragPoint")]
        public bool DragPoint { get; set; }

        [JsonProperty("adminArea1Type")]
        public string AdminArea1Type { get; set; }

        [JsonProperty("geocodeQuality")]
        public string GeocodeQuality { get; set; }

        [JsonProperty("geocodeQualityCode")]
        public string GeocodeQualityCode { get; set; }

        [JsonProperty("adminArea3Type")]
        public string AdminArea3Type { get; set; }
    }
    
    public class Sign
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("extraText")]
        public string ExtraText { get; set; }

        [JsonProperty("direction")]
        public int Direction { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class Maneuver
    {
        [JsonProperty("signs")]
        public List<Sign> Signs { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("maneuverNotes")]
        public List<string> ManeuverNotes { get; set; }

        [JsonProperty("direction")]
        public int Direction { get; set; }

        [JsonProperty("narrative")]
        public string Narrative { get; set; }

        [JsonProperty("iconUrl")]
        public string IconUrl { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("linkIds")]
        public List<object> LinkIds { get; set; }

        [JsonProperty("streets")]
        public List<string> Streets { get; set; }

        [JsonProperty("attributes")]
        public int Attributes { get; set; }

        [JsonProperty("transportMode")]
        public string TransportMode { get; set; }

        [JsonProperty("formattedTime")]
        public string FormattedTime { get; set; }

        [JsonProperty("directionName")]
        public string DirectionName { get; set; }

        [JsonProperty("mapUrl")]
        public string MapUrl { get; set; }

        [JsonProperty("startPoint")]
        public Coordinates StartPoint { get; set; }

        [JsonProperty("turnType")]
        public int TurnType { get; set; }
    }

    public class Leg
    {

        [JsonProperty("hasTollRoad")]
        public bool HasTollRoad { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("hasBridge")]
        public bool HasBridge { get; set; }

        [JsonProperty("hasTunnel")]
        public bool HasTunnel { get; set; }

        [JsonProperty("roadGradeStrategy")]
        public List<List<string>> RoadGradeStrategy { get; set; }

        [JsonProperty("hasHighway")]
        public bool HasHighway { get; set; }

        [JsonProperty("hasUnpaved")]
        public bool HasUnpaved { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("origIndex")]
        public int OrigIndex { get; set; }

        [JsonProperty("hasSeasonalClosure")]
        public bool HasSeasonalClosure { get; set; }

        [JsonProperty("origNarrative")]
        public string OrigNarrative { get; set; }

        [JsonProperty("hasCountryCross")]
        public bool HasCountryCross { get; set; }

        [JsonProperty("formattedTime")]
        public string FormattedTime { get; set; }

        [JsonProperty("destNarrative")]
        public string DestNarrative { get; set; }

        [JsonProperty("destIndex")]
        public int DestIndex { get; set; }

        [JsonProperty("maneuvers")]
        public List<Maneuver> Maneuvers { get; set; }

        [JsonProperty("hasFerry")]
        public bool HasFerry { get; set; }
    }

    public class RouteError
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("errorCode")]
        public int ErrorCode { get; set; }
    }

    public class Options
    {
        [JsonProperty("mustAvoidLinkIds")]
        public List<int> MustAvoidLinkIds { get; set; }

        [JsonProperty("drivingStyle")]
        public int DrivingStyle { get; set; }

        [JsonProperty("countryBoundaryDisplay")]
        public bool CountryBoundaryDisplay { get; set; }

        [JsonProperty("generalize")]
        public int Generalize { get; set; }

        [JsonProperty("narrativeType")]
        public string NarrativeType { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("avoidTimedConditions")]
        public bool AvoidTimedConditions { get; set; }

        [JsonProperty("destinationManeuverDisplay")]
        public bool DestinationManeuverDisplay { get; set; }

        [JsonProperty("enhancedNarrative")]
        public bool EnhancedNarrative { get; set; }

        [JsonProperty("filterZoneFactor")]
        public int FilterZoneFactor { get; set; }

        [JsonProperty("timeType")]
        public int TimeType { get; set; }

        [JsonProperty("maxWalkingDistance")]
        public double MaxWalkingDistance { get; set; }

        [JsonProperty("routeType")]
        public string RouteType { get; set; }

        [JsonProperty("transferPenalty")]
        public int TransferPenalty { get; set; }

        [JsonProperty("walkingSpeed")]
        public double WalkingSpeed { get; set; }

        [JsonProperty("stateBoundaryDisplay")]
        public bool StateBoundaryDisplay { get; set; }

        [JsonProperty("maxLinkId")]
        public int MaxLinkId { get; set; }

        [JsonProperty("arteryWeights")]
        public List<int> ArteryWeights { get; set; }

        [JsonProperty("tryAvoidLinkIds")]
        public List<int> TryAvoidLinkIds { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("routeNumber")]
        public int RouteNumber { get; set; }

        [JsonProperty("doReverseGeocode")]
        public bool DoReverseGeocode { get; set; }

        [JsonProperty("shapeFormat")]
        public string ShapeFormat { get; set; }

        [JsonProperty("maneuverPenalty")]
        public int ManeuverPenalty { get; set; }

        [JsonProperty("useTraffic")]
        public bool UseTraffic { get; set; }

        [JsonProperty("returnLinkDirections")]
        public bool ReturnLinkDirections { get; set; }

        [JsonProperty("avoidTripIds")]
        public List<int> AvoidTripIds { get; set; }

        [JsonProperty("manmaps")]
        public string Manmaps { get; set; }

        [JsonProperty("highwayEfficiency")]
        public int HighwayEfficiency { get; set; }

        [JsonProperty("sideOfStreetDisplay")]
        public bool SideOfStreetDisplay { get; set; }

        [JsonProperty("cyclingRoadFactor")]
        public int CyclingRoadFactor { get; set; }

        [JsonProperty("urbanAvoidFactor")]
        public int UrbanAvoidFactor { get; set; }
    }

    public class Route
    {
        [JsonProperty("hasTollRoad")]
        public bool HasTollRoad { get; set; }

        [JsonProperty("hasBridge")]
        public bool HasBridge { get; set; }

        [JsonProperty("computedWaypoints")]
        public List<object> ComputedWaypoints { get; set; }

        [JsonProperty("fuelUsed")]
        public int FuelUsed { get; set; }

        [JsonProperty("shape")]
        public Shape Shape { get; set; }

        [JsonProperty("hasTunnel")]
        public bool HasTunnel { get; set; }

        [JsonProperty("hasUnpaved")]
        public bool HasUnpaved { get; set; }

        [JsonProperty("hasHighway")]
        public bool HasHighway { get; set; }

        [JsonProperty("realTime")]
        public int RealTime { get; set; }

        [JsonProperty("boundingBox")]
        public BoundingBox BoundingBox { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("time")]
        public int Time { get; set; }

        [JsonProperty("locationSequence")]
        public List<int> LocationSequence { get; set; }

        [JsonProperty("hasSeasonalClosure")]
        public bool HasSeasonalClosure { get; set; }

        [JsonProperty("sessionId")]
        public string SessionId { get; set; }

        [JsonProperty("locations")]
        public List<Location> Locations { get; set; }

        [JsonProperty("hasCountryCross")]
        public bool HasCountryCross { get; set; }

        [JsonProperty("legs")]
        public List<Leg> Legs { get; set; }

        [JsonProperty("formattedTime")]
        public string FormattedTime { get; set; }

        [JsonProperty("routeError")]
        public RouteError RouteError { get; set; }

        [JsonProperty("options")]
        public Options Options { get; set; }

        [JsonProperty("hasFerry")]
        public bool HasFerry { get; set; }
    }

    public class Copyright
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("imageAltText")]
        public string ImageAltText { get; set; }
    }

    public class Info
    {
        [JsonProperty("copyright")]
        public Copyright Copyright { get; set; }

        [JsonProperty("statuscode")]
        public int Statuscode { get; set; }

        [JsonProperty("messages")]
        public List<string> Messages { get; set; }
    }

    public class OpenDirections
    {
        [JsonProperty("route")]
        public Route Route { get; set; }

        [JsonProperty("info")]
        public Info Info { get; set; }
    }
}