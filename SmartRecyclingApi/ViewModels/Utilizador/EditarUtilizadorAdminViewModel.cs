namespace SmartRecyclingApi.ViewModels.Utilizador
{
    public class EditarUtilizadorAdminViewModel
    {
        public long Id { get; set; }
        public string nome { get; set; }
        public string? morada { get; set; }
        public string? codigo_postal { get; set; }
        public int? telefone { get; set; }  
        public DateTime? data_nascimento { get; set; }
    }
}
