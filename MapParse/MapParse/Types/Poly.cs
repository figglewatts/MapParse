using System;

using MapParse.Util;

namespace MapParse.Types
{
	public struct Poly
	{
		public DynamicArray<Vertex> Verts { get; set; }
		public Plane P { get; set; }
		
		// TODO: implement NumberOfVertices
		
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
		
		public override bool Equals(object obj)
		{
			return obj == this;
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