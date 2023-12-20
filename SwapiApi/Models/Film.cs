namespace SwapiApi.Models
{
    public class Film
    {
        public string Title { get; set; }
        public int EpisodeId { get; set; }
        public string OpeningCrawl { get; set; }
        public string Director { get; set; }
        public string Producer { get; set; }

        public DateTime? ReleaseDate { get; set; } // Nullable DateTime

        public List<string> CharacterUrls { get; set; } = new List<string>();
        public List<string> PlanetUrls { get; set; } = new List<string>();
        public List<string> StarshipUrls { get; set; } = new List<string>();
        public List<string> VehicleUrls { get; set; } = new List<string>();
        public List<string> SpeciesUrls { get; set; } = new List<string>();

        public DateTime? Created { get; set; } // Nullable DateTime
        public DateTime? Edited { get; set; } // Nullable DateTime
        public string Url { get; set; }

        public List<Peoples> Characters { get; set; } = new List<Peoples>();
        public List<Planets> Planets { get; set; } = new List<Planets>();
        public List<Starships> Starships { get; set; } = new List<Starships>();
        public List<Vehicles> Vehicles { get; set; } = new List<Vehicles>();
        public List<Species> Species { get; set; } = new List<Species>();
    }
}
