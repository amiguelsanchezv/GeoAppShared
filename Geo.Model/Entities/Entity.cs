using MongoDB.Bson;

namespace Geo.Model
{
    public interface IEntity
    {
        ObjectId Id { get; set; }
    }
}
