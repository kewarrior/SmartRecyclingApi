using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartRecyclingApi.Models;
using SmartRecyclingApi.Services.Utilizador;

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


        [HttpGet("GettUtilizador")]

        public async Task<ActionResult<ResponseModel<List<UtilizadorModel>>>> GetUtilizadores()
        {
            var utilizadores = await _utilizadorInterface.GetUtilizadores();
            return Ok(utilizadores);
        }
    }
}
