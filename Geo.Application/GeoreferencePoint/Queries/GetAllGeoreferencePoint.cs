using Geo.Model;
using MediatR;

namespace Geo.Application
{
    public class GetAllGeoreferencePoint : IRequest<ICollection<GeoreferencePoint>>
    {
    }
}
