using Geo.Model;
using MediatR;

namespace Geo.Application
{
    public class GetByTwoRatioHandler(IGeoreferencePointRepository georeferencePointRepository) : IRequestHandler<GetByTwoRatio, ICollection<GeoreferencePoint>>
    {
        private readonly IGeoreferencePointRepository _georeferencePointRepository = georeferencePointRepository;

        public async Task<ICollection<GeoreferencePoint>> Handle(GetByTwoRatio request, CancellationToken cancellationToken)
        {
            return await _georeferencePointRepository.GetByTwoRatio(request.Coordinate, request.InnerRadius, request.OuterRadius);
        }
    }
}
