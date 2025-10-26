using SmartRecyclingApi.Data;
using SmartRecyclingApi.Models;
using SmartRecyclingApi.ViewModels.Pedido;

namespace SmartRecyclingApi.Services.Pedido
{
    public class PedidoService : IPedidoInterface
    {
        private readonly AppDbContext _context;

        public PedidoService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<ResponseModel<PedidoModel>> InserirPedido(InserirPedidoDto pedidoDto)
        {
            ResponseModel<PedidoModel> resposta = new ResponseModel<PedidoModel>();

            if (pedidoDto.ref_Utilizador == 0)
            {
                resposta.Mensagem = "O utilizador é necessário";
                return resposta;
            }

            var utilizadorExiste = await _context.Utilizadores.FindAsync(pedidoDto.ref_Utilizador);
            if (utilizadorExiste == null)
            {
                resposta.Mensagem = "Utilizador não encontrado";
                resposta.Status = false;
                return resposta;
            }

            try
            {
                var pedido = new PedidoModel()
                {
                    ref_Utilizador = pedidoDto.ref_Utilizador,
                    Tipo_Pedido = pedidoDto.Tipo_Pedido,
                    Status_Pedido = pedidoDto.Status_Pedido,
                    Data_Criacao = pedidoDto.Data_Criacao
                };

                _context.Add(pedido);
                await _context.SaveChangesAsync();

                resposta.Mensagem = "Pedido inserido com sucesso.";
                resposta.Status = true;
            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
            }
            return resposta;
        }
    }
}
