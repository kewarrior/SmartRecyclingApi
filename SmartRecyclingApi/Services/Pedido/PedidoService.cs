using Microsoft.EntityFrameworkCore;
using SmartRecyclingApi.Data;
using SmartRecyclingApi.Models;
using SmartRecyclingApi.ViewModels;
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

        public async Task<ResponseModel<PedidoModel>> GetPedidoByUtilizadorId(long ref_utilizador)
        {
            ResponseModel<PedidoModel> resposta = new ResponseModel<PedidoModel>();
            if (ref_utilizador == 0)
            {
                resposta.Mensagem = "É necessário o id do utilizador";
                return resposta;
            }

            try
            {
                var pedidos = await _context.Pedido.FirstOrDefaultAsync(u => u.ref_Utilizador == ref_utilizador);

                if (pedidos == null)
                {
                    resposta.Mensagem = "Nenhum pedido para esse Utilizador.";
                }



                resposta.Dados = pedidos;
                resposta.Mensagem = "Pedido Encontrado";
                return resposta;

            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
            }
            return resposta;
        }
        public async Task<ResponseModel<List<PedidoDTO>>> GetPedidoPendentes()
        {
            ResponseModel<List<PedidoDTO>> resposta = new ResponseModel<List<PedidoDTO>>();
            try
            {
                var pedidos = await (from p in _context.Pedido
                                     join u in _context.Utilizadores on p.ref_Utilizador equals u.Id
                                     where p.Status_Pedido == "Pendente"
                                     select new PedidoDTO
                                     {
                                         Id = p.Id,
                                         Tipo_Pedido = p.Tipo_Pedido,
                                         Status_Pedido = p.Status_Pedido,
                                         Data_Criacao = p.Data_Criacao,
                                         NomeUtilizador = u.nome
                                     }
                                     ).ToListAsync();

                if (!pedidos.Any())
                {
                    resposta.Mensagem = "Sem pedidos para mostrar";
                    return resposta;
                }

                resposta.Dados = pedidos;
                resposta.Mensagem = "Pedidos Encontrados";
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