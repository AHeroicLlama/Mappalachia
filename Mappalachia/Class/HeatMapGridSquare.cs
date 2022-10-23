using System.Drawing;

namespace Mappalachia
{
	// A single square on a heatmap, storing the 'weighting' for each color
	class HeatMapGridSquare
	{
		public double[] weights;

		public HeatMapGridSquare()
		{
			weights = new double[2];
		}

		public double GetTotalWeight()
		{
			return weights[0] + weights[1];
		}

		public Color GetColor(double largestWeight)
		{
			double squareSumWeight = GetTotalWeight();

			// No plots - return transparent
			if (largestWeight == 0 || squareSumWeight == 0)
			{
				return Color.FromArgb(0, 0, 0, 0);
			}

			double alpha = squareSumWeight / largestWeight * 255;

			// Mono color mode - no need to interpolate just return first palette color
			if (SettingsPlotHeatmap.colorMode == SettingsPlotHeatmap.ColorMode.Mono)
			{
				return Color.FromArgb((int)alpha, SettingsPlotStyle.GetFirstColor());
			}

			// Otherwise, duo color mode - return an interpolated color based on assigned weights
			double increment = weights[0] / squareSumWeight; // Normalized distance between color one and two
			Color interpolatedColor = ImageHelper.LerpColors(SettingsPlotStyle.GetSecondColor(), SettingsPlotStyle.GetFirstColor(), increment);

			return Color.FromArgb((int)alpha, interpolatedColor);
		}
	}
}
