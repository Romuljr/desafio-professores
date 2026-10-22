using DesafioProfessores.Api.Models;
using DesafioProfessores.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioProfessores.Api.Controllers
{
    [ApiController]
    [Route("api/professores")]
    public class ProfessoresController : ControllerBase
    {
        private readonly IProfessorRepository _repository;
        private readonly BuscadorProfessoresService _buscadorService;

        public ProfessoresController(IProfessorRepository repository, BuscadorProfessoresService buscadorService)
        {
            _repository = repository;
            _buscadorService = buscadorService;
        }

        [HttpPost("busca")]
        public IActionResult BuscarProfessores([FromBody] BuscaProfessoresRequest request)
        {
            var professores = _repository.ObterTodos();
            var (isValid, errorMessage, response) = _buscadorService.ProcessarBusca(request, professores);

            if (!isValid)
            {
                return BadRequest(new { mensagem = errorMessage });
            }

            return Ok(response);
        }
    }
}
