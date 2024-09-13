using MappalachiaLibrary;

namespace Preprocessor
{
	internal partial class Preprocessor
	{
		static List<string> ValidationFailures { get; } = new List<string>();

		static void FailValidation(string reason)
		{
			ConsoleColor originalColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;

			Console.WriteLine($"Validation failure: {reason}");
			ValidationFailures.Add(reason);

			Console.ForegroundColor = originalColor;

			File.AppendAllText(BuildPaths.GetErrorsPath(), reason);
		}
	}
}
