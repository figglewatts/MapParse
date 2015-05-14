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
	}
}