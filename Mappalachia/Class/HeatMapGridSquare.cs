using System.Drawing;

namespace Mappalachia.Class
{
	//A single square on a heatmap, storing the 'weighting' for each color
	class HeatMapGridSquare
	{
		public double[] weights;

		public HeatMapGridSquare()
		{
			weights = new double[2]; //[0] is red weight, [1] is blue weight
		}

		public double GetTotalWeight()
		{
			return weights[0] + weights[1];
		}

		public Color GetColor(double largestWeight)
		{
			double localSum = GetTotalWeight();
			double alpha = localSum / largestWeight * 255;
			double redValue = weights[0] / localSum * 255;
			double blueValue = weights[1] / localSum * 255;

			return (largestWeight == 0 || localSum == 0) ?
				Color.FromArgb(0, 0, 0, 0) :
				Color.FromArgb((int)alpha, (int)redValue, 0, (int)blueValue);
		}
	}
}
