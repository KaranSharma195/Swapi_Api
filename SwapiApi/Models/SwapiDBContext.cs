using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace SwapiApi.Models
{
    public class SwapiDBContext : DbContext
    {
        public SwapiDBContext(DbContextOptions<SwapiDBContext> option) : base(option)
        {
        }

        
        public virtual DbSet<Film> Films { get; set; }
        public virtual DbSet<Peoples> Peoples { get; set; }
        public virtual DbSet<Planets> Planets { get; set; }
        public virtual DbSet<Species> Speciess { get; set; }
        public virtual DbSet<Starships> Starships { get; set; }
        public virtual DbSet<Vehicles> Vehicles { get; set; }
      

    }
}
