namespace SmartRecyclingApi.ViewModels.Utilizador
{
    public class EditarUtilizadorDto
    {
        public long Id { get; set; }
        public string nome { get; set; }
        public string? email { get; set; }
        public string? password { get; set; }
        public string? morada { get; set; }
        public string? codigo_postal { get; set; }
        public int? telefone { get; set; }
    }
}
