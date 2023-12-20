namespace SwapiApi.Models
{
  
        public class Starships : IEntityWithName
    {
            public string Name { get; set; }
            public string Model { get; set; }
            public string Manufacturer { get; set; }
            public int CostInCredits { get; set; } // Assuming it's an integer

            public decimal Length { get; set; } // May need different parsing depending on format
            public int MaxAtmospheringSpeed { get; set; }
            public int Crew { get; set; }
            public int Passengers { get; set; }
            public int CargoCapacity { get; set; }
            public string Consumables { get; set; }
            public decimal HyperdriveRating { get; set; } // May need different parsing depending on format
            public int MGLT { get; set; }
            public string StarshipClass { get; set; }

            public List<string> Pilots { get; set; } = new List<string>();
            public List<string> Films { get; set; } = new List<string>();

            public DateTime Created { get; set; }
            public DateTime Edited { get; set; }
            public string Url { get; set; }
        

    }
}
