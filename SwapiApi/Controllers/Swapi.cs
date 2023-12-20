//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using SwapiApi.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace SwapiApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class Swapi : ControllerBase
//    {
//        [HttpGet]
//        public async Task<ActionResult<List<Film>>> SearchFilm(
//            string title = null,
//            string characters = null,
//            string director = null,
//            string producer = null,
//            string planets = null,
//            string starships = null,
//            string vehicles = null,
//            string species = null)
//        {
//            try
//            {
//                List<Film> filmsList = new List<Film>();
//                string apiBasicUri = "https://swapi.dev/api/films/";

//                // Build the query parameters based on the provided search criteria
//                var queryParams = new List<string>();

//                AddQueryParam(queryParams, "title", title);
//                AddQueryParam(queryParams, "director", director);
//                AddQueryParam(queryParams, "producer", producer);
//                AddQueryParam(queryParams, "planets", planets);
//                AddQueryParam(queryParams, "starships", starships);
//                AddQueryParam(queryParams, "vehicles", vehicles);
//                AddQueryParam(queryParams, "species", species);

//                string apiUrlWithQuery = $"{apiBasicUri}?{string.Join("&", queryParams)}";

//                // Log the constructed API URL for debugging
//                Console.WriteLine($"Constructed API URL: {apiUrlWithQuery}");

//                using (var client = new HttpClient())
//                {
//                    var result = await client.GetAsync(apiUrlWithQuery);
//                    result.EnsureSuccessStatusCode();
//                    string resultContentString = await result.Content.ReadAsStringAsync();

//                    var responseObject = JsonConvert.DeserializeObject<JObject>(resultContentString);

//                    if (responseObject.TryGetValue("results", out var filmsToken) && filmsToken is JArray filmsArray)
//                    {
//                        var filmFoams = filmsArray.ToObject<List<FilmFoam>>();

//                        var tasks = filmFoams.Select(async filmFoam =>
//                        {
//                            Console.WriteLine($"Processing film: {filmFoam.Title}");

//                            if (await IsFilmMatch(client, filmFoam, title, characters, director, producer, planets, starships, vehicles, species))
//                            {
//                                Console.WriteLine($"Film {filmFoam.Title} matches the search criteria.");

//                                Film filmWithDetails = new Film
//                                {
//                                    Title = filmFoam.Title,
//                                    EpisodeId = filmFoam.Episode_id,
//                                    OpeningCrawl = filmFoam.OpeningCrawl,
//                                    Director = filmFoam.Director,
//                                    Producer = filmFoam.Producer,
//                                    ReleaseDate = filmFoam.ReleaseDate,
//                                    Created = filmFoam.Created,
//                                    Edited = filmFoam.Edited,
//                                    Url = filmFoam.Url,
//                                    CharacterUrls = filmFoam.Characters ?? new List<string>(),
//                                    PlanetUrls = filmFoam.Planets ?? new List<string>(),
//                                    StarshipUrls = filmFoam.Starships ?? new List<string>(),
//                                    VehicleUrls = filmFoam.Vehicles ?? new List<string>(),
//                                    SpeciesUrls = filmFoam.Species ?? new List<string>(),
//                                };

//                                // Fetch details for characters, planets, starships, vehicles, and species
//                                filmWithDetails.Characters = await GetDetailsAsync<Peoples>(client, filmFoam.Characters);
//                                filmWithDetails.Planets = await GetDetailsAsync<Planets>(client, filmFoam.Planets);
//                                filmWithDetails.Starships = await GetDetailsAsync<Starships>(client, filmFoam.Starships);
//                                filmWithDetails.Vehicles = await GetDetailsAsync<Vehicles>(client, filmFoam.Vehicles);
//                                filmWithDetails.Species = await GetDetailsAsync<Species>(client, filmFoam.Species);

//                                return filmWithDetails;
//                            }

