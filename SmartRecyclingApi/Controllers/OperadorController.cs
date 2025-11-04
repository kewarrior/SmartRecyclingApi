using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartRecyclingApi.Services.Operador;
using SmartRecyclingApi.Services.Pedido;

namespace SmartRecyclingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperadorController : ControllerBase
    {

        private readonly IOperadorInterface _operadorInterface;

        public OperadorController(IOperadorInterface operadorInterface)
        {
            _operadorInterface = operadorInterface;
        }



    }
}
