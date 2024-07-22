using Microsoft.AspNetCore.Identity;

namespace TMSMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        public bool IsSuperAdmin { get; set; }

        public string? CompanyId { get; set; }

        public string? DepartmentId { get; set; }
    }
}
