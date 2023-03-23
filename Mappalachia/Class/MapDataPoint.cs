using System.Drawing;

namespace Mappalachia
{
	// A single data point to be mapped (one totally unique instance of an object)
	public class MapDataPoint
	{
		public float x;
		public float y;
		public int z; // Height
		public float weight; // The magnitude/importance of this plot (EG 2.0 may represent 2x scrap from a single junk, or 0.33 may represent a 33% chance of spawning)
		public string primitiveShape; // The name of the primitive shape which describes this item (only typically applicable to ACTI)
		public float boundX; // The bounds of the primitiveShape
		public float boundY;
		public int boundZ;
		public int rotationZ;
		public string instanceFormID; // The Form ID of the instance (NOT the FormID of the entity it places)
		MapCluster parentCluster; // Cluster this belongs to

		public MapDataPoint(int x, int y, int z, string instanceFormID = "00000000")
		{
			Initialize(x, y, z);
			this.instanceFormID = instanceFormID;
		}

		public MapDataPoint(int x, int y, int z, string primitiveShape, int boundX, int boundY, int boundZ, int rotationZ, string instanceFormID = "00000000")
		{
			Initialize(x, y, z);

			this.primitiveShape = primitiveShape;
			this.boundX = boundX;
			this.boundY = boundY;
			this.boundZ = boundZ;
			this.rotationZ = rotationZ;
			this.instanceFormID = instanceFormID;
		}

		void Initialize(int x, int y, int z)
		{
			this.x = Map.CorrectAxis(x, false);
			this.y = Map.CorrectAxis(y, true);
			this.z = z;

			// Default weight, can be assigned to later
			weight = 1f;
		}

		public void SetClusterMembership(MapCluster cluster)
		{
			parentCluster = cluster;
		}

		public void LeaveCluster()
		{
			parentCluster.RemoveMember(this);
		}

		public MapCluster GetParentCluster()
		{
			return parentCluster;
		}

		public bool IsMemberOfCluster()
		{
			return parentCluster != null;
		}

		public PointF Get2DPoint()
		{
			return new PointF(x, y);
		}

		// Returns the volume of the *bounding box* of the 'volume' shape of the data point in pixels (typically 0)
		public double GetVolumeBounds()
		{
			double boundsWith = GeometryHelper.GetMaximumBoundingBoxWidth(boundX, boundY);
			return boundsWith * boundsWith;
		}

		// Return if this volume in isolation is suitable to be drawn as a volume
		public bool QualifiesForVolumeDraw()
		{
			return !string.IsNullOrEmpty(primitiveShape) && // This has a primitive shape assigned
				GetVolumeBounds() <= Map.volumeRejectThreshold; // This is not too large so as to cause memory problems
		}
	}
}
