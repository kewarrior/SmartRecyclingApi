using SmartRecyclingApi.Enums;

namespace SmartRecyclingApi.ViewModels.Pedido
{
    public class InserirPedidoDto
    {
        public long ref_Utilizador { get; set; }
        public string? Tipo_Pedido { get; set; }
        public EnumStatusPedido Status_Pedido { get; set; }
        public DateTime? Data_Criacao { get; set; } = DateTime.Now;
    }
}
