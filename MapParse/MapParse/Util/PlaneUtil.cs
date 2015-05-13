using System;
using MapParse.Types;

namespace MapParse.Util
{
	public static class PlaneUtil
	{
		public static Plane CreatePlane()
		{
			Plane p;
			p.Normal = new Vec3();
			p.Distance = 0;
			return p;
		}
		
		// Calculate the distance to a plane from a vec3
		public static float DistanceToPlane(Plane a, Vec3 b)
		{
			return (float)(ParseUtil.RoundToSignificantDigits(a.Normal.Dot(b), 5) 
				+ ParseUtil.RoundToSignificantDigits(a.Distance, 5));
		}
		
		// Calculate whether a point is in front of, behind, or on a plane
		public static PointClassification ClassifyPoint(Plane a, Vec3 b)
		{
			float distance = DistanceToPlane(a, b);
			if (distance > Constants.Epsilon)
			{
				return PointClassification.FRONT;
			}
			else if (distance < -Constants.Epsilon)
			{
				return PointClassification.BACK;
			}
			else
			{
				return PointClassification.ON_PLANE;	
			}
		}
		
		// calculate the point of intersection of this plane and 2 others
		// based on "Intersection of 3 Planes" http://geomalgorithms.com/a05-_intersect-1.html
		public bool GetIntersection(Plane a, Plane b, Plane c, ref Vec3 intersection)
		{
			float denom = a.Normal.Dot(b.Normal.Cross(c.Normal));
			if (Math.Abs(denom) < Constants.Epsilon)
			{
				return false;
			}
			intersection = ((b.Normal.Cross(c.Normal) * -a.Distance) - (c.Normal.Cross(a.Normal) * b.Distance) - (a.Normal.Cross(b.Normal) * c.Distance)) / denom;
			return true;
		}
	}
}