using Newtonsoft.Json;

namespace ProjectP.Helpers.Converters;

public class RoundedNumberConverter : JsonConverter<decimal>
{
    private readonly int _decimalPlaces;

    public RoundedNumberConverter(int decimalPlaces)
    {
        _decimalPlaces = decimalPlaces;
    }

    public override void WriteJson(JsonWriter writer, decimal value, JsonSerializer serializer)
    {
        writer.WriteValue(Math.Round(value, _decimalPlaces));
    }

    public override decimal ReadJson(JsonReader reader, Type objectType, decimal existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Float || reader.TokenType == JsonToken.Integer)
        {
            var originalValue = reader.Value;
            decimal roundedValue = Math.Round(Convert.ToDecimal(originalValue), _decimalPlaces);
            return roundedValue;
        }

        throw new JsonException($"Unable to convert JSON token type '{reader.TokenType}' to double.");
    }

    /*public bool CanConvert(Type objectType)
    {
        return objectType == typeof(decimal) || objectType == typeof(double) || objectType == typeof(float) ||
               objectType == typeof(int);
    }*/
}