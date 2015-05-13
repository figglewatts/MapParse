using System;
using System.Collections;

namespace MapParse.Types
{
	public class Vec3
	{
		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }

		public Vec3() { }

		public Vec3(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}

		public float Dot(Vec3 v3)
		{
			return (X * v3.X) + (Y * v3.Y) + (Z * v3.Z);
		}

		public Vec3 Cross(Vec3 v3)
		{
			float x, y, z;
			x = Y * v3.Z - v3.Y * Z;
			y = X * v3.Z - v3.X * Z;
			z = X * v3.Y - v3.X * Y;
			return new Vec3(x, y, z);
		}

		public void Normalize()
		{
			float d = X * X + Y * Y + Z * Z;
			
			if (d == 1F || d == 0F)
			{
				return;
			}

			d = (float)Math.Sqrt(d);
			X *= d;
			Y *= d;
			Z *= d;
		}
		
		public float Magnitude()
		{
			return (float)Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
		}

		public static Vec3 operator+ (Vec3 a, Vec3 b)
		{
			return new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		}

		public static Vec3 operator- (Vec3 a, Vec3 b)
		{
			return new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
		}
		
		public static Vec3 operator* (Vec3 a, float b)
		{
			return new Vec3(a.X * b, a.Y * b, a.Z * b);	
		}
		public static Vec3 operator* (Vec3 a, int b)
		{
			return new Vec3(a.X * b, a.Y * b, a.Z * b);
		}

		public static Vec3 operator/ (Vec3 a, float b)
		{
			return new Vec3(a.X / b, a.Y / b, a.Z / b);
		}
		public static Vec3 operator/ (Vec3 a, int b)
		{
			return new Vec3(a.X / b, a.Y / b, a.Z / b);
		}
	}
}