using System.Collections;
using System.Collections.Generic;

namespace MapParse.Types
{
	public struct Face
	{
		public Plane P { get; set; }
		public Plane[] TexAxis { get; set; }
		public float[] TexScale { get; set; }
		public string Texture { get; set; }
		public float Rotation { get; set; }
		public DynamicArray<Poly> Polys { get; set; }
		
		public Face() 
		{
			TexAxis = new Plane[2];
			TexScale = new float[2];
			Polys = new DynamicArray<Poly>();
			Texture = "";
			P = new Plane();
			Rotation = 0F;
		}
	}
}