using System.Runtime.Serialization;

namespace Cinema.Enum
{
    public enum AccountStatus
    {
        [EnumMember(Value = "ACTIVATED")]
        ACTIVATED,
        [EnumMember(Value = "LOCKED")]
        LOCKED,
        [EnumMember(Value = "DELETED")]
        DELETED,
    }
}
