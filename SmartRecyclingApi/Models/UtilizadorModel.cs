namespace SmartRecyclingApi.Models
{
    public class UtilizadorModel
    {
        public long Id { get; set; }
        public string nome { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public string? morada { get; set; }
        public string? codigo_postal { get; set; }
        public int? telefone {  get; set; }
        public int? pontos { get; set; }
        public bool? status { get; set; }
        public bool? adesao { get; set; }
        public DateTime? data_nascimento { get; set; }

        public string Role { get; set; }


        public virtual ICollection<ReciclagemModel>? Reciclagem { get; set; }
        public virtual ICollection<PedidoModel>? Pedido { get; set; }

    }
}
