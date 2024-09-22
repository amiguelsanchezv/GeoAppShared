using Geo.Model;
using MediatR;

namespace Geo.Application
{
    public class UpdateGeoreferencePointOFSC : IRequest
    {
        public required int ActivityId { get; set; }

        public required List<GeoreferencePoint> Locations { get; set; }
        public required Location Location { get; set; }
    }
}
