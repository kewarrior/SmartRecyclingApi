using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartRecyclingApi.Models;
using SmartRecyclingApi.Services.Contentores;

namespace SmartRecyclingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentoresController : ControllerBase
    {
        private readonly IContentoresInterface _contentoresInterface;

        public ContentoresController(IContentoresInterface contentoresInterface)
        {
            _contentoresInterface = contentoresInterface;
        }

        [HttpGet("GetAllDadosReciclagem")]
        public async Task<ActionResult<ResponseModel<ReciclagemModel>>> GetAllDadosReciclagem()
        {
            var dados = await _contentoresInterface.GellAllDadosReciclagem();
            return Ok(dados);
        }

        [HttpGet("GetDadosUtilizador")]
        public async Task<ActionResult<ResponseModel<ReciclagemModel>>> GetDadosReciclagemUtilizador(long id)
        {
            var dados = await _contentoresInterface.GetDadosByUtilizadorId(id);
            return Ok(dados);
        }
    }
}
