using System.ComponentModel.DataAnnotations;

namespace TMSMVC.Models
{
    public class Department : ModelBase
    {

        public string Name { get; set; }

        public string CompanyID { get; set; }

        public string? AdminID { get; set; }

        public string OwnerID { get; set; }

        public DateTime Created { get; set; }

        public string CreatedBy { get; set; }

        public DateTime Modified { get; set; }

        public string ModifiedBy { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
