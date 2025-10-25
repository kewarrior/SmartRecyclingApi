using Microsoft.EntityFrameworkCore;
using SmartRecyclingApi.Data;
using SmartRecyclingApi.Models;

namespace SmartRecyclingApi.Services.Utilizador
{
    public class UtilizadorService : IUtilizadorInterface
    {
        private readonly AppDbContext _context;
        public UtilizadorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<List<UtilizadorModel>>> GetUtilizadores()
        {
            ResponseModel<List<UtilizadorModel>> resposta = new ResponseModel<List<UtilizadorModel>>();
            try
            {
                var utilizadores = await _context.Utilizadores.ToListAsync();

                resposta.Dados = utilizadores;
                resposta.Mensagem = "Todos os utilizadores foram carregados!";
                return resposta;

            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }

        }

        public async Task<ResponseModel<UtilizadorModel>> GetUtilizarbyId(int idUtilizador)
        {
            ResponseModel<UtilizadorModel> resposta = new ResponseModel<UtilizadorModel>();

            try
            {
                var utilizador = await _context.Utilizadores.FirstOrDefaultAsync(utilizadorBanco => utilizadorBanco.Id == idUtilizador);

                if (utilizador == null)
                {
                    resposta.Mensagem = "Não existe nenhum utilizador com esse ID";
                    return resposta;
                }

                resposta.Dados = utilizador;
                resposta.Mensagem = "Foi encontrado o utilizador";
                return resposta;

            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }

        }
    }
}
