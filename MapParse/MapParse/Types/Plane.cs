using System;
using System.Collections.Generic;

namespace MapParse.Types
{
	/// <summary>
	/// Plane class. Follows N dot P + D = 0 equation.
	/// </summary>
	public class Plane
	{
		public Vec3 Normal { get; set; }
		public double Distance { get; set; }

		public Plane()
		{
			this.Normal = new Vec3();
			this.Distance = 0F;
		}
		
		public Plane(Plane p)
		{
			this.Normal = p.Normal;
			this.Distance = p.Distance;
		}

		public Plane(Vec3 n, double d)
		{
			this.Normal = n;
			this.Distance = d;
		}

		public Plane(Vec3 a, Vec3 b, Vec3 c)
		{
			//Console.WriteLine("Calculating plane.");
			this.Normal = (c - b).Cross(a - b);
			//Console.WriteLine("Normal pre normalize: " + this.Normal.ToString());
			this.Normal.Normalize();
			//Console.WriteLine("Post normalize: " + this.Normal.ToString());
			this.Distance = this.Normal.Dot(a);
			//Console.WriteLine("A: " + a.ToString());
			//Console.WriteLine("B: " + b.ToString());
			//Console.WriteLine("C: " + c.ToString());
			//Console.WriteLine("D: " + this.Distance);
			//Console.WriteLine("\n\n\n");
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

		public override string ToString()
		{
			return this.Normal.ToString() + ", D: " + this.Distance.ToString();
		}
	}
}