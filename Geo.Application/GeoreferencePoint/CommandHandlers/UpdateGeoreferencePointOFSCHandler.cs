using MediatR;

namespace Geo.Application
{
    public class UpdateGeoreferencePointOFSCHandler(IGeoreferencePointRepository georeferencePointRepository) : IRequestHandler<UpdateGeoreferencePointOFSC>
    {
        private readonly IGeoreferencePointRepository _georeferencePointRepository = georeferencePointRepository;

        public async Task Handle(UpdateGeoreferencePointOFSC request, CancellationToken cancellationToken)
        {
            await _georeferencePointRepository.UpdateGeoreferencePointOFSC(request.ActivityId, request.Locations, request.Location);
        }
    }
}