using System.Text.Json.Serialization;

namespace TMSMVC.Models
{
    /// <summary>
    /// Represents the statuses of a Task <br />
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TaskStatuses
    {
        Select = 0,

        Draft = 10,

        InProgress = 20,

        Done = 30,

    }
}