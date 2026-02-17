using Microsoft.EntityFrameworkCore;
using SmartRecyclingApi.Data;
using SmartRecyclingApi.Models;
using SmartRecyclingApi.ViewModels.Operador;


namespace SmartRecyclingApi.Services.Operador
{
    public class OperadorService : IOperadorInterface
    {
        private readonly AppDbContext _context;

        public OperadorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<ReciclagemModel>> InserirDadosReciclagem(ReciclagemModel reciclagem)
        {
            ResponseModel<ReciclagemModel> resposta = new ResponseModel<ReciclagemModel>();

            if (reciclagem.ref_Utilizador == 0)
            {
                resposta.Mensagem = "Id do utilizador é obrigatorio";
                resposta.Status = false;
                return resposta;
            }

            var utilizadorExistente = await _context.Utilizadores.FindAsync(reciclagem.ref_Utilizador);

            if (utilizadorExistente == null)
            {
                resposta.Mensagem = "Utilizador não existente.";
                resposta.Status = false;
                return resposta;
            }

            try
            {
                var InserirDados = new ReciclagemModel()
                {
                    ref_Utilizador = reciclagem.ref_Utilizador,
                    MatPapel = reciclagem.MatPapel,
                    MatPlastico = reciclagem.MatPlastico,
                    MatVidro = reciclagem.MatVidro,
                    data_Reciclagem = DateTime.Now
                };

                _context.Add(InserirDados);
                await _context.SaveChangesAsync();

                resposta.Mensagem = "Dados Inseridos Com sucesso";
                resposta.Status = true;

            }
            catch (Exception ex)
            {
                resposta.Mensagem = ex.Message;
                resposta.Status = false;
                return resposta;
            }
            return resposta;

        }

        public async Task<ResponseModel<string>> GetNomeOperador(long id)
        {
            ResponseModel<string> resposta = new ResponseModel<string>();

            try
            {
                var utilizadorOperador = await _context.Utilizadores
                    .AsNoTracking()
                    .Where(o => o.Id == id)
                    .Select(o => o.nome)
                    .FirstOrDefaultAsync();

                if (string.IsNullOrEmpty(utilizadorOperador))
                {
                    resposta.Mensagem = "Nenhum utilizador com este ID";
                    resposta.Status = false;
                    return resposta;
                }

                resposta.Dados = utilizadorOperador;
                resposta.Status = true;
                resposta.Mensagem = "Nome do utilizador encontrado";
            }
            catch (Exception ex)
            {
                resposta.Mensagem = $"Erro ao obter nome do utilizador pelo ID {id}: {ex.Message}";
                resposta.Status = false;
            }
            return resposta;
        }

        public async Task<ResponseModel<List<OperUtilizadorView>>> GetUtilizadores()
        {
            ResponseModel<List<OperUtilizadorView>> resposta = new ResponseModel<List<OperUtilizadorView>>();
            try
            {
                var utilizadores = await _context.Utilizadores.Where(ut => ut.Role == "Utilizador").Select(ut => new OperUtilizadorView
                {
                    Id = ut.Id,
                    Nome = ut.nome,
                }).ToListAsync();

                if (utilizadores.Count == 0)
                {
                    resposta.Status = false;
                    resposta.Mensagem = "Sem utilizadores";
                }

                resposta.Dados = utilizadores;
                resposta.Mensagem = $"Foram encontrados : {utilizadores.Count}";
                resposta.Status = true;
                return resposta;
            }
            catch (Exception ex)
            {
                resposta.Status = false;
                resposta.Mensagem = $"Erro desconhecido: {ex.Message}";
                return resposta;
            }
        }
    }
}
