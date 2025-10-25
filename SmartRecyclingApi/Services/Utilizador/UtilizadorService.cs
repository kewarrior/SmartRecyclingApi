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

        public Task<ResponseModel<UtilizadorModel>> GetUtilizarbyId(int idUtilizador)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<List<UtilizadorModel>>> GetUtilizadores()
        {
            ResponseModel<List<UtilizadorModel>> resposta = new ResponseModel<List<UtilizadorModel>>();
            try
            {
                var autores = await _context.Utilizadores.ToListAsync();

                resposta.Dados = autores;
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


    }
}
