using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RouteGenius.Models;

namespace RouteGenius.Controllers
{
    [Route("api/[controller]")]
    public class RouteController : Controller
    {
        private static readonly Random Random = new Random();
        
        private static readonly object SyncLock = new object();
        
        private const string MapQuestApiKey = "zdcAhYGOGYS6zamXvL11tC2ClV083I2z";
        
        private static readonly HttpClient HttpClient = new HttpClient();

        [HttpPost]
        public JsonResult Post()
        {
            var postData = new StreamReader(Request.Body).ReadToEnd();

            try
            {
                var parameters = JsonConvert.DeserializeObject<RequestParameters>(postData);
                
                var results = GetExactNumberOfUniqueOpenDirections(parameters);
            
                var response = new List<Result>();

                foreach (var result in results)
                {
                    var cleaned = CleanDuplicates(
                        CleanTails(result));
                
                    var distance = CalculateTotalDistance(cleaned);
                
                    var thumbnail = GetStaticMapImage(cleaned);
                
                    response.Add(new Result
                    {
                        Coordinates = cleaned,
                        Distance = distance,
                        Thumbnail = thumbnail
                    });
                }
            
                return Json(response);
            }
            catch
            {
                return Json(new object());
            }
        }

        [HttpGet]
        public JsonResult Get()
        {
            var parameters = new RequestParameters
            {
                StartLocation = new Coordinates
                {
                    Lat = 54.69422,
                    Lng = 25.28386
                },
                LengthInMeters = 10000,
                TravelHeading = 0,
                TravelDirection = 0,
                Unit = "k",
                RouteType = "fastest",
                Locale = "en_US",
                Avoids = new List<string>
                {
                    "Ferry", "Toll Road", "Country Border Crossing"
                },
                CyclingRoadFactor = 1.0,
                RoadGradeStrategy = "DEFAULT_STRATEGY",
                DrivingStyle = 2,
                NumberOfItems = 25
            };

            var results = GetExactNumberOfUniqueOpenDirections(parameters);
            
            var response = new List<Result>();

            foreach (var result in results)
            {
                var cleaned = CleanDuplicates(
                    CleanTails(result));
                
                var distance = CalculateTotalDistance(cleaned);
                
                var thumbnail = GetStaticMapImage(cleaned);
                
                response.Add(new Result
                {
                    Coordinates = cleaned,
                    Distance = distance,
                    Thumbnail = thumbnail
                });
            }
            
            return Json(response);
        }

        private static IEnumerable<OpenDirections> GetExactNumberOfUniqueOpenDirections(RequestParameters parameters)
        {
            var openDirections = new List<OpenDirections>();
            
            while (openDirections.Count < parameters.NumberOfItems)
            {
                var receivedOnes = GetExactNumberOfOpenDirections(parameters, (parameters.NumberOfItems - openDirections.Count));

                foreach (var receivedOne in receivedOnes)
                {
                    // TODO: check if not an error, because it might be dangerous if request is too complex
                    
                    if (receivedOne.Route != null && receivedOne.Info.Statuscode == 0)
                    {
                        var unique = true;
                        
                        foreach (var openDirection in openDirections)
                        {
                            if (openDirection.Route.Shape.ShapePoints.All(receivedOne.Route.Shape.ShapePoints.Contains))
                            {
                                unique = false;
                            }
                        }

                        if (unique)
                        {
                            openDirections.Add(receivedOne);
                        }
                    }
                }
            }

            return openDirections;
        }

        private static IEnumerable<OpenDirections> GetExactNumberOfOpenDirections(RequestParameters parameters, int number)
        {
            var circles = new List<IEnumerable<Coordinates>>();
            
            for (var i = 0; i < number; i++)
            {
                var circle = GenerateRandomCirclePoints(parameters);
                circles.Add(circle);
            }
            
            var routes = Task.WhenAll(circles.Select(circle => GetRouteDirections(circle, parameters)).ToList()).Result;

            var openDirections = new List<OpenDirections>();
            
            foreach (var route in routes)
            {
                openDirections.Add(route);
            }

            return openDirections;
        }
        
        private static async Task<OpenDirections> GetRouteDirections(IEnumerable<Coordinates> circle, RequestParameters parameters)
        {
            var locations = new List<RequestLocation>
            {
                new RequestLocation
                {
                    LatLng = new Coordinates
                    {
                        Lat = parameters.StartLocation.Lat,
                        Lng = parameters.StartLocation.Lng
                    }
                }
            };


            locations.AddRange(circle.Select(point => new RequestLocation
            {
                LatLng = new Coordinates
                {
                    Lat = point.Lat,
                    Lng = point.Lng
                }
            }));

            locations.Add(new RequestLocation
            {
                LatLng = new Coordinates
                {
                    Lat = parameters.StartLocation.Lat,
                    Lng = parameters.StartLocation.Lng
                }
            });

            // https://developer.mapquest.com/documentation/open/directions-api/route/post/
            
            var json = JsonConvert.SerializeObject(new DirectionsRequest
            {
                Locations = locations,
                Options = new RequestOptions
                {
                    Unit = parameters.Unit,
                    RouteType = parameters.RouteType,
                    Locale = parameters.Locale,
                    Avoids = parameters.Avoids,
                    CyclingRoadFactor = parameters.CyclingRoadFactor,
                    RoadGradeStrategy = parameters.RoadGradeStrategy,
                    DrivingStyle = parameters.DrivingStyle,
                    NarrativeType = "text",
                    Generalize = 0,
                    ShapeFormat = "raw"
                }
            });

            const string mapQuestDirectionsUrl = "http://open.mapquestapi.com/directions/v2/route?key=" + MapQuestApiKey;
            
            var response = await HttpClient.PostAsync(mapQuestDirectionsUrl, new StringContent(json));

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<OpenDirections>(responseString);
            }

