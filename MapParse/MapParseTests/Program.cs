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

			/*Vec3 point = new Vec3(1, 0, 0);
			Plane a = new Plane(new Vec3(1, 0, 0), -1);

			Console.WriteLine(PlaneUtil.ClassifyPoint(a, point).ToString());*/

			Console.ReadLine();
		}
	}
}
