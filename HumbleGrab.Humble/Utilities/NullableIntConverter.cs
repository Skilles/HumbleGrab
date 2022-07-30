using System.Text.Json;
using System.Text.Json.Serialization;

namespace HumbleGrab.Humble.Utilities;

public class NullableIntConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return 0;
        }

        return JsonSerializer.Deserialize<int>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options) 
        => JsonSerializer.Serialize(writer, value, options);
}