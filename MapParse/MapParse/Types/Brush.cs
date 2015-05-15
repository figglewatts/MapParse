using System;

namespace MapParse.Types
{
	public class Brush
	{
		public Vec3 Min { get; set; }
		public Vec3 Max { get; set; }
		public DynamicArray<Face> Faces { get; set; }
		public float Width
		{
			get
			{
				return Math.Abs((Max.X - Min.X)); // TODO: check this for bugs thoroughly
			}
		}
		public float Height
		{
			get
			{
				return Math.Abs((Max.Z - Min.Z));
			}
		}
		public float Depth
		{
			get
			{
				return Math.Abs((Max.Y - Min.Y));
			}
		}
		public Vec3 Center
		{
			get
			{
				return new Vec3(Min.X + Width / 2, Min.Y + Depth / 2, Min.Z + Height / 2 );
			}
		}
		public int NumberOfFaces
		{
			get
			{
				return Faces.Length;
			}
		}

		public Brush(Brush b)
		{
			Min = b.Min;
			Max = b.Max;
			Faces = b.Faces;
		}
	}
}