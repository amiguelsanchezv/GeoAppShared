using Geo.Model;

namespace Geo.Service.Entities
{
    public class GeoreferencePointAccess(string connectionString, string databaseName, string collectionName) : MongoAccess<GeoreferencePoint>(connectionString, databaseName, collectionName)
    {
    }
}
