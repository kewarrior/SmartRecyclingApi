using SmartRecyclingApi.Data;

namespace SmartRecyclingApi.Services.Operador
{
    public class OperadorService : IOperadorInterface
    {
        private readonly AppDbContext _context;

        public OperadorService(AppDbContext context)
        {
            _context = context;
        }



    }
}
