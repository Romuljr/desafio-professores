using DesafioProfessores.Api.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioProfessores.Api.Services
{
    public class BuscadorProfessoresService
    {
        private static readonly HashSet<string> DiasValidos = new(StringComparer.OrdinalIgnoreCase)
    {
        "segunda", "terca", "quarta", "quinta", "sexta", "sabado", "domingo"
    };

        public (bool IsValid, string ErrorMessage, BuscaProfessoresResponse Response) ProcessarBusca(
            BuscaProfessoresRequest req, IEnumerable<Professor> professores)
        {
            // 1. Validações do Request
            if (string.IsNullOrWhiteSpace(req.Idioma))
                return (false, "O idioma não pode estar vazio.", null!);

            if (req.Dias == null || !req.Dias.Any())
                return (false, "Pelo menos um dia da semana deve ser informado.", null!);

            foreach (var dia in req.Dias)
            {
                if (!DiasValidos.Contains(dia))
                    return (false, $"O dia '{dia}' é inválido.", null!);
            }

            if (!TimeSpan.TryParseExact(req.InicioFaixa, @"hh\:mm", CultureInfo.InvariantCulture, out var inicioFaixa) ||
                !TimeSpan.TryParseExact(req.FimFaixa, @"hh\:mm", CultureInfo.InvariantCulture, out var fimFaixa))
            {
                return (false, "O formato dos horários deve ser HH:mm.", null!);
            }

            if (fimFaixa <= inicioFaixa)
                return (false, "O horário final da faixa deve ser maior que o horário inicial.", null!);

            if (req.DuracaoMinutos != 30 && req.DuracaoMinutos != 60 && req.DuracaoMinutos != 90)
                return (false, "A duração da aula deve ser 30, 60 ou 90 minutos.", null!);

            // 2. Execução das Regras de Elegibilidade
            var elegiveis = new List<ProfessorElegivelDto>();
            var naoElegiveis = new List<ProfessorNaoElegivelDto>();

            foreach (var prof in professores)
            {
                // Regra 1: Idioma
                if (!prof.Idiomas.Any(i => string.Equals(i, req.Idioma, StringComparison.OrdinalIgnoreCase)))
                {
                    naoElegiveis.Add(new ProfessorNaoElegivelDto
                    {
                        Id = prof.Id,
                        Nome = prof.Nome,
                        Motivo = "Não ensina o idioma"
                    });
                    continue;
                }

                // Regra 2: Agenda Bloqueada
                if (prof.AgendaBloqueada)
                {
                    naoElegiveis.Add(new ProfessorNaoElegivelDto
                    {
                        Id = prof.Id,
                        Nome = prof.Nome,
                        Motivo = "Agenda bloqueada"
                    });
                    continue;
                }

                // Regra 3, 4, 5, 6: Horários disponíveis
                var horariosPossiveis = CalcularHorariosElegiveis(prof, req, inicioFaixa, fimFaixa);

                if (!horariosPossiveis.Any())
                {
                    naoElegiveis.Add(new ProfessorNaoElegivelDto
                    {
                        Id = prof.Id,
                        Nome = prof.Nome,
                        Motivo = "Sem horário em comum nos dias pedidos"
                    });
                    continue;
                }

                elegiveis.Add(new ProfessorElegivelDto
                {
                    Id = prof.Id,
                    Nome = prof.Nome,
                    AulasNaSemana = prof.AulasMarcadas.Count,
                    Horarios = horariosPossiveis.Select(h => h.ToString(@"hh\:mm")).ToList()
                });
            }

            // Regra 7: Ordenação dos elegíveis
            var elegiveisOrdenados = elegiveis
                .OrderBy(p => p.AulasNaSemana)
                .ThenBy(p => p.Nome)
                .ToList();

            var response = new BuscaProfessoresResponse
            {
                Professores = elegiveisOrdenados,
                NaoElegiveis = naoElegiveis
            };

            return (true, string.Empty, response);
        }

        private List<TimeSpan> CalcularHorariosElegiveis(
            Professor prof, BuscaProfessoresRequest req, TimeSpan inicioFaixa, TimeSpan fimFaixa)
        {
            var duracao = TimeSpan.FromMinutes(req.DuracaoMinutos);
            var candidatosLocais = new List<TimeSpan>();

            // Gera slots em :00 e :30 dentro da faixa informada
            for (var t = inicioFaixa; t + duracao <= fimFaixa; t = t.Add(TimeSpan.FromMinutes(30)))
            {
                if (t.Minutes != 0 && t.Minutes != 30) continue;
                candidatosLocais.Add(t);
            }

            var horariosValidosEmTodosOsDias = new List<TimeSpan>();

            foreach (var horario in candidatosLocais)
            {
                var validoEmTodosOsDias = true;
                var fimAulaPretendida = horario + duracao;

                foreach (var dia in req.Dias)
                {
                    // Verifica se cabe na disponibilidade do professor no dia
                    var cabeNaDisponibilidade = prof.Disponibilidade.Any(d =>
                        string.Equals(d.Dia, dia, StringComparison.OrdinalIgnoreCase) &&
                        TimeSpan.Parse(d.Inicio) <= horario &&
                        TimeSpan.Parse(d.Fim) >= fimAulaPretendida);

                    if (!cabeNaDisponibilidade)
                    {
                        validoEmTodosOsDias = false;
                        break;
                    }

                    // Verifica se NÃO conflita com aulas marcadas no dia
                    var conflitaComAula = prof.AulasMarcadas.Any(a =>
                    {
                        if (!string.Equals(a.Dia, dia, StringComparison.OrdinalIgnoreCase))
                            return false;

                        var inicioAula = TimeSpan.Parse(a.Inicio);
                        var fimAula = inicioAula.Add(TimeSpan.FromMinutes(a.DuracaoMinutos));

                        // Permite encostar: (novoInicio < fimExistente) E (novoFim > inicioExistente)
                        return horario < fimAula && fimAulaPretendida > inicioAula;
                    });

                    if (conflitaComAula)
                    {
                        validoEmTodosOsDias = false;
                        break;
                    }
                }

                if (validoEmTodosOsDias)
                {
                    horariosValidosEmTodosOsDias.Add(horario);
                }
            }

            return horariosValidosEmTodosOsDias.OrderBy(h => h).ToList();
        }
    }
}
