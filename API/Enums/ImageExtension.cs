using System.Runtime.Serialization;

namespace ProjectP.Enums;

public enum ImageExtension
{
    [EnumMember(Value = ".jpg")]
    JPG,
    [EnumMember(Value = ".jpeg")]
    JPEG,
    [EnumMember(Value = ".png")]
    PNG,
    [EnumMember(Value = ".gif")]
    GIF,
}