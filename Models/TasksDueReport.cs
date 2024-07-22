using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TMSMVC.Models
{
    [Keyless]
    public class TasksDueReport
    {
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int DueInWeek { get; set; }
        public int DueInMonth { get; set; }

    }
}
