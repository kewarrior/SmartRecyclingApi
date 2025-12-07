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
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var response = await _utilizadorInterface.Login(request);

            if (!response.Status || response.Dados == null)
            {
                return Unauthorized(new { response.Mensagem });
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddSeconds(response.Dados.Expira),
                Secure = true, 
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append("accessToken", response.Dados.AcessoToken, cookieOptions);

            return Ok(new 
            { 
                Mensagem = "Login com sucesso. O token foi definido no cookie." ,
                Status = true
            });
        }

        [Authorize]
        [HttpGet("UtilizadorInfo")]
        public IActionResult GetUserInfo()
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var nome = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            return Ok(new
            {
                Email = email,
                Nome = nome,
                Role = role
            });
        }
    }
}
