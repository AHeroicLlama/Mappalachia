using MappalachiaLibrary;
using System.Text.RegularExpressions;

namespace Preprocessor
{
	internal partial class Preprocessor
	{
		static Regex ValidateLockLevel { get; } = new Regex(@"^(Advanced \(Level 1\)|Chained|Expert \(Level 2\)|Inaccessible|Master \(Level 3\)|Novice \(Level 0\)|Requires Key|Requires Terminal|Unknown|Barred)$");
		static Regex ValidatePrimitiveShape { get; } = new Regex("^(Box|Line|Plane|Sphere|Ellipsoid)$");
		static Regex ValidateSignature { get; } = new Regex("^[A-Z_]{4}$");
		static Regex ValidateMapMarkerIcon { get; } = new Regex("^(WhitespringResort|NukaColaQuantumPlant|TrainTrackMark|.*Marker)$");
		static Regex ValidateNpcClass { get; } = new Regex("^(Main|Sub|Critter[AB])$");
		static Regex ValidateComponent { get; } = new Regex("^(Acid|Adhesive|Aluminum|Antiseptic|Asbestos|Ballistic Fiber|Black Titanium|Bone|Ceramic|Circuitry|Cloth|Concrete|Copper|Cork|Crystal|Fertilizer|Fiber Optics|Fiberglass|Gear|Glass|Gold|Gunpowder|Lead|Leather|Nuclear Material|Oil|Plastic|Rubber|Screw|Silver|Spring|Steel|Ultracite|Wood)$");

		static List<string> ValidationFailures { get; } = new List<string>();

		static void FailValidation(string reason)
		{
			ConsoleColor originalColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;

			Console.WriteLine($"Validation failure: {reason}");
			ValidationFailures.Add(reason);

			Console.ForegroundColor = originalColor;

			File.AppendAllLines(BuildPaths.GetErrorsPath(), new List<string>() { reason });
		}
	}
}
