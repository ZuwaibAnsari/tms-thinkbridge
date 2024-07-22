using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMSMVC.Models
{
    public class ModelBase
    {
        [Key]
        public string Id { get; set; }

        //[NotMapped]
        //public ModelMetadata Metadata { get; set; } = new();
    }
}
