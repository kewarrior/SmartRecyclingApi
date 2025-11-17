using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartRecyclingApi.Models;
using SmartRecyclingApi.Services.Administrador;
using SmartRecyclingApi.ViewModels.Utilizador;

namespace SmartRecyclingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministradorController : ControllerBase
    {

        private readonly IAdminInterface _administradorInterface;

        public AdministradorController(IAdminInterface administradorInterface)
        {
            _administradorInterface = administradorInterface;
        }


        [HttpPost("AceitarPedido")]
        public async Task<ActionResult<ResponseModel<object>>> AceitarPedido(long id)
        {
            var update = await _administradorInterface.UpdatePedido(id);
            return Ok(update);
        }


        [HttpPost("ValidarAdministrador")]
        public async Task<ActionResult<ResponseModel<object>>> ValidarAdministrador(UtilizadorCriacaoDTO verificarUtilizador)
        {
            var validacao = await _administradorInterface.ValidarPassword(verificarUtilizador);
            return Ok(validacao);
        }

        [HttpGet("Listar")]
        public async Task<ActionResult<ResponseModel<UtilizadorModel>>> Listar()
        {
            var dados = await _administradorInterface.Listar();
            return Ok(dados);
        }

        [HttpPut("EditarUtilizador")]
        public async Task<ActionResult<ResponseModel<UtilizadorModel>>> Editar(UtilizadorCriacaoDTO utilizador)
        {
            var editar = await _administradorInterface.EditarUtilizador(utilizador);
            return Ok(editar);
        }

        [HttpDelete("ApagarUtilizador")]
        public async Task<ActionResult<ResponseModel<UtilizadorModel>>> ApagarUtilizador(long id)
        {
            var utilizador = await _administradorInterface.ApagarUtilizador(id);
            return Ok(utilizador);
        }

        [HttpPost("CriarAdmin")]
        public async Task<ActionResult<ResponseModel<UtilizadorModel>>> CriarAdmin(UtilizadorCriacaoDTO admin)
        {
            var adminCriacao = await _administradorInterface.CriarADmin(admin);
            return Ok(adminCriacao);
        }
    }
}
