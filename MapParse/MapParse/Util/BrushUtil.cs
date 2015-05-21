using System;

using MapParse.Types;

namespace MapParse.Util
{
	public static class BrushUtil
	{
		/// <summary>
		/// Iterate through the planes of the brushes faces and detect intersections to create vertices and polygons.
		/// </summary>
		public static void GeneratePolys(Brush brush)
		{
			// populate the brush's faces with polys
			for (int faceI = 0; faceI < brush.NumberOfFaces; faceI++)
			{
				brush.Faces[faceI].Polys.Add(new Poly());
			}
			
			// this monster loop calculates the brush's vertices from plane intersections
			// it's based on the fact that only 3 planes intersecting will generate a vertex,
			// hence the interesting looking for-loop.
			// you see things like 'int j = i' because it reduces the possible intersections
			// we need to take into account, as we don't want to process them more than once.
			for (int i = 0; i < brush.NumberOfFaces; i++)
			{
				for (int j = 0; j < brush.NumberOfFaces; j++)
				{
					for (int k = 0; k < brush.NumberOfFaces; k++)
					{
						CalculateIntersection(brush, i, j, k);
					}
				}
			}
		}
		
		/// <summary>
		/// Calculates if there is an intersection between 3 planes and adds vertices to each if there is.
		/// </summary>
		private static void CalculateIntersection(Brush brush, int i, int j, int k)
		{
			// make sure we're not processing 2 or 3 of the same face
			if (brush.Faces[i] != brush.Faces[j] && brush.Faces[i] != brush.Faces[k] && brush.Faces[j] != brush.Faces[k])
			{
				Vec3 intersection;
				if (PlaneUtil.GetIntersection(brush.Faces[i].P, brush.Faces[j].P, brush.Faces[k].P, out intersection))
				{
					if (PointInsideBrush(brush, intersection))
					{
						Vertex v = new Vertex(intersection);
						brush.Faces[i].Polys[0].Verts.Add(v);
						brush.Faces[j].Polys[0].Verts.Add(v);
						brush.Faces[k].Polys[0].Verts.Add(v);
						Console.WriteLine("Added vertices");
					}
					else
					{
						Console.WriteLine("Nope.");
					}
				}
			}
		}
		
		/// <summary>
		/// Returns true if the given point is inside the brush.
		/// </summary>
		public static bool PointInsideBrush(Brush b, Vec3 p)
		{
			for (int i = 0; i < b.NumberOfFaces; i++)
			{
				Console.WriteLine(PlaneUtil.ClassifyPoint(b.Faces[i].P, p).ToString());
				if (PlaneUtil.ClassifyPoint(b.Faces[i].P, p) == PointClassification.FRONT)
				{
					return false;
				}
			}
			return true;
		}
		
		/// <summary>
		/// Returns true if the 2 brushes AABBs are intersecting.
		/// </summary>
		public static bool AABBIntersect(Brush a, Brush b)
		{
			if ( (a.Min.X > b.Max.X) || (b.Min.X > a.Max.X) )
			{
				return false;
			}
			if ( (a.Min.Y > b.Max.Y) || (b.Min.Y > a.Max.Y) )
			{
				return false;
			}
			if ( (a.Min.Z > b.Max.Z) || (b.Min.Z > a.Max.Z) )
			{
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Calculates the AABB of the given brush. The brush is a reference, so it needs to exist prior to calling the function.
		/// </summary>
		public static void CalculateAABB(ref Brush b)
		{
			// use these as default values for max and min
			Vec3 min = b.Faces[0].Polys[0].Verts[0].P;
			Vec3 max = b.Faces[0].Polys[0].Verts[0].P;
			
			// iterate the brush's faces
			for (int fI = 0; fI < b.NumberOfFaces; fI++)
			{
				// iterate the face's polys
				for (int pI = 0; pI < b.Faces[fI].Polys.Length; pI++)
				{
					// iterate the poly's vertices
					for (int vI = 0; vI < b.Faces[fI].Polys[pI].NumberOfVertices; vI++)
					{
						Vertex vert = b.Faces[fI].Polys[pI].Verts[vI];
						
						// calculate minimum
						Vec3 _min = min;
						if (vert.P.X < min.X)
						{
							_min.X = vert.P.X;
						}
						if (vert.P.Y < min.Y)
						{
							_min.Y = vert.P.Y;
						}
						if (vert.P.Z < min.Z)
						{
							_min.Z = vert.P.Z;
						}
						min = _min;
						
						// calculate maximum
						Vec3 _max = max;
						if (vert.P.X > max.X)
						{
							_max.X = vert.P.X;
						}
						if (vert.P.Y > max.Y)
						{
							_max.Y = vert.P.Y;
						}
						if (vert.P.Z > max.Z)
						{
							_max.Z = vert.P.Z;
						}
						max = _max;
					}
				}
			}
		}
	}
}