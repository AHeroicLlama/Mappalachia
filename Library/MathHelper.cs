namespace Library
{
	public static class MathHelper
	{
		public static double StdDev(this IEnumerable<double> values)
		{
			double mean = values.Average();
			double variance = values.Average(v => Math.Pow(v - mean, 2));
			return Math.Sqrt(variance);
		}
	}
}
