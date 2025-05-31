using System.ComponentModel;

namespace Billi.Backend.CrossCutting.Enums
{
    public enum StatusType
    {
        [Description("Inactive")]
        Inactive = 0,

        [Description("Active")]
        Active = 1
    }
}