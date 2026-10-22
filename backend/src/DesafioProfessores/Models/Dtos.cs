using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioProfessores.Api.Models
{
    public class BuscaProfessoresRequest
    {
        public string Idioma { get; set; } = string.Empty;
        public List<string> Dias { get; set; } = new();
        public string InicioFaixa { get; set; } = string.Empty;
        public string FimFaixa { get; set; } = string.Empty;
        public int DuracaoMinutos { get; set; }
    }

    public class BuscaProfessoresResponse
    {
        public List<ProfessorElegivelDto> Professores { get; set; } = new();
        public List<ProfessorNaoElegivelDto> NaoElegiveis { get; set; } = new();
    }

    public class ProfessorElegivelDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int AulasNaSemana { get; set; }
        public List<string> Horarios { get; set; } = new();
    }

    public class ProfessorNaoElegivelDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
    }

    // Representação do JSON
    public class Professor
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public List<string> Idiomas { get; set; } = new();
        public bool AgendaBloqueada { get; set; }
        public List<BlocoDisponibilidade> Disponibilidade { get; set; } = new();
        public List<AulaMarcada> AulasMarcadas { get; set; } = new();
    }

    public class BlocoDisponibilidade
    {
        public string Dia { get; set; } = string.Empty;
        public string Inicio { get; set; } = string.Empty;
        public string Fim { get; set; } = string.Empty;
    }

    public class AulaMarcada
    {
        public string Dia { get; set; } = string.Empty;
        public string Inicio { get; set; } = string.Empty;
        public int DuracaoMinutos { get; set; }
    }
}