            return new OpenDirections();
        }
        
        private static IEnumerable<Coordinates> GenerateRandomCirclePoints(RequestParameters parameters)
        {
            var radius = GetPlusMinusTenPercentRouteLenght(parameters.LengthInMeters) / 2.0 / Math.PI;

            var circlePoints = Math.Round(GetRandomNumber(4.0, 12.0));

            var degrees = new List<double>();

            var direction = GetDirectionRadiansByTravelHeading(parameters.TravelHeading);

            var dx = radius * Math.Cos(direction);
            var dy = radius * Math.Sin(direction);
            
            var deltaLat = dy / 110540.0;
            var deltaLon = dx / (111320.0 * Math.Cos(parameters.StartLocation.Lat * Math.PI / 180.0));
            
            var center = new Coordinates
            {
                Lat = parameters.StartLocation.Lat + deltaLat,
                Lng = parameters.StartLocation.Lng + deltaLon
            };
            
            degrees.Add(direction + Math.PI);

            var sign = GetSignByTravelDirection(parameters.TravelDirection);
            
            var circlePointsCollection = new List<Coordinates>();

            for (var i = 1; i < circlePoints + 1; i++)
            {
                degrees.Add(degrees[i - 1] + sign * 2.0 * Math.PI / (circlePoints + 1));

                dx = radius * Math.Cos(degrees[i]);
                dy = radius * Math.Sin(degrees[i]);
                
                deltaLat = dy / 110540.0;
                deltaLon = dx / (111320.0 * Math.Cos(center.Lat * Math.PI / 180.0));
                
                circlePointsCollection.Add(new Coordinates
                {
                    Lat = center.Lat + deltaLat,
                    Lng = center.Lng + deltaLon
                });
            }
            
            return circlePointsCollection;
        }
        
        private static double GetSignByTravelDirection(int travelDirection)
        {
            if (travelDirection == 0)
            {
                return -1.0;
            }

            return 1.0;
        }
        
        private static double GetDirectionRadiansByTravelHeading(int travelHeading)
        {
            switch (travelHeading)
            {
                case 0:
                    return GetRandomNumber(0, 1.0) * 2.0 * Math.PI;
                case 1:
                    return GetRandomNumber(0, 1.0) * Math.PI / 4.0 + 3.0 + Math.PI / 8.0;
                case 2:
                    return GetRandomNumber(0, 1.0) * Math.PI / 4.0 + 1.0 * Math.PI / 8.0;
                case 3:
                    return GetRandomNumber(0, 1.0) * Math.PI / 4.0 - Math.PI / 8.0;
                case 4:
                    return GetRandomNumber(0, 1.0) * Math.PI / 4.0 + 13.0 * Math.PI / 8.0;
                case 5:
                    return GetRandomNumber(0, 1.0) * Math.PI / 4.0 + 11.0 * Math.PI / 8.0;
                case 6:
                    return GetRandomNumber(0, 1.0) * Math.PI / 4.0 + 9.0 * Math.PI / 8.0;
                case 7:
                    return GetRandomNumber(0, 1.0) * Math.PI / 4.0 + 7.0 * Math.PI / 8.0;
                case 8:
                    return GetRandomNumber(0, 1.0) * Math.PI / 4.0 + 5.0 * Math.PI / 8.0;
                default:
                    return GetRandomNumber(0, 1.0) * 2.0 * Math.PI;
            }
        }
        
        private static double GetPlusMinusTenPercentRouteLenght(int lengthInMeters)
        {
            return lengthInMeters * GetRandomNumber(0.9, 1.1);
        }
        
        private static double GetRandomNumber(double minimum, double maximum)
        {
            lock (SyncLock)
            {
                return Random.NextDouble() * (maximum - minimum) + minimum;
            }
        }

        private static List<LatLng> CleanDuplicates(IReadOnlyList<LatLng> data)
        {
            var cleaned = new List<LatLng>
            {
                data[0]
            };

            for (var i = 1; i < data.Count - 1; i++)
            {
                if (Math.Abs(data[i].Lat - data[i - 1].Lat) > float.MinValue && Math.Abs(data[i].Lng - data[i - 1].Lng) > float.MinValue)
                {
                    cleaned.Add(data[i]);
                }
            }
            
            return cleaned;
        }
        
