using Geo.Application;
using Geo.Model;
using MediatR;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Geo.Service
{
    public class Operations : IOperations
    {
        public async Task<GeoreferencePoint> AddGeoreferencePoint(IMediator mediator, GeoreferencePoint georeferencePoint)
        {
            return await mediator.Send(new CreateGeoreferencePoint
            {
                Location = georeferencePoint.Location,
                Manageable = georeferencePoint.Manageable,
                References = georeferencePoint.References,
                RelatedCustomers = georeferencePoint.RelatedCustomers,
                Tipo = georeferencePoint.Tipo
            });
        }

        public async Task<ICollection<GeoreferencePoint>> GetAll(IMediator mediator)
        {
            return await mediator.Send(new GetAllGeoreferencePoint());
        }

        public async Task<ICollection<GeoreferencePoint>> GetByRatio(IMediator mediator, GeoJsonPoint<GeoJson2DCoordinates> coordinate, double meters)
        {
            return await mediator.Send(new GetByRatio() { Coordinate = coordinate, Meters = meters });
        }

        public async Task<ICollection<GeoreferencePoint>> GetByTwoRatio(IMediator mediator, GeoJsonPoint<GeoJson2DCoordinates> coordinate, double innerRadius, double outerRadius)
        {
            return await mediator.Send(new GetByTwoRatio() { Coordinate = coordinate, InnerRadius = innerRadius, OuterRadius = outerRadius });
        }

        public async Task UpdateActivityLocations(IMediator mediator, int activityId, List<GeoreferencePoint> locations, Location location)
        {
            await mediator.Send(new UpdateGeoreferencePointOFSC() { ActivityId = activityId, Locations = locations, Location = location });
        }
    }
}
