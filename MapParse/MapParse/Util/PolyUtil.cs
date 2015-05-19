using System;

using MapParse.Types;

namespace MapParse.Util
{
	public static class PolyUtil
	{
		public static PolyClassification ClassifyPoly(Poly poly)
		{
			bool front = false;
			bool back = false;
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
		
		public static void SplitPoly(Poly poly, out Poly front, out Poly back)
		{
			PointClassification[] pointClassification = new PointClassification[poly.NumberOfVertices];
			
			// classify all points
			for (int i = 0; i < poly.NumberOfVertices; i++)
			{
				pointClassification[i] = PlaneUtil.ClassifyPoint(poly.P, poly.Verts[i].P);
			}
			
			// build fragments
			front = new Poly();
			back = new Poly();
			
			front.P = poly.P;
			back.P = poly.P;
			for (int i = 0; i < poly.NumberOfVertices; i++)
			{
				// add point to appropriate list
				switch (pointClassification[i])
				{
					case PointClassification.FRONT:
						front.Verts.Add(poly.Verts[i]);
						break;
					case PointClassification.BACK:
						back.Verts.Add(poly.Verts[i]);
						break;
					case PointClassification.ON_PLANE:
						front.Verts.Add(poly.Verts[i]);
						back.Verts.Add(poly.Verts[i]);
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
					Vertex v = new Vertex(); // new vertex created by the split
					float p; // percentage between 2 vertices
					
					PlaneUtil.GetIntersection(poly.P, poly.Verts[i].P, poly.Verts[iNext].P, out v, out p);
					v.Tex[0] = poly.Verts[iNext].Tex[0] - poly.Verts[i].Tex[0];
					v.Tex[1] = poly.Verts[iNext].Tex[1] - poly.Verts[i].Tex[1];
					v.Tex[0] = poly.Verts[i].Tex[0] + (p * v.Tex[0]);
					v.Tex[1] = poly.Verts[i].Tex[1] + (p * v.Tex[1]);
					
					front.Verts.Add(v);
					back.Verts.Add(v);
				}
			}
			
			CalculatePlane(ref front);
			CalculatePlane(ref back);
		}

		public static void CalculateTextureCoordinates(ref Poly p, int texWidth, int texHeight, Plane[] texAxis, float[] texScale)
		{
			// calculate poly UVs
			for (int i = 0; i < p.NumberOfVertices; i++)
			{
				float u;
				float v;
				u = (((p.Verts[i].P.X * texAxis[0].Normal.X + p.Verts[i].P.Z * texAxis[0].Normal.Y + p.Verts[i].P.Y * texAxis[0].Normal.Z) / texWidth) / texScale[0]) + (texAxis[0].Distance / texWidth);
				v = (((p.Verts[i].P.X * texAxis[1].Normal.X + p.Verts[i].P.Z * texAxis[1].Normal.Y + p.Verts[i].P.Y * texAxis[1].Normal.Z) / texWidth) / texScale[1]) + (texAxis[1].Distance / texWidth);
				p.Verts[i].Tex[0] = u;
				p.Verts[i].Tex[1] = v;
			}

			// check which axis should be normalized
			bool doU = true;
			bool doV = true;
			for (int i = 0; i < p.NumberOfVertices; i++)
			{
				if (p.Verts[i].Tex[0] < 1 && p.Verts[i].Tex[0] > -1)
				{
					doU = false;
				}
				if (p.Verts[i].Tex[1] < 1 && p.Verts[i].Tex[1] > -1)
				{
					doV = false;
				}
			}

			// calculate coordinate nearest to 0
			if (doU || doV)
			{
				float nearestU = 0;
				float u = p.Verts[0].Tex[0];
				float nearestV = 0;
				float v = p.Verts[0].Tex[1];

				if (doU)
				{
					if (u > 1)
					{
						nearestU = (float)Math.Floor(u);
					}
					else
					{
						nearestU = (float)Math.Ceiling(u);
					}
				}
				if (doV)
				{
					if (v > 1)
					{
						nearestU = (float)Math.Floor(v);
					}
					else
					{
						nearestU = (float)Math.Ceiling(v);
					}
				}

				for (int i = 0; i < p.NumberOfVertices; i++)
				{
					if (doU)
					{
						u = p.Verts[i].Tex[0];
						if (Math.Abs(u) < Math.Abs(nearestU))
						{
							if (u > 1)
							{
								nearestU = (float)Math.Floor(u);
							}
							else
							{
								nearestU = (float)Math.Ceiling(u);
							}
						}
					}
					if (doV)
					{
						v = p.Verts[i].Tex[1];
						if (Math.Abs(v) < Math.Abs(nearestV))
						{
							if (v > 1)
							{
								nearestV = (float)Math.Floor(v);
							}
							else
							{
								nearestV = (float)Math.Ceiling(v);
							}
						}
					}
				}

				// normalize texture coordinates
				for (int i = 0; i < p.NumberOfVertices; i++)
				{
					p.Verts[i].Tex[0] = p.Verts[i].Tex[0] - nearestU;
					p.Verts[i].Tex[1] = p.Verts[i].Tex[1] - nearestV;
				}
			}
		}

		public static void SortVerticesClockwise(ref Poly p)
		{
			// calculate center of polygon
			Vec3 center = new Vec3();
			for (int i = 0; i < p.NumberOfVertices; i++)
			{
				center += p.Verts[i].P;
			}

			center /= p.NumberOfVertices;

			// sort vertices
			for (int i = 0; i < p.NumberOfVertices - 2; i++)
			{
				Vec3 a = new Vec3();
				Plane plane;
				float smallestAngle = -1;
				int smallest = -1;

				a = p.Verts[i].P - center;
				a.Normalize();

				plane = new Plane(p.Verts[i].P, center, center + p.P.Normal);
				for (int j = i + 1; j < p.NumberOfVertices; j++)
				{
					if (PlaneUtil.ClassifyPoint(plane, p.Verts[j].P) != PointClassification.BACK)
					{
						Vec3 b = new Vec3();
						float angle;

						b = p.Verts[j].P - center;
						b.Normalize();

						angle = a.Dot(b);

						if (angle > smallestAngle)
						{
							smallestAngle = angle;
							smallest = j;
						}
					}
				}

				if (smallest == -1)
				{
					// TODO: throw MalformedPolyException
				}

				Vertex t = p.Verts[smallest];
				p.Verts[smallest] = p.Verts[i + 1];
				p.Verts[i + 1] = t;
			}

			// check if vertex order needs to be reversed for back-facing polygon
			Plane oldPlane = p.P;
			CalculatePlane(ref p);
			if (p.P.Normal.Dot(oldPlane.Normal) < 0)
			{
				int j = p.NumberOfVertices;
				for (int i = 0; i < j / 2; i++)
				{
					Vertex v = p.Verts[i];
					p.Verts[i] = p.Verts[j - i];
					p.Verts[j - i] = v;
				}
			}
		}
		
		public static bool CalculatePlane(ref Poly poly)
		{
			Vec3 centerOfMass = new Vec3();
			float magnitude;
			int i, j;
			
			if (poly.NumberOfVertices < 3)
			{
				// throw exception because this is not a valid polygon
				return false;
			}
			
			Vec3 normal = new Vec3();
			for (i = 0; i < poly.NumberOfVertices; i++)
			{
				j = i+1;
				if (j >= poly.NumberOfVertices)
				{
					j = 0;
				}
				normal.X = poly.P.Normal.X + (poly.Verts[i].P.Y - poly.Verts[j].P.Y) * (poly.Verts[i].P.Z + poly.Verts[j].P.Z);
				normal.Y = poly.P.Normal.Y + (poly.Verts[i].P.Z - poly.Verts[j].P.Z) * (poly.Verts[i].P.X + poly.Verts[j].P.X);
				normal.Z = poly.P.Normal.Z + (poly.Verts[i].P.X - poly.Verts[j].P.X) * (poly.Verts[i].P.Y + poly.Verts[j].P.Y);
				
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
			centerOfMass = centerOfMass / poly.NumberOfVertices;
			poly.P = new Plane(normal, -(centerOfMass.Dot(poly.P.Normal)));

			poly.P.Normal.Normalize();
			
			return true;
		}
		
		public static Poly CopyPoly(Poly poly)
		{
			Poly _poly = new Poly();
			_poly.Verts = poly.Verts;
			_poly.P = poly.P;
			return _poly;
		}
	}
}