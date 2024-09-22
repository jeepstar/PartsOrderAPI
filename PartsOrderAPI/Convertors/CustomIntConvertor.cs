using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Serilog;

namespace PartsOrderAPI.Validation
{
    public class CustomIntConverter : JsonConverter<int>
    {
        private static readonly Serilog.ILogger _logger = Log.ForContext<CustomIntConverter>();

        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out int value))
                {                    
                    return value;
                }
                else
                {
                    var errorMessage = "Error converting JSON token to int.";                    
                    throw new JsonException(errorMessage);
                }
            }
            catch (JsonException ex)
            {
                _logger.Error(ex, "JSON deserialization error: {Message}", ex.Message);
                throw new JsonException(ex.Message, ex);
            }
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            _logger.Information("Writing int value to JSON: {Value}", value);
            writer.WriteNumberValue(value);
        }
    }
}
