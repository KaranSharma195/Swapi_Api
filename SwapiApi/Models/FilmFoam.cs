namespace SwapiApi.Models
{
    public class FilmFoam
    {
        public string Title { get; set; }
        public int Episode_id { get; set; }
        public string OpeningCrawl { get; set; }
        public string Director { get; set; }
        public string Producer { get; set; }

        public DateTime? ReleaseDate { get; set; } // Nullable DateTime

        public List<string> Characters { get; set; } = new List<string>();
        public List<string> Planets { get; set; } = new List<string>();
        public List<string> Starships { get; set; } = new List<string>();
        public List<string> Vehicles { get; set; } = new List<string>();
        public List<string> Species { get; set; } = new List<string>();

        public DateTime? Created { get; set; } // Nullable DateTime
        public DateTime? Edited { get; set; } // Nullable DateTime
        public string Url { get; set; }
    }
}
