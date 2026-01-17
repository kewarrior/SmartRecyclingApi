namespace SmartRecyclingApi.ViewModels.Utilizador
{
    public class EditarUtilizadorAdminViewModel
    {
        public long Id { get; set; }
        public string nome { get; set; }
        public string? morada { get; set; }
        public string? codigoPostal { get; set; }
        public int? telefone { get; set; }  
        public DateTime? dataNascimento { get; set; }
    }
}
