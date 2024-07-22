using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TMSMVC.Models
{
    public class TaskItem : ModelBase
    {
        public string Title { get; set; }

        public string? Description { get; set; }

        public TaskStatuses Status { get; set; } = TaskStatuses.Draft;

        [DisplayName("Assign to Department")]
        public string? AssignedToDepartmentID { get; set; }

        [DisplayName("Assign to User")]
        public string? AssignedToUserID { get; set; }

        public DateTime? ExpectedCompletionDate { get; set; }

        public TaskPriorities Priority { get; set; } = TaskPriorities.Medium;

        public string OwnerID { get; set; }

        public DateTime Created { get; set; }

        public string CreatedBy { get; set; }

        public DateTime Modified { get; set; }

        public string ModifiedBy { get; set; }

        public bool IsActive { get; set; } = true;

    }
}
