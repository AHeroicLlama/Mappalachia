using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mappalachia
{
	public class JsonConverterColor : JsonConverter<Color>
	{
		public override Color Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions serializerOptions)
		{
			return reader.GetString().ToColor();
		}

		public override void Write(Utf8JsonWriter writer, Color color, JsonSerializerOptions serializerOptions)
		{
			writer.WriteStringValue(ColorTranslator.ToHtml(color));
		}
	}
}
