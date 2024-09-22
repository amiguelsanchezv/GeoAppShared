using Geo.Model;
using MediatR;

namespace Geo.Application
{
    public class CreateGeoreferencePointHandler(IGeoreferencePointRepository georeferencePointRepository) : IRequestHandler<CreateGeoreferencePoint, GeoreferencePoint>
    {
        private readonly IGeoreferencePointRepository _georeferencePointRepository = georeferencePointRepository;

        public async Task<GeoreferencePoint> Handle(CreateGeoreferencePoint request, CancellationToken cancellationToken)
        {
            return await _georeferencePointRepository.AddGeoreferencePoint(new GeoreferencePoint()
            {
                Tipo = request.Tipo,
                Location = request.Location,
                References = request.References,
                Manageable = request.Manageable,
                RelatedCustomers = request.RelatedCustomers
            });
        }
    }
}
