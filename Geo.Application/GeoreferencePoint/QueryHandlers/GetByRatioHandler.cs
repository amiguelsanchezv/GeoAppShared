using Geo.Model;
using MediatR;

namespace Geo.Application
{
    public class GetByRatioHandler(IGeoreferencePointRepository georeferencePointRepository) : IRequestHandler<GetByRatio, ICollection<GeoreferencePoint>>
    {
        private readonly IGeoreferencePointRepository _georeferencePointRepository = georeferencePointRepository;

        public async Task<ICollection<GeoreferencePoint>> Handle(GetByRatio request, CancellationToken cancellationToken)
        {
            return await _georeferencePointRepository.GetByRatio(request.Coordinate, request.Meters);
        }
    }
}
