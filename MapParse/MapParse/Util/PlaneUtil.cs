using System;
using MapParse.Types;

namespace MapParse.Util
{
	public static class PlaneUtil
	{
		public static Plane CreatePlane()
		{
			Plane p = new Plane();
			p.Normal = new Vec3();
			p.Distance = 0;
			return p;
		}
		
		/// <summary>
		/// Calculate the distance to a plane from a Vec3
		/// </summary>
		/// <param name="a">The plane.</param>
		/// <param name="b">The Vec3</param>
		/// <returns>The distance to the plane from the point.</returns>
		public static float DistanceToPlane(Plane a, Vec3 b)
		{
			return (float)(ParseUtils.RoundToSignificantDigits(a.Normal.Dot(b), 5) 
				+ ParseUtils.RoundToSignificantDigits(a.Distance, 5));
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
		
		/// <summary>
		/// Calculate the point of intersection of 3 planes
		/// based on "Intersection of 3 Planes" http://geomalgorithms.com/a05-_intersect-1.html
		/// </summary>
		public static bool GetIntersection(Plane a, Plane b, Plane c, out Vec3 intersection)
		{
			float denom = a.Normal.Dot(b.Normal.Cross(c.Normal));
			if (Math.Abs(denom) < Constants.Epsilon)
			{
				intersection = new Vec3();
				return false;
			}
			intersection = ((b.Normal.Cross(c.Normal) * -a.Distance) - (c.Normal.Cross(a.Normal) * b.Distance) - (a.Normal.Cross(b.Normal) * c.Distance)) / denom;
			return true;
		}

		public static bool GetIntersection(Plane p, Vec3 start, Vec3 end, out Vertex intersection, out float percentage)
		{
			Vec3 direction = end - start;
			float num, denom;

			direction.Normalize();
			
			denom = p.Normal.Dot(direction);
			
			if (Math.Abs(denom) < Constants.Epsilon)
			{
				percentage = 0F;
				intersection = new Vertex(new Vec3());
				return false;
			}
			
			num = -DistanceToPlane(p, start);
			percentage = num / denom;
			intersection = new Vertex(start + (direction * percentage));
			percentage = percentage / (end - start).Magnitude();
			return true;
		}
	}
}