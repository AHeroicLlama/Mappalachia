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

			File.AppendAllLines(BuildPaths.GetErrorsPath(), new List<string>() { reason });
		}

		static void ConcludeValidation()
		{
			Console.WriteLine();
			ConsoleColor originalColor = Console.ForegroundColor;

			if (ValidationFailures.Count > 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;

				Console.WriteLine("Validation failed. The following errors were reported:");

				foreach (string failure in ValidationFailures)
				{
					Console.WriteLine($"* {failure}");
				}

				Console.WriteLine($"\nError details stored to {BuildPaths.GetErrorsPath()}");
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("Validation passed!");
			}

			Console.ForegroundColor = originalColor;
			Console.WriteLine();
		}
	}
}
