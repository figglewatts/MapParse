using System;
using System.Collections;

namespace MapParse.Types
{
	public struct Vertex
	{
		public Vec3 P { get; set; }
		public float[] Tex { get; set; }
	
		public Vertex(Vec3 v)
		{
			this.P = v;
			this.Tex = new float[2];
		}
	
		public Vertex(Vertex v)
		{
			this.P = v.P;
			this.Tex = v.Tex;
		}
	}
}