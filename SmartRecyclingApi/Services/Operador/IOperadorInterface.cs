using SmartRecyclingApi.Models;
using SmartRecyclingApi.ViewModels.Operador;

namespace SmartRecyclingApi.Services.Operador
{
    public interface IOperadorInterface
    {
        Task<ResponseModel<ReciclagemModel>> InserirDadosReciclagem(ReciclagemModel reciclagem);
        Task<ResponseModel<string>> GetNomeOperador(long id);
        Task<ResponseModel<List<OperUtilizadorView>>> GetUtilizadores();
    }
}
