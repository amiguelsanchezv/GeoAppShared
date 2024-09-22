using Geo.Model;
using MediatR;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Geo.Application
{
    public interface IOperations
    {
        Task<ICollection<GeoreferencePoint>> GetAll(IMediator mediator);
        Task<ICollection<GeoreferencePoint>> GetByRatio(IMediator mediator, GeoJsonPoint<GeoJson2DCoordinates> coordinate, double meters);
        Task<ICollection<GeoreferencePoint>> GetByTwoRatio(IMediator mediator, GeoJsonPoint<GeoJson2DCoordinates> coordinate, double innerRadius, double outerRadius);
        Task<GeoreferencePoint> AddGeoreferencePoint(IMediator mediator, GeoreferencePoint georeferencePoint);
        Task UpdateActivityLocations(IMediator mediator, int activityId, List<GeoreferencePoint> locations, Location location);
    }
}
