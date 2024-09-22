using System.Text.Json.Serialization;
using System.Text.Json;
using Serilog;

namespace PartsOrderAPI.Validation
{
    public class CustomStringConverter : JsonConverter<string>
    {
        private static readonly Serilog.ILogger _logger = Log.ForContext<CustomStringConverter>();

        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                if (reader.TokenType == JsonTokenType.String)
                {
                    var value = reader.GetString();                    
                    return value;
                }
                else
                {
                    var errorMessage = "Error converting JSON token to string.";                    
                    throw new JsonException(errorMessage);
                }
            }
            catch (JsonException ex)
            {
                _logger.Error(ex, "JSON deserialization error: {Message}", ex.Message);
                throw new JsonException(ex.Message, ex);
            }
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            _logger.Information("Writing string value to JSON: {Value}", value);
            writer.WriteStartObject();
            writer.WriteString("Name", value);
            writer.WriteEndObject();
        }
    }
}
