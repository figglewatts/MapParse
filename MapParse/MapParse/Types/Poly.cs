using System;

using MapParse.Util;

namespace MapParse.Types
{
	public class Poly
	{
		public DynamicArray<Vertex> Verts { get; set; }
		public Plane P { get; set; }
		
		public int NumberOfVertices
		{
			get
			{
				return Verts.Length;
			}
		}
		
		/// <summary>
		/// Creates a Poly with a set number of vertices.
		/// </summary>
		public Poly(int numberOfVertices)
		{
			this.Verts = new DynamicArray<Vertex>();
			P = PlaneUtil.CreatePlane();
		}
		/// <summary>
		/// Creates a poly with a set number of vertices along plane p.
		/// </summary>
		public Poly(int numberOfVertices, Plane p)
		{
			this.Verts = new DynamicArray<Vertex>();
			P = p;
		}
		public Poly(Poly p)
		{
			Verts = p.Verts;
			P = p.P;
		}
		public Poly()
		{
			Verts = new DynamicArray<Vertex>();
			P = new Plane();
		}
		
		public static bool operator== (Poly a, Poly b)
		{
			if (a.NumberOfVertices == b.NumberOfVertices)
			{
				if (a.P.Distance == b.P.Distance)
				{
					if (a.P.Normal == b.P.Normal)
					{
						for (int i = 0; i < a.NumberOfVertices; i++)
						{
							if (a.Verts[i].P == b.Verts[i].P)
							{
								if (a.Verts[i].Tex[0] != b.Verts[i].Tex[0])
								{
									return false;
								}
								if (a.Verts[i].Tex[1] != b.Verts[i].Tex[1])
								{
									return false;
								}
							}
							else
							{
								return false;
							}
						}
						return true;
					}
				}
			}
			return false;
		}
		
		public static bool operator!= (Poly a, Poly b)
		{
			if (a == b)
			{
				return false;
			}
			return true;
		}
	}
}