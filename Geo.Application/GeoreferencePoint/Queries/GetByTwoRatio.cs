using Geo.Model;
using MediatR;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Geo.Application
{
    public class GetByTwoRatio : IRequest<ICollection<GeoreferencePoint>>
    {
        public required GeoJsonPoint<GeoJson2DCoordinates> Coordinate { get; set; }
        public required double InnerRadius { get; set; }
        public required double OuterRadius { get; set; }
    }
}
