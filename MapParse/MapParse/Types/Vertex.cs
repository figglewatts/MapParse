using System;
using System.Collections;

public struct Vertex
{
	public Vec3 P { get; set; }
	public float[] Tex { get; set; }
	
	public Vertex(Vec3 v)
	{
		this.P = v;
		this.Tex = new float[2];
	}
	
	public static Vertex Create()
	{
		Vertex v;
		v.P = new Vec3();
		v.Tex = new float[2];
	}
}