//                            Console.WriteLine($"Film {filmFoam.Title} does not match the search criteria.");
//                            return null;
//                        });

//                        filmsList.AddRange(await Task.WhenAll(tasks.Where(t => t != null)));
//                    }
//                    else
//                    {
//                        // Log or handle unexpected API response
//                        return BadRequest("Unexpected API response format.");
//                    }
//                }

//                return filmsList;
//            }
//            catch (Exception ex)
//            {
//                // Log the exception for debugging purposes
//                Console.WriteLine($"An unexpected error occurred: {ex}");

//                // Return a more meaningful error message to the client
//                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred. Exception: {ex.Message}");
//            }
//        }

//        private async Task<bool> IsFilmMatch(HttpClient client, FilmFoam filmFoam, string title, string characters, string director, string producer, string planets, string starships, string vehicles, string species)
//        {
//            return (string.IsNullOrEmpty(title) || filmFoam.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
//                && (string.IsNullOrEmpty(characters) || await EntityNameExistsAsync<Peoples>(client, filmFoam.Characters, characters))
//                && (string.IsNullOrEmpty(director) || filmFoam.Director.Equals(director, StringComparison.OrdinalIgnoreCase))
//                && (string.IsNullOrEmpty(producer) || filmFoam.Producer.Equals(producer, StringComparison.OrdinalIgnoreCase))
//                && (string.IsNullOrEmpty(planets) || ContainsAny(filmFoam.Planets, planets.Split(',')))
//                && (string.IsNullOrEmpty(starships) || ContainsAny(filmFoam.Starships, starships.Split(',')))
//                && (string.IsNullOrEmpty(vehicles) || ContainsAny(filmFoam.Vehicles, vehicles.Split(',')))
//                && (string.IsNullOrEmpty(species) || await EntityNameExistsAsync<Species>(client, filmFoam.Species, species));
//        }

//        private void AddQueryParam(List<string> queryParams, string paramName, string paramValue)
//        {
//            if (!string.IsNullOrEmpty(paramValue))
//            {
//                queryParams.Add($"{paramName}={Uri.EscapeDataString(paramValue)}");
//            }
//        }

//        private async Task<List<T>> GetDetailsAsync<T>(HttpClient client, List<string> urls)
//        {
//            var tasks = urls.Select(async url =>
//            {
//                try
//                {
//                    var result = await client.GetAsync(url);
//                    result.EnsureSuccessStatusCode();
//                    string resultContentString = await result.Content.ReadAsStringAsync();

//                    return JsonConvert.DeserializeObject<T>(resultContentString);
//                }
//                catch (Exception ex)
//                {
//                    // Log the exception for debugging purposes
//                    Console.WriteLine($"Error fetching details from {url}: {ex.ToString()}");
//                    return default(T);
//                }
//            });

//            return (await Task.WhenAll(tasks)).Where(t => t != null).ToList();
//        }

//        private async Task<bool> EntityNameExistsAsync<T>(HttpClient client, List<string> urls, string name) where T : IEntityWithName
//        {
//            var tasks = urls.Select(async url =>
//            {
//                try
//                {
//                    var result = await client.GetAsync(url);
//                    result.EnsureSuccessStatusCode();
//                    string resultContentString = await result.Content.ReadAsStringAsync();

//                    var entity = JsonConvert.DeserializeObject<T>(resultContentString);

//                    return entity.Name.Equals(name, StringComparison.OrdinalIgnoreCase);
//                }
//                catch (Exception ex)
//                {
//                    // Log the exception for debugging purposes
//                    Console.WriteLine($"Error fetching details from {url}: {ex.ToString()}");
//                    return false;
//                }
//            });

//            return (await Task.WhenAll(tasks)).Any(result => result);
//        }

//        private bool ContainsAny(IEnumerable<string> source, IEnumerable<string> values)
//        {
//            return values.Any(v => source.Contains(v, StringComparer.OrdinalIgnoreCase));
//        }
//    }
//}


