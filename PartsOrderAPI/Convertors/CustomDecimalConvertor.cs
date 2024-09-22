using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Serilog;

namespace PartsOrderAPI.Validation
{
    public class CustomDecimalConverter : JsonConverter<decimal>
    {
        private static readonly Serilog.ILogger _logger = Log.ForContext<CustomDecimalConverter>();

        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                if (reader.TokenType == JsonTokenType.Number && reader.TryGetDecimal(out decimal value))
                {
                    
                    return value;
                }
                else
                {
                    var errorMessage = "Error converting JSON token to decimal.";                    
                    throw new JsonException(errorMessage);
                }
            }
            catch (JsonException ex)
            {
                _logger.Error(ex, "JSON deserialization error: {Message}", ex.Message);
                throw new JsonException(ex.Message, ex);
            }
        }

        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        {
            
            writer.WriteNumberValue(value);
        }
    }
}
