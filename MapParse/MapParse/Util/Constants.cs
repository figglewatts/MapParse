using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapParse.Util
{
	/// <summary>
	/// A class containing constant values. Used for easy access to said constants.
	/// </summary>
	public static class Constants
	{
		/// <summary>
		/// A value used for comparing floats. E.G. to determine if a float was equal to a value you'd see if it was within plus or minus Epsilon of the value.
		/// </summary>
		public const double Epsilon = 1e-5;
	}
}
