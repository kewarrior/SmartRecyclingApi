namespace SmartRecyclingApi.ViewModels.Utilizador
{
    public class UtilizadorCriacaoDTO
    {
        public long? Id { get; set; }
        public string nome { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public string? role { get; set; }
    }
}
