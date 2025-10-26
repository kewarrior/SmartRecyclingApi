
using Microsoft.AspNetCore.Mvc;
using SmartRecyclingApi.Models;
using SmartRecyclingApi.Services.Utilizador;
using SmartRecyclingApi.ViewModels.Utilizador;

namespace SmartRecyclingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizadorController : ControllerBase
    {
        private readonly IUtilizadorInterface _utilizadorInterface;

        public UtilizadorController (IUtilizadorInterface utilizadorInterface) 
        {
            _utilizadorInterface = utilizadorInterface;
        }


        [HttpGet("GetUtilizador")]
        public async Task<ActionResult<ResponseModel<List<UtilizadorModel>>>> GetUtilizadores()
        {
            var utilizadores = await _utilizadorInterface.GetUtilizadores();
            return Ok(utilizadores);
        }

        [HttpGet("GetUtilizadorbyID/{idUtilizador}")]
        public async Task<ActionResult<ResponseModel<UtilizadorModel>>> GetUtilizarbyId(int idUtilizador)
        {
            var utilizador = await _utilizadorInterface.GetUtilizarbyId(idUtilizador);
            return Ok(utilizador);
        }

        [HttpPost("CriarUtilizador")]
        public async Task<ActionResult<ResponseModel<UtilizadorModel>>> CriarUtilizador(UtilizadorCriacaoDTO utilizadorCriacaoDto)
        {
            var criarUtilizador = await _utilizadorInterface.CriarUtilizador(utilizadorCriacaoDto);
            return Ok(criarUtilizador);
        }

        [HttpPut("EditarUtilizador")]
        public async Task<ActionResult<ResponseModel<UtilizadorModel>>> EditarUtilizador(EditarUtilizadorDto editarUtilizadorDto)
        {
            var editarUtilizador = await _utilizadorInterface.EditarUtilizador(editarUtilizadorDto);
            return Ok (editarUtilizador);
        }

        [HttpGet("Login")]

        public async Task<ActionResult<ResponseModel<UtilizadorModel>>> Login(string email, string password)
        {
            var login = await _utilizadorInterface.Login( email,password);
            return Ok(login);
        }
    }
}
