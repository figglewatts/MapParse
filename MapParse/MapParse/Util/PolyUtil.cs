using System;

using MapParse.Types;

namespace MapParse.Util
{
	public static class PolyUtil
	{
		public static PolyClassification ClassifyPoly(Poly poly)
		{
			bool front, back = false;
			float distance = 0;
			
			for (int i = 0; i < poly.NumberOfVertices; i++)
			{
				distance = poly.P.Normal.Dot(poly.Verts[i].P) + poly.P.Distance;
				if (distance > Constants.Epsilon)
				{
					if (back)
					{
						return PolyClassification.SPLIT;
					}
					front = true;
				}
				else if (distance < -Constants.Epsilon)
				{
					if (front)
					{
						return PolyClassification.SPLIT;
					}
					back = true;
				}
			}
			if (front)
			{
				return PolyClassification.FRONT;
			}
			else if (back)
			{
				return PolyClassification.BACK;
			}
			return PolyClassification.ON_PLANE;
		}
		
		public static void SplitPoly(Poly poly, ref Poly front, ref Poly back)
		{
			PointClassification[] pointClassification = new PointClassification[poly.NumberOfVertices];
			
			// classify all points
			for (int i = 0; i < poly.NumberOfVertices; i++)
			{
				pointClassification[i] = PlaneUtil.ClassifyPoint(poly.Verts[i].P);
			}
			
			// build fragments
			Poly _front = new Poly();
			Poly _back = new Poly();
			
			_front.P = poly.P;
			_back.P = poly.P;
			for (int i = 0; i < poly.NumberOfVertices; i++)
			{
				// add point to appropriate list
				switch (pointClassification[i])
				{
					case PointClassification.FRONT:
						_front.Verts.Add(poly.Verts[i]);
						break;
					case PointClassification.BACK:
						_back.Verts.Add(poly.Verts[i]);
						break;
					case PointClassification.ON_PLANE:
						_front.Verts.Add(poly.Verts[i]);
						_back.Verts.Add(poly.Verts[i]);
						break;
				}
				
				// check if edges should be split
				int iNext = i+1;
				bool ignore = false;
				
				if (i == (poly.NumberOfVertices-1))
				{
					iNext = 0;
				}
				
				if (pointClassification[i] == PointClassification.ON_PLANE && pointClassification[iNext] != PointClassification.ON_PLANE)
				{
					ignore = true;
				}
				else if (pointClassification[iNext] == PointClassification.ON_PLANE && pointClassification[i] != PointClassification.ON_PLANE)
				{
					ignore = true;
				}
				
				if (!ignore && pointClassification[i] != pointClassification[iNext])
				{
					Vertex v = null; // new vertex created by the split
					float p = 0; // percentage between 2 vertices
					
					PlaneUtil.GetIntersection(poly.P, poly.Verts[i], poly.Verts[iNext], ref v, ref p);
					v.Tex[0] = poly.Verts[iNext].Tex[0] - poly.Verts[i].Tex[0];
					v.Tex[1] = poly.Verts[iNext].Tex[1] - poly.Verts[i].Tex[1];
					v.Tex[0] = poly.Verts[i].Tex[0] + (p * v.Tex[0]);
					v.Tex[1] = poly.Verts[i].Tex[1] + (p * v.Tex[1]);
					
					_front.Verts.Add(v);
					_back.Verts.Add(v);
				}
			}
			
			_front.CalculatePlane();
			_back.CalculatePlane();
		}
		
		public static bool CalculatePlane(Poly poly)
		{
			Vec3 centerOfMass = new Vec3();
			float magnitude;
			int i, j;
			
			if (poly.NumberOfVertices < 3)
			{
				// throw exception because this is not a valid polygon
				return false;
			}
			
			poly.P.Normal = new Vec3();
			Vec3 normal = new Vec3();
			for (int i = 0; i < poly.NumberOfVertices; i++)
			{
				j = i+1;
				if (j >= poly.NumberOfVertices)
				{
					j = 0;
				}
				normal.X = poly.P.Normal.X + (poly.Verts[i].P.Y - poly.Verts[j].P.Y) * (poly.Verts[i].P.Z + poly.Verts[j].P.Z);
				normal.Y = poly.P.Normal.Y + (poly.Verts[i].P.Z - poly.Verts[j].P.Z) * (poly.Verts[i].P.X + poly.Verts[j].P.X);
				normal.Z = poly.P.Normal.Z + (poly.Verts[i].P.X - poly.Verts[j].P.X) * (poly.Verts[i].P.Y + poly.Verts[j].P.Y);
				
				poly.P.Normal = normal;
				
				centerOfMass.X += poly.Verts[i].P.X;
				centerOfMass.Y += poly.Verts[i].P.Y;
				centerOfMass.Z += poly.Verts[i].P.Z;
			}
			
			// maybe use -Constants.Epsilon here
			if (Math.Abs(poly.P.Normal.X) < Constants.Epsilon && Math.Abs(poly.P.Normal.Y) < Constants.Epsilon && Math.Abs(poly.P.Normal.Z) < Constants.Epsilon)
			{
				return false;
			}
			magnitude = poly.P.Normal.Magnitude();
			if (magnitude < Constants.Epsilon)
			{
				return false;
			}
			poly.P.Normal.Normalize();
			
			centerOfMass = centerOfMass / poly.NumberOfVertices;
			
			poly.P.Distance = -(centerOfMass.Dot(poly.P.Normal));
			return true;
		}
	}
}