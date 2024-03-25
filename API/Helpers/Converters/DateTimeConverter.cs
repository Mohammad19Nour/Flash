using Newtonsoft.Json.Converters;

namespace ProjectP.Helpers.Converters;

public class DateTimeConverter : IsoDateTimeConverter
{
    public DateTimeConverter()
    {
        base.DateTimeFormat = "yyyy-MM-dd HH:mm";
    }
}