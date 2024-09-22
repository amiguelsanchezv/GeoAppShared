using Geo.Model;
using MediatR;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Geo.Application
{
    public class GetByRatio : IRequest<ICollection<GeoreferencePoint>>
    {
        public required GeoJsonPoint<GeoJson2DCoordinates> Coordinate { get; set; }
        public double Meters { get; set; }
    }
}
