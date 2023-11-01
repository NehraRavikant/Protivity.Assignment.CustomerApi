using Microsoft.EntityFrameworkCore;


namespace Protivity.Assignment.CustomerApi.Repository.DataModel
{
    public class CustomerContext: DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        {
                
        }

        public DbSet<Customer> Customers { get; set; } = null!;
    }
}
