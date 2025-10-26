using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRecyclingApi.Models
{
    public class PedidoModel
    {
        public long Id { get; set; }
        public long ref_Utilizador { get; set; }
        public string? Tipo_Pedido { get; set; }
        public string? Status_Pedido { get; set; }
        public DateTime? Data_Criacao { get; set; }


        [ForeignKey("ref_Utilizador")]
        public virtual UtilizadorModel Utilizador {get;set;}
    }
}
