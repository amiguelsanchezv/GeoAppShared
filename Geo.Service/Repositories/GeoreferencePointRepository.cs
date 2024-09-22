using Geo.Application;
using Geo.Model;
using Geo.Service.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using System.Net.Http.Headers;
using System.Text;

namespace Geo.Service
{
    public class GeoreferencePointRepository(GeoreferencePointAccess access, GeoreferencePointOFSCContext ofscAccess) : IGeoreferencePointRepository
    {
        private readonly GeoreferencePointAccess _access = access;
        private readonly GeoreferencePointOFSCContext _ofscAccess = ofscAccess;

        public async Task<GeoreferencePoint> AddGeoreferencePoint(GeoreferencePoint georeferencePoint)
        {
            return await _access.Add(georeferencePoint);
        }

        public async Task<ICollection<GeoreferencePoint>> GetAll()
        {
            return await _access.All();
        }

        public async Task<ICollection<GeoreferencePoint>> GetByRatio(GeoJsonPoint<GeoJson2DCoordinates> coordinate, double meters)
        {
            var filter = Builders<GeoreferencePoint>.Filter.NearSphere(g => g.Location, coordinate, meters);
            return GeoreferencePoint.GetLocationOrder(await _access.CurrentMongoColletion.Find(filter).ToListAsync(), coordinate);
        }

        public async Task<ICollection<GeoreferencePoint>> GetByTwoRatio(GeoJsonPoint<GeoJson2DCoordinates> coordinate, double innerRadius, double outerRadius)
        {
            var filter = Builders<GeoreferencePoint>.Filter.NearSphere(g => g.Location, coordinate, outerRadius, innerRadius);
            return GeoreferencePoint.GetLocationOrder(await _access.CurrentMongoColletion.Find(filter).ToListAsync(), coordinate);
        }

        public async Task UpdateGeoreferencePointOFSC(int activityId, List<GeoreferencePoint> locations, Location location)
        {
            var googleMapsUrl = "https://www.google.com/maps/dir/?api=1&origin={0}&destination={0}&waypoints={1}&travelmode=walking";
            var locationRest = new LocationOFSC();
            int index = 0;

            var wayPoints = new List<string>();
            foreach (var l in locations)
            {
                wayPoints.Add(LocationOFSC.GetLocationGoogleMaps(l.Location));
            }
            locationRest.Issue_Rate = string.Format(googleMapsUrl, LocationOFSC.GetLocationGoogleMaps(location), string.Join("|", wayPoints));

            foreach (var l in locations)
            {
                switch (index)
                {
                    case 0:
                        locationRest.Location1 = LocationOFSC.GetLocationOFSC(l.Location);
                        index++;
                        break;
                    case 1:
                        locationRest.Location2 = LocationOFSC.GetLocationOFSC(l.Location);
                        index++;
                        break;
                    case 2:
                        locationRest.Location3 = LocationOFSC.GetLocationOFSC(l.Location);
                        index++;
                        break;
                    default:
                        index++;
                        break;
                }
            }

            var stringJson = System.Text.Json.JsonSerializer.Serialize(locationRest);
            await _ofscAccess.UpdateActivity(activityId, stringJson);
        }
    }
}
