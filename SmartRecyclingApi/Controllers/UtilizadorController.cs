using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartRecyclingApi.Models;
using SmartRecyclingApi.Services.Utilizador;
using SmartRecyclingApi.ViewModels.Utilizador;

namespace SmartRecyclingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UtilizadorController : ControllerBase
    {
        private readonly IUtilizadorInterface _utilizadorInterface;

        public UtilizadorController(IUtilizadorInterface utilizadorInterface)
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
    
            var resposta = await _utilizadorInterface.CriarUtilizador(utilizadorCriacaoDto);

            if (resposta.Status == false)
            {
                return BadRequest(resposta);
            }
            return Ok(resposta);
        }

        [HttpPut("EditarUtilizador")]
        public async Task<ActionResult<ResponseModel<UtilizadorModel>>> EditarUtilizador(EditarUtilizadorDto editarUtilizadorDto)
        {
            var editarUtilizador = await _utilizadorInterface.EditarUtilizador(editarUtilizadorDto);
            return Ok(editarUtilizador);
        }


        [AllowAnonymous]
        [HttpPost("Login")]

        public async Task<ActionResult<ResponseModel<LoginResponseModel>>> Login(LoginRequest request)
        {
            var result = await _utilizadorInterface.Login(request);
            if (result.Status == false) 
            {
                return Unauthorized();
            }
            return Ok(result);

        }
    }
}
