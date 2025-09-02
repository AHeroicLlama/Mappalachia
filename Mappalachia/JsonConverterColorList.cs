using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mappalachia
{
	public class JsonConverterColorList : JsonConverter<List<Color>>
	{
		public override List<Color> Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions serializerOptions)
		{
			List<Color> colors = new List<Color>();

			while (reader.Read())
			{
				if (reader.TokenType == JsonTokenType.EndArray)
				{
					break;
				}

				if (reader.TokenType == JsonTokenType.String)
				{
					colors.Add(reader.GetString().ToColor());
				}
			}

			return colors;
		}

		public override void Write(Utf8JsonWriter writer, List<Color> colors, JsonSerializerOptions serializerOptions)
		{
			writer.WriteStartArray();

			foreach (Color color in colors)
			{
				writer.WriteStringValue(ColorTranslator.ToHtml(color));
			}

			writer.WriteEndArray();
		}
	}
}
