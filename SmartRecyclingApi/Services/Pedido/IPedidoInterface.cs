using SmartRecyclingApi.Models;
using SmartRecyclingApi.ViewModels.Pedido;

namespace SmartRecyclingApi.Services.Pedido
{
    public interface IPedidoInterface
    {
        Task<ResponseModel<PedidoModel>> InserirPedido(InserirPedidoDto pedidoDto);
        Task<ResponseModel<PedidoModel>> GetPedidoByUtilizadorId(long ref_utilizador);
    }
}
