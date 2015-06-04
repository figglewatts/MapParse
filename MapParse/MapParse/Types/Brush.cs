using System;

namespace MapParse.Types
{
	public class Brush
	{
		public Vec3 Min { get; set; }
		public Vec3 Max { get; set; }
		public DynamicArray<Face> Faces { get; set; }
		public float Width
		{
			get
			{
				return (float)Math.Abs((Max.X - Min.X)); // TODO: check this for bugs thoroughly
			}
		}
		public float Height
		{
			get
			{
				return (float)Math.Abs((Max.Z - Min.Z));
			}
		}
		public float Depth
		{
			get
			{
				return (float)Math.Abs((Max.Y - Min.Y));
			}
		}
		public Vec3 Center
		{
			get
			{
				return new Vec3(Min.X + Width / 2, Min.Y + Depth / 2, Min.Z + Height / 2 );
			}
		}
		public int NumberOfFaces
		{
			get
			{
				return Faces.Length;
			}
		}

		public Brush()
		{
			Min = new Vec3();
			Max = new Vec3();
			Faces = new DynamicArray<Face>();
		}

		public Brush(Brush b)
		{
			Min = b.Min;
			Max = b.Max;
			Faces = b.Faces;
		}

		public void GeneratePolys()
		{
			int numberOfFaces = NumberOfFaces;
			for (int faceI = 0; faceI < numberOfFaces; faceI++)
			{
				Faces[faceI].Polys.Add(new Poly());
			}

			// MASSIVE BUG HERE COULD BE DUE TO THE FACT THAT THERE IS ONLY ONE POLY PER FACE???

			for (int i = 0; i < numberOfFaces - 2; i++)
			{
				for (int j = i; j < numberOfFaces - 1; j++)
				{
					for (int k = j; k < numberOfFaces; k++)
					{
						if (Faces[i] != Faces[j] && Faces[i] != Faces[k] && Faces[j] != Faces[k])
						{
							Vec3 intersection = Vec3.zero;
							if (Faces[i].P.GetIntersection(Faces[j].P, Faces[k].P, ref intersection))
							{
								if (PointInsideBrush(intersection))
								{
									Vertex v = new Vertex(intersection);
									Faces[i].Polys[0].Verts.Add(v);
									Faces[j].Polys[0].Verts.Add(v);
									Faces[k].Polys[0].Verts.Add(v);
								}
							}
						}
					}
				}
			}
		}

		private bool PointInsideBrush(Vec3 point)
		{
			for (int i = 0; i < NumberOfFaces; i++)
			{
				if (Faces[i].P.ClassifyPoint(point) == PointClassification.FRONT)
				{
					return false;
				}
			}
			return true;
		}

		public bool AABBIntersect(Brush brush)
		{
			if ((Min.X > brush.Max.X) || (brush.Min.X > Max.X))
			{
				return false;
			}
			if ((Min.Y > brush.Max.Y) || (brush.Min.Y > Max.Y))
			{
				return false;
			}
			if ((Min.Z > brush.Max.Z) || (brush.Min.Z > Max.Z))
			{
				return false;
			}
			return true;
		}

		public void CalculateAABB()
		{
			Min = Faces[0].Polys[0].Verts[0].P;
			Max = Faces[0].Polys[0].Verts[0].P;

			for (int i = 0; i < NumberOfFaces; i++)
			{
				Face face = Faces[i];
				for (int j = 0; j < face.Polys.Length; j++)
				{
					Poly poly = face.Polys[j];
					for (int k = 0; k < poly.NumberOfVertices; k++)
					{
						Vertex vert = poly.Verts[k];
						
						// calculate minimum
						Vec3 min = Min;
						if (vert.P.X < Min.X)
						{
							min.X = vert.P.X;
						}
						if (vert.P.Y < Min.Y)
						{
							min.Y = vert.P.Y;
						}
						if (vert.P.Z < Min.Z)
						{
							min.Z = vert.P.Z;
						}
						Min = min;

						// calculate maximum
						Vec3 max = Max;
						if (vert.P.X > Max.X)
						{
							max.X = vert.P.X;
						}
						if (vert.P.Y > Max.Y)
						{
							max.Y = vert.P.Y;
						}
						if (vert.P.Z > Max.Z)
						{
							max.Z = vert.P.Z;
						}
						Max = max;
					}
				}
			}
		}
	}
}