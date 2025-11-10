using Microsoft.EntityFrameworkCore;
using SmartRecyclingApi.Data;
using SmartRecyclingApi.Models;

namespace SmartRecyclingApi.Services.Contentores
{
    public class ContentoresService : IContentoresInterface
    {
        private readonly AppDbContext _context;

        public ContentoresService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseModel<List<ReciclagemModel>>> GellAllDadosReciclagem()
        {
            ResponseModel<List<ReciclagemModel>> resposta = new ResponseModel<List<ReciclagemModel>>();
            try
            {
                var dados = await _context.Reciclagem.Select(u => new ReciclagemModel
                {
                    ref_Utilizador = u.ref_Utilizador,
                    MatPapel = u.MatPapel,
                    MatVidro = u.MatVidro,
                    MatPlastico = u.MatPlastico,
                    data_Reciclagem = u.data_Reciclagem,
                })
                .ToListAsync();

                resposta.Dados = dados;
                resposta.Status = true;
                resposta.Mensagem = "Dados recolhidos com sucesso";
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
