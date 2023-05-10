using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Cinema.Enum
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FilmStatus
    {
        NOSCHEDULED,
        SCHEDULED,
        AVAILABLE,
        DELETED,
    }
}
