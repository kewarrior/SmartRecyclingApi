namespace SmartRecyclingApi.ViewModels
{
    public class PedidoDTO
    {
        public long Id { get; set; }
        public long ref_Utilizador { get; set; }
        public string? Tipo_Pedido { get; set; }
        public string? Status_Pedido { get; set; }
        public DateTime? Data_Criacao { get; set; }
        public string NomeUtilizador { get; set; }
    }
}
