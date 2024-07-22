using System.ComponentModel.DataAnnotations;

namespace TMSMVC.Models
{
    public class Company : ModelBase
    {
        public string Name { get; set; }

        public string? AdminID { get; set; }

        public string OwnerID { get; set; }

        public DateTime Created { get; set; }

        public string CreatedBy { get; set; }

        public DateTime Modified { get; set; }

        public string ModifiedBy { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
