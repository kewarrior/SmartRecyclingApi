using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartRecyclingApi.Models;
using SmartRecyclingApi.Services.Pedido;
using SmartRecyclingApi.ViewModels.Pedido;

namespace SmartRecyclingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
    }
}
