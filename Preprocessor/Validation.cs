using Library;

namespace Preprocessor
{
	internal partial class Preprocessor
	{
		static List<string> ValidationFailures { get; } = new List<string>();

		static void FailValidation(string reason)
		{
			BuildTools.StdOutWithColor($"Validation failure: {reason}", ConsoleColor.Red);
			ValidationFailures.Add(reason);

			File.AppendAllLines(BuildTools.ErrorsPath, new List<string>() { reason });
		}

		static void ConcludeValidation()
		{
			Console.WriteLine();

			if (ValidationFailures.Count > 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;

				Console.WriteLine($"Validation failed. The following {ValidationFailures.Count} error(s) were reported:");

				foreach (string failure in ValidationFailures)
				{
					Console.WriteLine($"* {failure}");
				}

				Console.WriteLine($"\nError details stored to {BuildTools.ErrorsPath}");

				Console.ResetColor();
			}
			else
			{
				BuildTools.StdOutWithColor("Validation passed!", ConsoleColor.Green);
			}
		}
	}
}
