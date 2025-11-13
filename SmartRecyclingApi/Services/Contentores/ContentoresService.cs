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

        public async Task<ResponseModel<List<ReciclagemModel>>> GetDadosByUtilizadorId(long id)
        {
            ResponseModel<List<ReciclagemModel>> resposta = new ResponseModel<List<ReciclagemModel>>();

            try
            {
                var dados = _context.Reciclagem.Where(u => u.ref_Utilizador == id);

                if (dados == null)
                {
                    resposta.Status = false;
                    resposta.Mensagem = "Não foi possivel encontrados dados sobre esse utilizador";
                    return resposta;
                }

                if (dados != null)
                {
                    var formatarDados = await dados.Select(dados => new ReciclagemModel
                    {

                        ref_Utilizador = dados.ref_Utilizador,
                        MatPapel = dados.MatPapel,
                        MatPlastico = dados.MatPlastico,
                        MatVidro = dados.MatVidro

                    }).ToListAsync();

                    if (!formatarDados.Any())
                    {
                        resposta.Status = false;
                        resposta.Mensagem = "Não foi possivel encontrados dados sobre esse utilizador";
                        return resposta;
                    }
                    resposta.Status = true;
                    resposta.Mensagem = "Dados carregados com sucesso";
                    resposta.Dados = formatarDados;
                }

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
