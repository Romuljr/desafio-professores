using DesafioProfessores.Api.Models;
using System.Globalization;
using System.Text.Json;

namespace DesafioProfessores.Api.Services;

public interface IProfessorRepository
{
    IEnumerable<Professor> ObterTodos();
}
