using System;
using System.Collections;

namespace MapParse.Types
{
	public class Vec3
	{
		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }

		public static Vec3 zero
		{
			get
			{
				return new Vec3(0, 0, 0);
			}
		}

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

		public static float Dot(Vec3 a, Vec3 b)
		{
			return a.Dot(b);
		}

		public Vec3 Cross(Vec3 v3)
		{
			float x, y, z;
			x = Y * v3.Z - v3.Y * Z;
			y = Z * v3.X - v3.Z * X;
			z = X * v3.Y - v3.X * Y;
			return new Vec3(x, y, z);
		}

		public static Vec3 Cross(Vec3 a, Vec3 b)
		{
			return a.Cross(b);
		}

		public Vec3 Normalized()
		{
			float d = Magnitude();
			return new Vec3(X / d, Y / d, Z / d);
		}

		public void Normalize()
		{
			float d = Magnitude();
			
			if (d == 1F || d == 0F)
			{
				return;
			}

			X = X / d;
			Y = Y / d;
			Z = Z / d;
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
		public static Vec3 operator- (Vec3 a)
		{
			return new Vec3() - a;
		}
		
		public static Vec3 operator* (Vec3 a, float b)
		{
			return new Vec3(a.X * b, a.Y * b, a.Z * b);	
		}
		public static Vec3 operator* (float a, Vec3 b)
		{
			return b * a;
		}
		public static Vec3 operator* (Vec3 a, double b)
		{
			return new Vec3((float)(a.X * b), (float)(a.Y * b), (float)(a.Z * b));
		}
		public static Vec3 operator* (double a, Vec3 b)
		{
			return b * a;
		}
		public static Vec3 operator* (Vec3 a, int b)
		{
			return new Vec3(a.X * b, a.Y * b, a.Z * b);
		}

		public static Vec3 operator/ (Vec3 a, float b)
		{
			return new Vec3(a.X / b, a.Y / b, a.Z / b);
		}
		public static Vec3 operator/ (Vec3 a, double b)
		{
			return new Vec3((float)(a.X / b), (float)(a.Y / b), (float)(a.Z / b));
		}
		public static Vec3 operator/ (Vec3 a, int b)
		{
			return new Vec3(a.X / b, a.Y / b, a.Z / b);
		}
		
		public static bool operator== (Vec3 a, Vec3 b)
		{
			if (a.X == b.X)
			{
				if (a.Y == b.Y)
				{
					if (a.Z == b.Z)
					{
						return true;
					}
				}
			}
			return false;
		}
		public override bool Equals(object obj)
		{
			return obj == this;
		}
		
		public static bool operator!= (Vec3 a, Vec3 b)
		{
			if (a == b)
			{
				return false;
			}
			return true;
		}

		public override string ToString()
		{
			return "X: " + this.X + ", Y: " + this.Y + ", Z: " + this.Z;
		}
	}
}