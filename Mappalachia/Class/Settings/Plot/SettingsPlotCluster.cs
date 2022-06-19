using System.Drawing;

namespace Mappalachia
{
	static class SettingsPlotCluster
	{
		// Cluster mode tuning
		public static readonly int refinementMaxIterations = 50; // Hard cap to stop clustering iterations in Precise mode after this many times
		public static readonly float minimumPolygonArea = 250; // The minimum area px^2 that a cluster polygon must fill or else uses a centroid-centered bounding circle
		public static readonly int polygonLineThickness = 4;
		public static readonly int boundingCircleMinRadius = 15; // Clusters given a bounding circle are rendered at least this big
		public static readonly int polygonPointReductionRange = 5; // Points in the cluster convex hull this close together are merged
		public static readonly int minimumAngle = 10; // A cluster convex hull with an interior angle tighter than this are dropped as too 'pointy' and fall back to the circle
		public static readonly Brush weightBrush = new SolidBrush(Color.FromArgb(200, Color.White)); // Brush used to paint weight text

		public static readonly int minRange = boundingCircleMinRadius * 2;
		public static readonly int maxRange = 300;

		public static readonly bool defaultLiveUpdate = false;
		public static readonly int defaultClusterRange = 100;

		public static bool liveUpdate = defaultLiveUpdate;
		public static int clusterRange = defaultClusterRange;
	}
}
