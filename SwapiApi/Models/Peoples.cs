namespace SwapiApi.Models
{
    public class Peoples : IEntityWithName
    {
       
            public string Name { get; set; }
            public int Height { get; set; }
            public int Mass { get; set; }
            public string HairColor { get; set; }
            public string SkinColor { get; set; }
            public string EyeColor { get; set; }
            public string BirthYear { get; set; }
            public string Gender { get; set; }
            public string HomeworldUrl { get; set; } // Assuming it's the URL of the homeworld

            public List<string> Films { get; set; } = new List<string>();
            public List<string> Species { get; set; } = new List<string>();
            public List<string> Vehicles { get; set; } = new List<string>();
            public List<string> Starships { get; set; } = new List<string>();

            public DateTime Created { get; set; }
            public DateTime Edited { get; set; }
            public string Url { get; set; }
       
    }
}
