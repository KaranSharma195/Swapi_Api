namespace SwapiApi.Models
{
    public class Species : IEntityWithName
    {
        public string Name { get; set; }
        public string Classification { get; set; }
        public string Designation { get; set; }

        public int? AverageHeight { get; set; } // Can be null if not provided

        public List<string> SkinColors { get; set; } = new List<string>();
        public List<string> HairColors { get; set; } = new List<string>();
        public List<string> EyeColors { get; set; } = new List<string>();

        public int? AverageLifespan { get; set; } // Can be null if not provided

        public string HomeworldUrl { get; set; }

        public string Language { get; set; }

        public List<string> PeopleUrls { get; set; } = new List<string>();
        public List<string> FilmsUrls { get; set; } = new List<string>();

        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }
        public string Url { get; set; }
    }

}