using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SwapiApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SwapiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Swapi : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;

        public Swapi(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        [HttpGet]
        public async Task<ActionResult<List<Film>>> SearchFilm(
            string title = null,
            string characters = null,
            string director = null,
            string producer = null,
            string planets = null,
            string starships = null,
            string vehicles = null,
            string species = null)
        {
            try
            {
                // Check if the result is in the cache
                string cacheKey = BuildCacheKey(title, characters, director, producer, planets, starships, vehicles, species);
                if (_memoryCache.TryGetValue(cacheKey, out List<Film> cachedFilms))
                {
                    return cachedFilms;
                }

                // If not in cache, proceed with API requests as before

                List<Film> filmsList = new List<Film>();
                string apiBasicUri = "https://swapi.dev/api/films/";

                // Build the query parameters based on the provided search criteria
                var queryParams = new List<string>();

                AddQueryParam(queryParams, "title", title);
                AddQueryParam(queryParams, "director", director);
                AddQueryParam(queryParams, "producer", producer);
                AddQueryParam(queryParams, "planets", planets);
                AddQueryParam(queryParams, "starships", starships);
                AddQueryParam(queryParams, "vehicles", vehicles);
                AddQueryParam(queryParams, "species", species);

                string apiUrlWithQuery = $"{apiBasicUri}?{string.Join("&", queryParams)}";

                // Log the constructed API URL for debugging
                Console.WriteLine($"Constructed API URL: {apiUrlWithQuery}");

                using (var client = new HttpClient())
                {
                    var result = await client.GetAsync(apiUrlWithQuery);
                    result.EnsureSuccessStatusCode();
                    string resultContentString = await result.Content.ReadAsStringAsync();

                    var responseObject = JsonConvert.DeserializeObject<JObject>(resultContentString);

                    if (responseObject.TryGetValue("results", out var filmsToken) && filmsToken is JArray filmsArray)
                    {
                        var filmFoams = filmsArray.ToObject<List<FilmFoam>>();

                        var tasks = filmFoams.Select(async filmFoam =>
                        {
                            Console.WriteLine($"Processing film: {filmFoam.Title}");

                            if (await IsFilmMatch(client, filmFoam, title, characters, director, producer, planets, starships, vehicles, species))
                            {
                                Console.WriteLine($"Film {filmFoam.Title} matches the search criteria.");

                                Film filmWithDetails = new Film
                                {
                                    Title = filmFoam.Title,
                                    EpisodeId = filmFoam.Episode_id,
                                    OpeningCrawl = filmFoam.OpeningCrawl,
                                    Director = filmFoam.Director,
                                    Producer = filmFoam.Producer,
                                    ReleaseDate = filmFoam.ReleaseDate,
                                    Created = filmFoam.Created,
                                    Edited = filmFoam.Edited,
                                    Url = filmFoam.Url,
                                    CharacterUrls = filmFoam.Characters ?? new List<string>(),
                                    PlanetUrls = filmFoam.Planets ?? new List<string>(),
                                    StarshipUrls = filmFoam.Starships ?? new List<string>(),
                                    VehicleUrls = filmFoam.Vehicles ?? new List<string>(),
                                    SpeciesUrls = filmFoam.Species ?? new List<string>(),
                                };

                                // Fetch details for characters, planets, starships, vehicles, and species
                                filmWithDetails.Characters = await GetDetailsAsync<Peoples>(client, filmFoam.Characters);
                                filmWithDetails.Planets = await GetDetailsAsync<Planets>(client, filmFoam.Planets);
                                filmWithDetails.Starships = await GetDetailsAsync<Starships>(client, filmFoam.Starships);
                                filmWithDetails.Vehicles = await GetDetailsAsync<Vehicles>(client, filmFoam.Vehicles);
                                filmWithDetails.Species = await GetDetailsAsync<Species>(client, filmFoam.Species);

                                return filmWithDetails;
                            }

                            Console.WriteLine($"Film {filmFoam.Title} does not match the search criteria.");
                            return null;
                        });

                        filmsList.AddRange(await Task.WhenAll(tasks.Where(t => t != null)));
                    }
                    else
                    {
                        // Log or handle unexpected API response
                        return BadRequest("Unexpected API response format.");
                    }
                }

                // Store the result in the cache
                _memoryCache.Set(cacheKey, filmsList, TimeSpan.FromMinutes(30)); // Set cache expiration time

                return filmsList;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine($"An unexpected error occurred: {ex}");

                // Return a more meaningful error message to the client
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred. Exception: {ex.Message}");
            }
        }

        private async Task<bool> IsFilmMatch(HttpClient client, FilmFoam filmFoam, string title, string characters, string director, string producer, string planets, string starships, string vehicles, string species)
        {
            return (string.IsNullOrEmpty(title) || filmFoam.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                && (string.IsNullOrEmpty(characters) || await EntityNameExistsAsync<Peoples>(client, filmFoam.Characters, characters))
                && (string.IsNullOrEmpty(director) || filmFoam.Director.Equals(director, StringComparison.OrdinalIgnoreCase))
                && (string.IsNullOrEmpty(producer) || filmFoam.Producer.Equals(producer, StringComparison.OrdinalIgnoreCase))
                && (string.IsNullOrEmpty(planets) || ContainsAny(filmFoam.Planets, planets.Split(',')))
                && (string.IsNullOrEmpty(starships) || ContainsAny(filmFoam.Starships, starships.Split(',')))
                && (string.IsNullOrEmpty(vehicles) || ContainsAny(filmFoam.Vehicles, vehicles.Split(',')))
                && (string.IsNullOrEmpty(species) || await EntityNameExistsAsync<Species>(client, filmFoam.Species, species));
        }

        private void AddQueryParam(List<string> queryParams, string paramName, string paramValue)
        {
            if (!string.IsNullOrEmpty(paramValue))
            {
                queryParams.Add($"{paramName}={Uri.EscapeDataString(paramValue)}");
            }
        }

        private async Task<List<T>> GetDetailsAsync<T>(HttpClient client, List<string> urls)
        {
            var tasks = urls.Select(async url =>
            {
                try
                {
                    var result = await client.GetAsync(url);
                    result.EnsureSuccessStatusCode();
                    string resultContentString = await result.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<T>(resultContentString);
                }
                catch (Exception ex)
                {
                    // Log the exception for debugging purposes
                    Console.WriteLine($"Error fetching details from {url}: {ex.ToString()}");
                    return default(T);
                }
            });

            return (await Task.WhenAll(tasks)).Where(t => t != null).ToList();
        }

        private async Task<bool> EntityNameExistsAsync<T>(HttpClient client, List<string> urls, string name) where T : IEntityWithName
        {
            var tasks = urls.Select(async url =>
            {
                try
                {
                    var result = await client.GetAsync(url);
                    result.EnsureSuccessStatusCode();
                    string resultContentString = await result.Content.ReadAsStringAsync();

                    var entity = JsonConvert.DeserializeObject<T>(resultContentString);

                    return entity.Name.Equals(name, StringComparison.OrdinalIgnoreCase);
                }
                catch (Exception ex)
                {
                    // Log the exception for debugging purposes
                    Console.WriteLine($"Error fetching details from {url}: {ex.ToString()}");
                    return false;
                }
            });

            return (await Task.WhenAll(tasks)).Any(result => result);
        }

        private bool ContainsAny(IEnumerable<string> source, IEnumerable<string> values)
        {
            return values.Any(v => source.Contains(v, StringComparer.OrdinalIgnoreCase));
        }

        private string BuildCacheKey(params object[] parameters)
        {
            return string.Join("_", parameters.Select(p => p?.ToString() ?? "null"));
        }
    }
}
