using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartRecyclingApi.Data;
using SmartRecyclingApi.Models;
using SmartRecyclingApi.ViewModels.Pedido;
using System.Linq.Expressions;

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
        public async Task<ResponseModel<List<PedidoModel>>> GetPedidoPendentes()
        {
            ResponseModel<List<PedidoModel>> resposta = new ResponseModel<List<PedidoModel>>();
            try
            {
                var pedidos = _context.Pedido.Where(u => u.Status_Pedido == "Pendente").ToList();

                if (pedidos == null)
                {
                    resposta.Mensagem = "Sem pedidos para mostrar";
                }

                resposta.Dados = pedidos;
                resposta.Mensagem = "Pedidos Encontrados";
                return resposta;
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