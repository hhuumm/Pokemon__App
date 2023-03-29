using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pokemon__App.Data;
using Pokemon__App.Models;
using System.Diagnostics;

namespace Pokemon__App.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PokemonController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pokemon>>> GetPokemon(
            [FromQuery] string? search,
            [FromQuery(Name = "hp[gte]")] int? hpGte,
            [FromQuery(Name = "hp[lte]")] int? hpLte,
            [FromQuery(Name = "attack[gte]")] int? attackGte,
            [FromQuery(Name = "attack[lte]")] int? attackLte,
            [FromQuery(Name = "defense[gte]")] int? defenseGte,
            [FromQuery(Name = "defense[lte]")] int? defenseLte,
            [FromQuery] int? page)
        {
            IQueryable<Pokemon> query = _context.Pokemon;

            // Search
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => EF.Functions.Like(p.Name, $"%{search}%"));
            }

            // Filter
            if (hpGte.HasValue)
            {
                query = query.Where(p => p.HP >= hpGte.Value);
            }
            if (hpLte.HasValue)
            {
                query = query.Where(p => p.HP <= hpLte.Value);
            }
            if (attackGte.HasValue)
            {
                query = query.Where(p => p.Attack >= attackGte.Value);
            }
            if (attackLte.HasValue)
            {
                query = query.Where(p => p.Attack <= attackLte.Value);
            }
            if (defenseGte.HasValue)
            {
                query = query.Where(p => p.Defense >= defenseGte.Value);
            }
            if (defenseLte.HasValue)
            {
                query = query.Where(p => p.Defense <= defenseLte.Value);
            }

            // Pagination
            int pageSize = 20;
            int pageNumber = page ?? 1;
            var pagedQuery = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var pokemon = await pagedQuery.ToListAsync();

            // Sort by Levenshtein distance
            if (!string.IsNullOrEmpty(search))
            {
                var searchTerm = search.ToLower();
                pokemon = pokemon.OrderByDescending(p => LevenshteinDistance(p.Name.ToLower(), searchTerm)).ToList();
            }

            return pokemon;
        }

        private static int LevenshteinDistance(string s1, string s2)
        {
            var source1Length = s1.Length;
            var source2Length = s2.Length;

            var matrix = new int[source1Length + 1, source2Length + 1];

            // First calculation, if one entry is empty return full length
            if (source1Length == 0)
                return source2Length;

            if (source2Length == 0)
                return source1Length;

            // Initialization of matrix with row size source1Length and columns size source2Length
            for (var i = 0; i <= source1Length; matrix[i, 0] = i++) { }
            for (var j = 0; j <= source2Length; matrix[0, j] = j++) { }

            // Calculate rows and collumns distances
            for (var i = 1; i <= source1Length; i++)
            {
                for (var j = 1; j <= source2Length; j++)
                {
                    var cost = (s2[j - 1] == s1[i - 1]) ? 0 : 1;

                    matrix[i, j] = Math.Min(
                        Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                        matrix[i - 1, j - 1] + cost);
                }
            }
            // return result
            return matrix[source1Length, source2Length];
        }

        [HttpPost]
        public async Task<IActionResult> SavePokemon([FromBody] List<Pokemon> pokemonList)
        {
            if (pokemonList == null || pokemonList.Count == 0)
            {
                return BadRequest("No pokemon provided");
            }

            try
            {
                List<Pokemon> newPokemonList = new List<Pokemon>();

                foreach (Pokemon pokemon in pokemonList)
                {
                    if (!_context.Pokemon.Any(p => p.Name == pokemon.Name))
                    {
                        newPokemonList.Add(pokemon);
                    }
                }
                if (newPokemonList.Count > 0)
                {
                    _context.Pokemon.AddRange(newPokemonList);
                    await _context.SaveChangesAsync();

                    return Ok("Pokemon saved successfully");
                }
                else
                {
                    return Ok("No new pokemon added");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error saving pokemon: {ex.Message}");
            }
        }
    }


}
