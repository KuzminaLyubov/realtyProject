
using System.Data.Entity;

namespace RealtyApp.Models
{
    public class RealtyContext : DbContext
    {
        public RealtyContext() : base("DefaultConnection")
        {

        }
        public DbSet<RealEstate> Realty { get; set; }
    }
}