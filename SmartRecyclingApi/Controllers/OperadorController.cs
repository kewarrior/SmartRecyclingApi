using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartRecyclingApi.Models;
using SmartRecyclingApi.Services.Operador;
using SmartRecyclingApi.Services.Pedido;

namespace SmartRecyclingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperadorController : ControllerBase
    {

        private readonly IOperadorInterface _operadorInterface;

        public OperadorController(IOperadorInterface operadorInterface)
        {
            _operadorInterface = operadorInterface;
        }

        [HttpPost("InserirDadosReciclagem")]
        public async Task<ActionResult<ResponseModel<ReciclagemModel>>> InserirDadosReciclagem(ReciclagemModel reciclagem)
        {
            var inserir = await _operadorInterface.InserirDadosReciclagem(reciclagem);
            return Ok(inserir);
        }


        [HttpGet("GetNomeOperador")]
        public async Task<ActionResult<ResponseModel<UtilizadorModel>>> GetNomeOperador(long id)
        {
            var nomeOperador = await _operadorInterface.GetNomeOperador(id);
            return Ok (nomeOperador); 
        }
    }
}
