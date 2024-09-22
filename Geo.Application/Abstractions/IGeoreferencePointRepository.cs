using Geo.Model;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Geo.Application
{
    public interface IGeoreferencePointRepository
    {
        Task<ICollection<GeoreferencePoint>> GetAll();
        Task<ICollection<GeoreferencePoint>> GetByRatio(GeoJsonPoint<GeoJson2DCoordinates> coordinate, double meters);
        Task<ICollection<GeoreferencePoint>> GetByTwoRatio(GeoJsonPoint<GeoJson2DCoordinates> coordinate, double innerRadius, double outerRadius);
        Task<GeoreferencePoint> AddGeoreferencePoint(GeoreferencePoint georeferencePoint);
        Task UpdateGeoreferencePointOFSC(int activityId, List<GeoreferencePoint> locations, Location location);
    }
}
