namespace SwapiApi.Models
{
    public class Planets : IEntityWithName
    {
        public string Name { get; set; }

        public int RotationPeriod { get; set; }
        public int OrbitalPeriod { get; set; }
        public int Diameter { get; set; }

        public string Climate { get; set; }
        public string Gravity { get; set; }

        public List<string> Terrain { get; set; } = new List<string>();

        public int SurfaceWaterPercentage { get; set; } // Assuming "surface_water" is a percentage

        public long Population { get; set; } // Assuming "population" is a long integer

        public List<string> ResidentUrls { get; set; } = new List<string>();
        public List<string> FilmUrls { get; set; } = new List<string>();

        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }
        public string Url { get; set; }
    }

}
