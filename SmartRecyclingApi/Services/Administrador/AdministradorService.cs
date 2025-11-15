using Microsoft.EntityFrameworkCore;
using SmartRecyclingApi.Data;
using SmartRecyclingApi.Models;

namespace SmartRecyclingApi.Services.Administrador
{
    public class AdministradorService : IAdminInterface
    {
        private readonly AppDbContext _context;

        public AdministradorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<object>> UpdatePedido(long id)
        {
            ResponseModel<object> resposta = new ResponseModel<object>();

            try
            {

                var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(u => u.Id == id && u.adesao == false);

                if (utilizador == null)
                {
                    resposta.Status = false;
                    resposta.Mensagem = "Utilizador não encontrado ou adesão já aceite";
                    return resposta;
                }

                var updateStatus = await _context.Utilizadores.Where(u => u.Id == id).ExecuteUpdateAsync(set => set.SetProperty(p => p.adesao, true));

                var apagarPedido = await _context.Pedido.Where(u => u.ref_Utilizador == id).ExecuteDeleteAsync();

                resposta.Dados = utilizador.nome;
                resposta.Status = true;
                resposta.Mensagem = $"Adesao do utilizador {utilizador.nome} aceite com sucesso e pedido removido";
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Status = false;
                resposta.Mensagem = $"Erro ao aceitar pedido: {ex.Message}";

            }
            return resposta;
        }
    }
}