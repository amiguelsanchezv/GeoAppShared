using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Geo.Model
{
    public class GeoreferencePoint : IEntity
    {
        public ObjectId Id { get; set; }
        public required string Tipo { get; set; }
        public bool Manageable { get; set; }
        public required GeoJsonPoint<GeoJson2DCoordinates> Location { get; set; }
        public double? Distance { get; set; }
        public Reference[]? References { get; set; }
        public Customer[]? RelatedCustomers { get; set; }

        public GeoreferencePoint GetReferencePoint(GeoJsonPoint<GeoJson2DCoordinates> coordinate)
        {
            this.Distance = (Model.Location.GetLocation(coordinate)).CalculateDistance(Model.Location.GetLocation(this.Location));
            return this;
        }
        public static List<GeoreferencePoint> GetLocationOrder(List<GeoreferencePoint> georeferencePoints, GeoJsonPoint<GeoJson2DCoordinates> coordinate)
        {
            var georeferences = new List<GeoreferencePoint>();
            foreach (var g in georeferencePoints)
            {
                georeferences.Add(g.GetReferencePoint(coordinate));
            }
            return [.. georeferences.OrderBy(g => g.Distance)];
        }
    }

    public class GeoreferencePointApi
    {
        public required string Tipo { get; set; }
        public bool Manageable { get; set; }
        public required Location Location { get; set; }
        public Reference[]? References { get; set; }
        public CustomerApi[]? RelatedCustomers { get; set; }

        public GeoreferencePoint GetGeoreferencePoint()
        {
            return new GeoreferencePoint()
            {
                Tipo = this.Tipo,
                Location = new GeoJsonPoint<GeoJson2DCoordinates>(new GeoJson2DCoordinates(this.Location.Latitude, this.Location.Longitude)),
                Manageable = this.Manageable,
                References = this.References,
                RelatedCustomers = CustomerApi.GetCustomers(this.RelatedCustomers ?? [])
            };
        }
    }

    public class GeoreferencePointRatio
    {
        public required Location Location { get; set; }
        public required double Meters { get; set; }
        public int? ActivityId { get; set; }
        public GeoJsonPoint<GeoJson2DCoordinates> GetCoordinates()
        {
            return new GeoJsonPoint<GeoJson2DCoordinates>(new GeoJson2DCoordinates(this.Location.Latitude, this.Location.Longitude));
        }
    }

    public class GeoreferencePointTwoRatio
    {
        public required Location Location { get; set; }
        public required double InnerRadius { get; set; }
        public required double OuterRadius { get; set; }
        public int? ActivityId { get; set; }
        public GeoJsonPoint<GeoJson2DCoordinates> GetCoordinates()
        {
            return new GeoJsonPoint<GeoJson2DCoordinates>(new GeoJson2DCoordinates(this.Location.Latitude, this.Location.Longitude));
        }
    }
}
