using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapParse.Types
{
	public class MapFile
	{
		public List<Entity> Entities { get; set; }

		public MapFile()
		{
			Entities = new List<Entity>();
		}
	}
}
