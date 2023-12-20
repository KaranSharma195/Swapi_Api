namespace SwapiApi.Models
{
    public class Vehicles : IEntityWithName
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }

        public string CostInCredits { get; set; } // "unknown" needs special handling

        public decimal Length { get; set; } // Same as before, parse based on specific format
        public int MaxAtmospheringSpeed { get; set; }
        public int Crew { get; set; }
        public int Passengers { get; set; }
        public int CargoCapacity { get; set; }
        public string Consumables { get; set; }

        public string VehicleClass { get; set; }

        public List<string> Pilots { get; set; } = new List<string>();
        public List<string> Films { get; set; } = new List<string>();

        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }
        public string Url { get; set; }
    }

}
