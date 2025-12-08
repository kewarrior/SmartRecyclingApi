using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartRecyclingApi.Models;
using SmartRecyclingApi.Services.Pedido;
using SmartRecyclingApi.ViewModels.Pedido;

namespace SmartRecyclingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoInterface _pedidoInterface;

        public PedidoController (IPedidoInterface pedidoInterface)
        {
            _pedidoInterface = pedidoInterface;
        }


        [HttpPost("InserirPedido")]

        public async Task<ActionResult<ResponseModel<PedidoModel>>> InserirPedido(InserirPedidoDto pedidoDto)
        {
            var inserirPedido = await _pedidoInterface.InserirPedido(pedidoDto);
            return Ok (inserirPedido);
        }

        [HttpGet("GetPedidoByUtilizadorId")]

        public async Task<ActionResult<ResponseModel<PedidoModel>>> GetPedidoByUtilizadorId(long ref_utilizador)
        {
            var pedido = await _pedidoInterface.GetPedidoByUtilizadorId(ref_utilizador);
            return Ok (pedido);
        }


        [HttpGet("GetPedidoPendentes")]
        public async Task<ActionResult<ResponseModel<List<PedidoModel>>>> GetPedidoPendentes()
        {
            var pedidosPenentes = await _pedidoInterface.GetPedidoPendentes();
            return Ok (pedidosPenentes);
        }
    }
}
