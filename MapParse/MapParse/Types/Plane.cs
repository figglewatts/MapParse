using System;
using System.Collections.Generic;

namespace MapParse.Types
{
	public struct Plane
	{
		public Vec3 Normal { get; set; }
		public float Distance { get; set; }

		public Plane()
		{
			this.Normal = new Vec3();
		}

		public Plane(Plane p)
		{
			this.Normal = p.Normal;
			this.Distance = p.Distance;
		}

		public Plane(Vec3 n, float d)
		{
			this.Normal = n;
			this.Distance = d;
		}

		public Plane(Vec3 a, Vec3 b, Vec3 c)
		{
			this.Normal = (c - b).Cross(a - b);
			this.Normal.Normalize();
			this.Distance = -this.Normal.Dot(a);
		}

		public static bool operator== (Plane a, Plane b)
		{
			if (a.Normal == b.Normal)
			{
				if (a.Distance == b.Distance)
				{
					return true;
				}
			}
			return false;
		}

		public static bool operator!= (Plane a, Plane b)
		{
			if (a == b)
			{
				return false;
			}
			return true;
		}
	}
}