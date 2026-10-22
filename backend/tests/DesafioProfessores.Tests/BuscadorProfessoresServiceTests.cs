using DesafioProfessores.Api.Models;
using DesafioProfessores.Api.Services;
using Xunit;

namespace DesafioProfessores.Tests;

public class BuscadorProfessoresServiceTests
{
    private readonly BuscadorProfessoresService _service = new();

    private List<Professor> ObterCenarioBase()
    {
        return new List<Professor>
        {
            new Professor
            {
                Id = 1,
                Nome = "Ana",
                Idiomas = new() { "Inglês", "Espanhol" },
                AgendaBloqueada = false,
                Disponibilidade = new() {
                    new BlocoDisponibilidade { Dia = "segunda", Inicio = "18:00", Fim = "22:00" },
                    new BlocoDisponibilidade { Dia = "quarta", Inicio = "18:00", Fim = "22:00" }
                },
                AulasMarcadas = new() {
                    new AulaMarcada { Dia = "segunda", Inicio = "19:00", DuracaoMinutos = 60 }
                }
            },
            new Professor
            {
                Id = 2,
                Nome = "Bruno",
                Idiomas = new() { "Inglês" },
                AgendaBloqueada = false,
                Disponibilidade = new() {
                    new BlocoDisponibilidade { Dia = "segunda", Inicio = "18:00", Fim = "21:00" }
                },
                AulasMarcadas = new() // 0 aulas na semana
            },
            new Professor
            {
                Id = 3,
                Nome = "Carla",
                Idiomas = new() { "Inglês" },
                AgendaBloqueada = true, // Regra 2: Agenda bloqueada
                Disponibilidade = new() {
                    new BlocoDisponibilidade { Dia = "segunda", Inicio = "08:00", Fim = "22:00" }
                },
                AulasMarcadas = new()
            }
        };
    }

    [Fact]
    public void Regra1_ProfessorDeveEnsinarOIdiomaPedido()
    {
        // O Bruno só ensina Inglês. Buscando Espanhol, ele deve ir para naoElegiveis.
        var req = new BuscaProfessoresRequest
        {
            Idioma = "Espanhol",
            Dias = new() { "segunda" },
            InicioFaixa = "18:00",
            FimFaixa = "20:00",
            DuracaoMinutos = 60
        };

        var (_, _, res) = _service.ProcessarBusca(req, ObterCenarioBase());

        var bruno = res.NaoElegiveis.FirstOrDefault(p => p.Id == 2);
        Assert.NotNull(bruno);
        Assert.Equal("Não ensina o idioma", bruno.Motivo);
    }

    [Fact]
    public void Regra2_ProfessorComAgendaBloqueadaNaoAparece()
    {
        // Carla tem agenda bloqueada = true. Deve ir para naoElegiveis.
        var req = new BuscaProfessoresRequest
        {
            Idioma = "Inglês",
            Dias = new() { "segunda" },
            InicioFaixa = "08:00",
            FimFaixa = "12:00",
            DuracaoMinutos = 60
        };

        var (_, _, res) = _service.ProcessarBusca(req, ObterCenarioBase());

        var carla = res.NaoElegiveis.FirstOrDefault(p => p.Id == 3);
        Assert.NotNull(carla);
        Assert.Equal("Agenda bloqueada", carla.Motivo);
    }

    [Fact]
    public void Regra3_E_Regra4_PrecisaTerHorarioFixoEmTodosOsDiasE_CaberNaDisponibilidade()
    {
        // Pedindo segunda E quarta das 18:00 às 19:00.
        // Ana tem disponibilidade em ambos os dias nesse horário.
        var req = new BuscaProfessoresRequest
        {
            Idioma = "Inglês",
            Dias = new() { "segunda", "quarta" },
            InicioFaixa = "18:00",
            FimFaixa = "19:00",
            DuracaoMinutos = 60
        };

        var (_, _, res) = _service.ProcessarBusca(req, ObterCenarioBase());

        var ana = res.Professores.FirstOrDefault(p => p.Id == 1);
        Assert.NotNull(ana);
        Assert.Contains("18:00", ana.Horarios);
    }

    [Fact]
    public void Regra5_NaoPodeSobreporAulaMarcada_PoremEncostarEhPermitido()
    {
        // Ana tem aula marcada segunda das 19:00 às 20:00.
        // Um slot às 18:00 (duração 60 min -> termina exatamente às 19:00) DEVE ser permitido (encostar).
        // Um slot às 19:00 (sobrepõe a aula existente) DEVE ser bloqueado.
        var req = new BuscaProfessoresRequest
        {
            Idioma = "Espanhol",
            Dias = new() { "segunda" },
            InicioFaixa = "18:00",
            FimFaixa = "20:00",
            DuracaoMinutos = 60
        };

        var (_, _, res) = _service.ProcessarBusca(req, ObterCenarioBase());

        var ana = res.Professores.FirstOrDefault(p => p.Id == 1);
        Assert.NotNull(ana);
        Assert.Contains("18:00", ana.Horarios);   // Termina às 19:00 (Encosta - Permitido)
        Assert.DoesNotContain("19:00", ana.Horarios); // Conflita com a aula existente (Bloqueado)
    }

    [Fact]
    public void Regra7_Ordenacao_PrimeiroQuemTemMenosAulas_DepoisOrdemAlfabetica()
    {
        // Bruno tem 0 aulas marcadas e Ana tem 1. Bruno deve vir primeiro na lista.
        var req = new BuscaProfessoresRequest
        {
            Idioma = "Inglês",
            Dias = new() { "segunda" },
            InicioFaixa = "18:00",
            FimFaixa = "19:00",
            DuracaoMinutos = 60
        };

        var (_, _, res) = _service.ProcessarBusca(req, ObterCenarioBase());

        Assert.Equal(2, res.Professores.Count);
        Assert.Equal("Bruno", res.Professores[0].Nome); // 0 aulas
        Assert.Equal("Ana", res.Professores[1].Nome);   // 1 aula
    }
}