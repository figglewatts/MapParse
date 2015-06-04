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

		/*
		/// <summary>
		/// Calculate the distance to a plane from a Vec3
		/// </summary>
		/// <param name="a">The plane.</param>
		/// <param name="b">The Vec3</param>
		/// <returns>The distance to the plane from the point.</returns>
		public static double DistanceToPlane(Plane a, Vec3 b)
		{
			//Console.WriteLine("Calculating distance to plane:");
			//Console.WriteLine("Normal dot b: " + a.Normal.Dot(b));
			//Console.WriteLine("Plane distance: " + a.Distance);
			//double dist = (a.Normal.Dot(b) - a.Distance);
			//Console.WriteLine("Distance to plane: " + dist);
			double dist = (a.Normal.Dot(b) + a.Distance) / a.Normal.Magnitude();
			return dist;
		}
		
		// Calculate whether a point is in front of, behind, or on a plane
		public static PointClassification ClassifyPoint(Plane a, Vec3 b)
		{
			//Console.WriteLine("Attempting to classify point on plane:");
			//Console.WriteLine(a.ToString());
			//Console.WriteLine(b.ToString());
			double distance = -DistanceToPlane(a, b);
			//Console.WriteLine(distance);
			if (distance > Constants.Epsilon)
			{
				//Console.ForegroundColor = ConsoleColor.Red;
				//Console.WriteLine("FRONT");
				//Console.ResetColor();
				return PointClassification.FRONT;
			}
			else if (distance < -Constants.Epsilon)
			{
				//Console.ForegroundColor = ConsoleColor.Green;
				//Console.WriteLine("BACK");
				//Console.ResetColor();
				return PointClassification.BACK;
			}
			else
			{
				//Console.ForegroundColor = ConsoleColor.Green;
				//Console.WriteLine("ON_PLANE");
				//Console.ResetColor();
				return PointClassification.ON_PLANE;	
			}
		}
		
		/// <summary>
		/// Calculate the point of intersection of 3 planes
		/// based on "Intersection of 3 Planes" http://geomalgorithms.com/a05-_intersect-1.html
		/// </summary>
		public static bool GetIntersection(Plane a, Plane b, Plane c, out Vec3 intersection)
		{
			double denom = a.Normal.Dot(b.Normal.Cross(c.Normal));
			if (Math.Abs(denom) < Constants.Epsilon)
			{
				intersection = new Vec3();
				return false;
			}
			else
			{
				intersection = (-(a.Distance * b.Normal.Cross(c.Normal)) - (b.Distance * c.Normal.Cross(a.Normal)) - (c.Distance * a.Normal.Cross(b.Normal))) / denom;
				return true;
			}
		}

		public static bool GetIntersection(Plane p, Vec3 start, Vec3 end, out Vertex intersection, out double percentage)
		{
			Vec3 direction = end - start;
			double num, denom;

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
		}*/
	}
}