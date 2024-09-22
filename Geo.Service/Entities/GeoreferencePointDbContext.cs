using Geo.Model;
using Microsoft.EntityFrameworkCore;

namespace Geo.Service
{
    public class GeoreferencePointDbContext(DbContextOptions<GeoreferencePointDbContext> options) : DbContext(options)
    {
        public DbSet<GeoreferencePoint> GeoreferencePoint { get; set; }
    }
}
