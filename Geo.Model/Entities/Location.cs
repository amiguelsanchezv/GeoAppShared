using MongoDB.Driver.GeoJsonObjectModel;
using System.Text.Json.Serialization;

namespace Geo.Model
{
    public class Location(double longitude, double latitude)
    {
        public double Longitude { get; set; } = longitude;
        public double Latitude { get; set; } = latitude;

        public double CalculateDistance(Location location)
        {
            var distanceX1 = this.Latitude * (Math.PI / 180.0);
            var distanceY1 = this.Longitude * (Math.PI / 180.0);
            var distanceX2 = location.Latitude * (Math.PI / 180.0);
            var distanceY2 = location.Longitude * (Math.PI / 180.0) - distanceY1;
            var distance3 = Math.Pow(Math.Sin((distanceX2 - distanceX1) / 2.0), 2.0) +
                Math.Cos(distanceX1) * Math.Cos(distanceX2) * Math.Pow(Math.Sin(distanceY2 / 2.0), 2.0);
            var distanceResult = 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(distance3), Math.Sqrt(1.0 - distance3)));
            return distanceResult;
        }
        public static Location GetLocation(GeoJsonPoint<GeoJson2DCoordinates> location)
        {
            return new Location(location.Coordinates.Y, location.Coordinates.X);
        }
        public static List<Location> GetListLocation(List<GeoreferencePoint> georeferencePoints)
        {
            List<Location> locations = [];
            foreach (var item in georeferencePoints)
            {
                locations.Add(Location.GetLocation(item.Location));
            }
            return locations;
        }
    }
    public class LocationOFSC
    {
        [JsonPropertyName("location_1")]
        public string? Location1 { get; set; }
        [JsonPropertyName("location_2")]
        public string? Location2 { get; set; }
        [JsonPropertyName("location_3")]
        public string? Location3 { get; set; }
        [JsonPropertyName("issue_route")]
        public string? Issue_Rate { get; set; }
        public static string GetLocationOFSC(GeoJsonPoint<GeoJson2DCoordinates> geoLocation)
        {
            var location = Location.GetLocation(geoLocation);
            return String.Format("lat:{0},lng:{1}", location.Latitude.ToString().Replace(",", "."), location.Longitude.ToString().Replace(",", "."));
        }
        public static string GetLocationGoogleMaps(GeoJsonPoint<GeoJson2DCoordinates> geoLocation)
        {
            var location = Location.GetLocation(geoLocation);
            return String.Format("{0},{1}", location.Latitude.ToString().Replace(",", "."), location.Longitude.ToString().Replace(",", "."));
        }
        public static string GetLocationGoogleMaps(Location location)
        {
            return String.Format("{0},{1}", location.Latitude.ToString().Replace(",", "."), location.Longitude.ToString().Replace(",", "."));
        }
        public static string GetLocationTagsGoogleMaps(int index)
        {
            return String.Format("{0}", ("Camara_" + index));
        }
    }
}
