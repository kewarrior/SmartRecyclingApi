namespace SmartRecyclingApi.ViewModels.Utilizador
{
    public class UtilizadorCriacaoDTO
    {
        public long? Id { get; set; }
        public string nome { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public string? morada { get;set;  }
        public string codigopostal { get; set; }
        public int? telefone { get; set; }
        public DateTime? data_nascimento {  get; set; }
        public string? role { get; set; }
    }
}
