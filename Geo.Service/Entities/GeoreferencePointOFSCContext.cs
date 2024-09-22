using Geo.Service.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo.Service.Entities
{
    public class GeoreferencePointOFSCContext(string instance, string clientId, string clientSecret) : OFSCAccess(instance, clientId, clientSecret)
    {
    }
}
