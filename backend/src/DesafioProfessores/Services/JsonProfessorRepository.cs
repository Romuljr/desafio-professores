using DesafioProfessores.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DesafioProfessores.Api.Services
{
    public class JsonProfessorRepository : IProfessorRepository
    {
        private readonly List<Professor> _professores = new();

        public JsonProfessorRepository(IWebHostEnvironment env)
        {
            var caminhoJson = Path.Combine(env.ContentRootPath, "Data", "professores.json");
            if (File.Exists(caminhoJson))
            {
                var jsonString = File.ReadAllText(caminhoJson);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var doc = JsonDocument.Parse(jsonString);
                if (doc.RootElement.TryGetProperty("professores", out var prop))
                {
                    _professores = JsonSerializer.Deserialize<List<Professor>>(prop.GetRawText(), options) ?? new();
                }
            }
        }

        public IEnumerable<Professor> ObterTodos() => _professores;
    }
}
