using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using RCN.API.Data;

namespace RCN.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepository Repositorio;

        public ProdutosController(IProdutoRepository repositorio)
        {
            Repositorio = repositorio;
        }

        [HttpPost]
        [ApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult Criar([FromBody]Produto produto)
        {
            if (produto.Codigo == "")
                return BadRequest("Codigo do produto nao informado!");

            if (string.IsNullOrEmpty(produto.Descricao))
                return BadRequest("Descricao do produto nao informada!");

            Repositorio.Inserir(produto);

            return Created(nameof(Criar), produto);
        }

        [HttpGet]
        [ApiVersion("1.0")]
        [ResponseCache(Duration=30)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [Produces("application/json", "application/xml")]
        public IActionResult Obter()
        {
            var  lista = Repositorio.Obter();

            return Ok(lista);
        }

        [HttpGet("{id}")]
        [ApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public IActionResult Obter(int id)
        {
            var produto = Repositorio.Obter(id);

            if(produto == null) return NotFound();

           return Ok(produto);
        }

        [HttpPut]
        [ApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public IActionResult Atualizar([FromBody]Produto produto)
        {
            var prod = Repositorio.Obter(produto.Id);

            if(prod == null) return NotFound();

            if (produto.Codigo == "")
                return BadRequest("Codigo do produto nao informado!");

            if (string.IsNullOrEmpty(produto.Descricao))
                return BadRequest("Descricao do produto nao informada!");

            Repositorio.Editar(produto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ApiVersion("1.0")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult Apagar(int id)
        {
            var produto = Repositorio.Obter(id);

            if(produto == null) return NotFound();

            Repositorio.Excluir(produto);
            
            return Ok();
        }

        [HttpGet("{codigo}")]
        [ApiVersion("2.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult ObterPorCodigo(string codigo)
        {
            return Ok("metodo obter por codigo - versao 2");
        }

        [HttpGet("")]
        [ApiVersion("3.0")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult ObterTodos()
        {
            List<string> lista = new List<string>();

            for (int i = 0; i < 1000; i++)
            {
                lista.Add($"indice: {i}");
            }
            
            return Ok(string.Join(",",lista));
        }
    }
}