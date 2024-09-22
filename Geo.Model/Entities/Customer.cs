using MongoDB.Driver.GeoJsonObjectModel;

namespace Geo.Model
{
    public class Customer
    {
        public required string IdCustomer { get; set; }
        public required string IdService { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required GeoJsonPoint<GeoJson2DCoordinates> Location { get; set; }
    }
    public class CustomerApi
    {
        public required string IdCustomer { get; set; }
        public required string IdService { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required Location Location { get; set; }

        public static Customer[] GetCustomers(CustomerApi[] customersApi)
        {
            List<Customer> customers = [];
            foreach (var c in customersApi)
            {
                customers.Add(new Customer()
                {
                    Address = c.Address,
                    IdCustomer = c.IdCustomer,
                    IdService = c.IdService,
                    Name = c.Name,
                    Location = new GeoJsonPoint<GeoJson2DCoordinates>(new GeoJson2DCoordinates(c.Location.Latitude, c.Location.Longitude))
                });
            }
            return [.. customers];
        }
    }
}
