using System.Drawing;

namespace Mappalachia
{
	static class SettingsPlotCluster
	{
		// Cluster mode tuning
		public const int polygonLineThickness = 3;
		public const int webLineThickness = 1;
		public const int fontSize = 55;
		public const int minFontSize = 40;
		public static readonly Brush weightBrush = new SolidBrush(Color.FromArgb(170, Color.White)); // Brush used to paint weight text

		public const int minRange = 1;
		public const int maxRange = 2000;
		public const int minWeightCap = 0;
		public const int maxWeightCap = 200;

		public const bool defaultClusterWeb = false;
		public const bool defaultLiveUpdate = true;
		public const int defaultClusterRange = 100;
		public const int defaultClusterWeightCap = 3;

		public static bool clusterWeb = defaultClusterWeb;
		public static bool liveUpdate = defaultLiveUpdate;
		public static int clusterRange = defaultClusterRange;
		public static int minClusterWeight = defaultClusterWeightCap;
	}
}
