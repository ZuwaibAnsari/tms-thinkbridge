using System.Text.Json.Serialization; 

namespace TMSMVC.Models
{
    /// <summary>
    /// Represents the priorities of a Task <br />
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TaskPriorities
    {
        Select = 0,

        High = 10,

        Medium = 20,
 
        Low = 30,

    }
}