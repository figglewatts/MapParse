using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapParse.Types
{
	public struct Property
	{
		public string Key { get; set; }
		public string Value { get; set; }

		public Property(string k, string v)
		{
			Key = k;
			Value = v;
		}
	}
}
