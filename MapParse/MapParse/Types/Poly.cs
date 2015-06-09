using System;

using MapParse.Exceptions;
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
			P = new Plane();
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

		public static PolyClassification ClassifyPoly(Poly poly)
		{
			bool front = false;
			bool back = false;
			float distance;

			for (int i = 0; i < poly.NumberOfVertices; i++)
			{
				distance = Vec3.Dot(poly.P.Normal, poly.Verts[i].P) + (float)poly.P.Distance;
				if (distance > 0.001)
				{
					if (back)
					{
						return PolyClassification.SPLIT;
					}
					front = true;
				}
				else if (distance < -0.001)
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

		public void SplitPoly(Poly poly, ref Poly front, ref Poly back)
		{
			PointClassification[] pointClassification = new PointClassification[poly.NumberOfVertices];

			// classify all points
			for (int i = 0; i < poly.NumberOfVertices; i++)
			{
				pointClassification[i] = P.ClassifyPoint(poly.Verts[i].P);
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
				int iNext = i + 1;
				bool ignore = false;

				if (i == (poly.NumberOfVertices - 1))
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
					Vertex v = null; // new vertex made by splitting
					float p = 0; // percentage between the 2 points

					poly.P.GetIntersection(poly.Verts[i].P, poly.Verts[iNext].P, ref v, ref p);
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

		public void CalculateTextureCoordinates(int texWidth, int texHeight, Plane[] texAxis, float[] texScale)
		{
			for (int i = 0; i < NumberOfVertices; i++)
			{
				float u;
				float v;

				u = (((Verts[i].P.X * texAxis[0].Normal.X + Verts[i].P.Z * texAxis[0].Normal.Z + Verts[i].P.Y * texAxis[0].Normal.Y) / texWidth) / texScale[0]) + ((float)texAxis[0].Distance / texWidth);
				v = (((Verts[i].P.X * texAxis[1].Normal.X + Verts[i].P.Z * texAxis[1].Normal.Z + Verts[i].P.Y * texAxis[1].Normal.Y) / texHeight) / texScale[1]) + ((float)texAxis[1].Distance / texHeight);

				Verts[i].Tex[0] = u;
				Verts[i].Tex[1] = v;
			}

			// check which axis should be normalized
			bool doU = true;
			bool doV = true;
			for (int i = 0; i < NumberOfVertices; i++)
			{
				if (Verts[i].Tex[0] < 1 && Verts[i].Tex[0] > -1)
				{
					doU = false;
				}
				if (Verts[i].Tex[1] < 1 && Verts[i].Tex[1] > -1)
				{
					doV = false;
				}
			}

			// calculate coordinate nearest to 0
			if (doU || doV)
			{
				float nearestU = 0;
				float u = Verts[0].Tex[0];
				float nearestV = 0;
				float v = Verts[0].Tex[1];

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

				for (int i = 0; i < NumberOfVertices; i++)
				{
					if (doU)
					{
						u = Verts[i].Tex[0];
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
						v = Verts[i].Tex[1];
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
				for (int i = 0; i < NumberOfVertices; i++)
				{
					Verts[i].Tex[0] = Verts[i].Tex[0] - nearestU;
					Verts[i].Tex[1] = Verts[i].Tex[1] - nearestV;
				}
			}
		}

		public void SortVerticesCW()
		{
			// calculate center of polygon
			Vec3 center = Vec3.zero;
			for (int i = 0; i < NumberOfVertices; i++)
			{
				center += Verts[i].P;
			}

			center /= NumberOfVertices;

			// sort vertices
			for (int i = 0; i < NumberOfVertices - 2; i++)
			{
				Vec3 a = Vec3.zero;
				Plane p;
				float smallestAngle = -1;
				int smallest = -1;

				a = Verts[i].P - center;
				a.Normalize();

				p = new Plane(Verts[i].P, center, center + P.Normal);
				for (int j = i + 1; j < NumberOfVertices; j++)
				{
					if (p.ClassifyPoint(Verts[j].P) != PointClassification.BACK)
					{
						Vec3 b = Vec3.zero;
						float angle;

						b = Verts[j].P - center;
						b.Normalize();

						angle = Vec3.Dot(a, b);

						if (angle > smallestAngle)
						{
							smallestAngle = angle;
							smallest = j;
						}
					}
				}

				if (smallest == -1)
				{
					//throw new MalformedPolyException("Polygon has less than 3 vertices!");
					return;
				}

				Vertex t = Verts[smallest];
				Verts[smallest] = Verts[i + 1];
				Verts[i + 1] = t;
			}

			// check if vertex order needs to be reversed for back-facing polygon
			Plane oldPlane = P;
			CalculatePlane();
			if (Vec3.Dot(P.Normal, oldPlane.Normal) < 0)
			{
				int j = NumberOfVertices;
				for (int i = 0; i < j / 2; i++)
				{
					Vertex v = Verts[i];
					Verts[i] = Verts[j - i];
					Verts[j - i] = v;
				}
			}
		}

		public bool CalculatePlane()
		{
			Vec3 centerOfMass = Vec3.zero;
			float magnitude;
			int i;
			int j;

			if (NumberOfVertices < 3)
			{
				//throw new MalformedPolyException("Poly has less than 3 vertices.");
				return false;
			}

			P.Normal = Vec3.zero;
			Vec3 normal = Vec3.zero;
			for (i = 0; i < NumberOfVertices; i++)
			{
				j = i + 1;
				if (j >= NumberOfVertices)
				{
					j = 0;
				}
				normal.X = P.Normal.X + (Verts[i].P.Y - Verts[j].P.Y) * (Verts[i].P.Z + Verts[j].P.Z);
				normal.Y = P.Normal.Y + (Verts[i].P.Z - Verts[j].P.Z) * (Verts[i].P.X + Verts[j].P.X);
				normal.Z = P.Normal.Z + (Verts[i].P.X - Verts[j].P.X) * (Verts[i].P.Y + Verts[j].P.Y);

				P.Normal = normal;

				centerOfMass.X += Verts[i].P.X;
				centerOfMass.Y += Verts[i].P.Y;
				centerOfMass.Z += Verts[i].P.Z;
			}

			if (Math.Abs(P.Normal.X) < Constants.Epsilon && Math.Abs(P.Normal.Y) < Constants.Epsilon && Math.Abs(P.Normal.Z) < Constants.Epsilon)
			{
				return false;
			}
			magnitude = P.Normal.Magnitude();
			if (magnitude < Constants.Epsilon)
			{
				return false;
			}
			Vec3 normalized = Vec3.zero;
			normalized.X = P.Normal.X / magnitude;
			normalized.Y = P.Normal.Y / magnitude;
			normalized.Z = P.Normal.Z / magnitude;
			P.Normal = normalized;

			centerOfMass.X /= NumberOfVertices;
			centerOfMass.Y /= NumberOfVertices;
			centerOfMass.Z /= NumberOfVertices;

			P.Distance = -(Vec3.Dot(centerOfMass, P.Normal));
			return true;
		}
	}
}