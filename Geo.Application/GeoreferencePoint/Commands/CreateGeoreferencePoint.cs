using Geo.Model;
using MediatR;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Geo.Application
{
    public class CreateGeoreferencePoint : IRequest<GeoreferencePoint>
    {
        public required string Tipo { get; set; }
        public bool Manageable { get; set; }
        public required GeoJsonPoint<GeoJson2DCoordinates> Location { get; set; }
        public Reference[]? References { get; set; }

        public Customer[]? RelatedCustomers { get; set; }
    }
}
