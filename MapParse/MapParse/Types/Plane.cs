using System;
using System.Collections.Generic;

namespace MapParse.Types
{
	public struct Plane
	{
		public Vec3 Normal;
		public float Distance = 0;

		public Plane()
		{
			this.Normal = new Vec3();
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
	}
}