using Microsoft.EntityFrameworkCore;
using SmartRecyclingApi.Data;
using SmartRecyclingApi.Models;


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
                return resposta;
            }

            var utilizadorExistente = await _context.Utilizadores.FindAsync(reciclagem.ref_Utilizador);

            if (utilizadorExistente == null)
            {
                resposta.Mensagem = "Utilizador não existente.";
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
            }
            return resposta;

        }

        public async Task<ResponseModel<UtilizadorModel>> GetNomeOperador(long id)
        {
            ResponseModel<UtilizadorModel> resposta = new ResponseModel<UtilizadorModel>();

            try
            {
                var utilizadorOperador = await _context.Utilizadores.FirstOrDefaultAsync(o => o.Id == id);

                if(utilizadorOperador == null)
                {
                    resposta.Mensagem = "Nenhum utilizador com este ID";
                    return resposta;
                }

                resposta.Dados = utilizadorOperador;
                resposta.Status = true;
                resposta.Mensagem = "Utilizador encontrado";

            }catch(Exception ex)
            {
                resposta.Mensagem = $"Erro ao obter utilizador pelo ID {id}";
                resposta.Status = false;
            }
            return resposta;
        }


    }
}
