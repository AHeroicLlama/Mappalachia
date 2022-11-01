using System.Drawing;

namespace Mappalachia
{
	static class SettingsPlotCluster
	{
		// Cluster mode tuning
		public static readonly int polygonLineThickness = 3;
		public static readonly int webLineThickness = 1;
		public static readonly int fontSize = 55;
		public static readonly int minFontSize = 40;
		public static readonly Brush weightBrush = new SolidBrush(Color.FromArgb(170, Color.White)); // Brush used to paint weight text

		public static readonly int minRange = 1;
		public static readonly int maxRange = 2000;
		public static readonly int minWeightCap = 1;
		public static readonly int maxWeightCap = 200;

		public static readonly bool defaultClusterWeb = false;
		public static readonly bool defaultLiveUpdate = true;
		public static readonly int defaultClusterRange = 100;
		public static readonly int defaultClusterWeightCap = 3;

		public static bool clusterWeb = defaultClusterWeb;
		public static bool liveUpdate = defaultLiveUpdate;
		public static int clusterRange = defaultClusterRange;
		public static int minClusterWeight = defaultClusterWeightCap;
	}
}
