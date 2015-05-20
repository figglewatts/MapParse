using System;
using System.Collections;

namespace MapParse.Types
{
	public class Vertex
	{
		public Vec3 P { get; set; }
		public double[] Tex { get; set; }
	
		public Vertex(Vec3 v)
		{
			P = v;
			Tex = new double[2];
		}
	
		public Vertex(Vertex v)
		{
			P = v.P;
			Tex = v.Tex;
		}

		public Vertex()
		{
			P = new Vec3();
			Tex = new double[2];
		}
	}
}