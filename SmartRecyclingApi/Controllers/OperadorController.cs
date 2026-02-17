using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartRecyclingApi.Models;
using SmartRecyclingApi.Services.Operador;
using SmartRecyclingApi.Services.Pedido;
using SmartRecyclingApi.ViewModels.Operador;

namespace SmartRecyclingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [HttpGet("GetUtilizadores")]
        public async Task<ActionResult<ResponseModel<OperUtilizadorView>>> GetUtilizadores()
        {
            var nomeOperador = await _operadorInterface.GetUtilizadores();
            return Ok (nomeOperador); 
        }
    }
}
