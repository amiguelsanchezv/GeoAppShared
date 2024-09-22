using Geo.Model;
using MediatR;

namespace Geo.Application
{
    public class GetAllGeoreferenceHandler(IGeoreferencePointRepository georeferencePointRepository) : IRequestHandler<GetAllGeoreferencePoint, ICollection<GeoreferencePoint>>
    {
        private readonly IGeoreferencePointRepository _georeferencePointRepository = georeferencePointRepository;

        public async Task<ICollection<GeoreferencePoint>> Handle(GetAllGeoreferencePoint request, CancellationToken cancellationToken)
        {
            return await _georeferencePointRepository.GetAll();
        }
    }
}