        private static List<LatLng> CleanTails(OpenDirections route)
        {
            var routeLatLng = new List<Location>();

            for (var i = 0; i < route.Route.Shape.ShapePoints.Count; i += 2)
            {
                routeLatLng.Add(new Location
                {
                    LatLng = new LatLng
                    {
                        Lat = route.Route.Shape.ShapePoints[i],
                        Lng = route.Route.Shape.ShapePoints[i + 1]
                    }
                });
            }
            
            var pLpoints = new List<LatLng>();
            var pLdist = new List<double>();
            var pLclose = new List<int>();
            var newPath = new List<LatLng>();
            var pLuse = new List<bool>();
            
            for (var i = 0; i < routeLatLng.Count; i++)
            {
                pLpoints.Add(routeLatLng[i].LatLng);
            }
            
            pLdist.Add(0);

            var cumulative = 0.0;
            
            for (var i = 0; i < pLpoints.Count - 1; i++)
            {
                pLuse.Add(false);
                
                cumulative += LatLngDistance(pLpoints[i].Lat, pLpoints[i].Lng, pLpoints[i + 1].Lat, pLpoints[i + 1].Lng);  
                
                pLdist.Add(cumulative);
            }
            
            var point = 0;
            var closest = 0.0;

            for (var i = 0; i < pLpoints.Count; i++)
            {
                var thisOne = pLpoints[i];

                for (var j = i + 1; j < pLpoints.Count; j++)
                {
                    var thatOne = pLpoints[j];

                    var dist = LatLngDistance(thisOne.Lat, thisOne.Lng, thatOne.Lat, thatOne.Lng);

                    if (j == i + 1)
                    {
                        closest = dist;
                        point = j;
                    }
                    else
                    {
                        if (dist < closest)
                        {
                            closest = dist;
                            point = j;
                        }
                    }
                }

                pLclose.Add(point);
            }

            const double tailLimit = 0.2;

            for (var i = 0; i < pLpoints.Count; i++)
            {
                if (i > pLuse.Count - 1)
                {
                    pLuse.Add(true);
                }
                else
                {
                    pLuse[i] = true;
                }

                if (pLclose[i] - i != 1)
                {
                    var tailSize = (pLdist[pLclose[i]] - pLdist[i]) / cumulative;

                    if (tailSize < tailLimit)
                    {
                        i = pLclose[i];
                    }
                }
            }

            for (var i = 0; i < pLpoints.Count; i++)
            {
                if (pLuse[i])
                {
                    newPath.Add(pLpoints[i]);
                }
            }
            
            newPath.Add(newPath[0]);

            return newPath;
        }
        
        private static double LatLngDistance(double lat1, double lng1, double lat2, double lng2)
        {
            const double r = 6371.0;
            
            var dLat = (lat2 - lat1) * Math.PI / 180.0;
            var dLon = (lng2 - lng1) * Math.PI / 180.0;

            var a = Math.Sin(dLat / 2.0) * Math.Sin(dLat / 2.0) + Math.Cos(lat1 * Math.PI / 180.0) * Math.Cos(lat2 * Math.PI / 180.0) * Math.Sin(dLon / 2.0) * Math.Sin(dLon / 2.0);

            var c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));

            return r * c;
        }
        
        private static double CalculateTotalDistance(IReadOnlyList<LatLng> cleaned)
        {
            var total = 0.0;

            for (var i = 0; i < cleaned.Count - 1; i++)
            {
                total += LatLngDistance(cleaned[i].Lat, cleaned[i].Lng, cleaned[i + 1].Lat, cleaned[i + 1].Lng);
            }

            return Math.Round(total * 1000);
        }
        
        private static string GetStaticMapImage(IReadOnlyList<LatLng> cleaned)
        {
            var startPoint = cleaned[0].Lat + "," + cleaned[0].Lng;
            
            var mapUrl = "https://open.mapquestapi.com/staticmap/v5/map?key=" + MapQuestApiKey + "&shape=weight:2|border:ff0000|" + PointsBuilder(cleaned, "|") + "&shape=weight:2|border:ff0000|fill:ff0000|radius:0.1km|" + startPoint + "&size=400,400&type=light&margin=100";
            
            return mapUrl;
        }
        
        private static string PointsBuilder(IReadOnlyList<LatLng> cleaned, string separator)
        {
            const double pointLimit = 255.0;
            
            var thinned = new List<LatLng>
            {
                cleaned[0]
            };
            
            var ratio = pointLimit / cleaned.Count;

            if (ratio > 1.0)
            {
                ratio = 1.0;
            }
            
            var track = 0.0;
            var last = -1.0;
            
            foreach (var point in cleaned)
            {
                track += ratio;
                
                if (Math.Floor(track) > last)
                {
                    thinned.Add(point);
                    last = Math.Floor(track);
                }
            }
            
            thinned.Add(cleaned[cleaned.Count - 1]);
            
            thinned = CleanDuplicates(thinned);
            
            var pointsCollection = new List<string>();
            
            foreach (var point in thinned)
            {
                pointsCollection.Add(string.Concat(point.Lat.ToString(CultureInfo.InvariantCulture), ",", point.Lng.ToString(CultureInfo.InvariantCulture)));
            }

            return string.Join(separator, pointsCollection);
        }
    }
}