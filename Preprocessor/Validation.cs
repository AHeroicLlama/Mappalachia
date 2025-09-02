using static Library.BuildTools;

namespace Preprocessor
{
	internal partial class Preprocessor
	{
		static List<string> ValidationFailures { get; } = new List<string>();

		static bool FailValidation(string reason)
		{
			StdOutWithColor($"Validation failure: {reason}", ColorError);
			ValidationFailures.Add(reason);

			AppendToErrorLog(reason);

			return false;
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

				Console.WriteLine($"\nError details stored to {ErrorsPath}");

				Console.ResetColor();
			}
			else
			{
				StdOutWithColor("Validation passed!", ConsoleColor.Green);
			}
		}
	}
}
