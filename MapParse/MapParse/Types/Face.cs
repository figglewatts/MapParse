﻿using System.Collections;
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

		public Face(Face f)
		{
			TexAxis = f.TexAxis;
			TexScale = f.TexScale;
			Polys = f.Polys;
			Texture = f.Texture;
			P = f.P;
			Rotation = f.Rotation;
		}

		public static bool operator== (Face a, Face b)
		{
			if (a.P == b.P)
			{
				if (a.TexAxis == b.TexAxis)
				{
					if (a.TexScale == b.TexScale)
					{
						if (a.Texture == b.Texture)
						{
							if (a.Rotation == b.Rotation)
							{
								if (a.Polys == b.Polys)
								{
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		public static bool operator!= (Face a, Face b)
		{
			if (a == b)
			{
				return false;
			}
			return true;
		}
	}
}