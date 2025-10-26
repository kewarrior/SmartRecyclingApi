using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRecyclingApi.Models
{
    public class ReciclagemModel
    {
        public int Id { get; set; }
        public long ref_Utilizador { get; set; }
        public double? MatVidro { get; set; }
        public double? MatPlastico { get; set; }
        public double? MatPapel { get; set; }
        public DateTime? data_Reciclagem { get; set; }


        [ForeignKey("ref_Utilizador")]
        public virtual UtilizadorModel Utilizador { get; set; }
    }
}
