using Newtonsoft.Json;

namespace ProjectP.Helpers.Converters;

public class PictureUrlConverter : JsonConverter
{
    private readonly string _baseUrl;

    public PictureUrlConverter(string baseUrl)
    {
        _baseUrl = baseUrl;
    }

    public override bool CanConvert(Type objectType)
    {
        // This converter handles only strings
        return objectType == typeof(string);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        return reader.Value;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
       
        if (writer.Path.ToLower().EndsWith("url"))
        {
            // Concatenate base URL with the picture URL
            writer.WriteValue(_baseUrl + value.ToString());
        }
        else
        {
            // If the property name is not "PictureUrl", write the value as is
            writer.WriteValue(value);
        }
    }
}