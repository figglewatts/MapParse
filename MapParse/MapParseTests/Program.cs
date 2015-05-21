using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MapParse;
using MapParse.Types;
using MapParse.Util;

namespace MapParseTests
{
	class Program
	{
		static void Main(string[] args)
		{
			MapParser.ParseMap(@"E:\Documents\parsertest.map");

			/*Vec3 intersection;

			Plane a = new Plane(new Vec3(1, 0, 0), 1);
			Plane b = new Plane(new Vec3(0, 1, 0), 1);
			Plane c = new Plane(new Vec3(0, 0, 1), 1);

			PlaneUtil.GetIntersection(a, b, c, out intersection);

			Console.WriteLine(intersection.ToString());*/

			Console.ReadLine();
		}
	}
}
