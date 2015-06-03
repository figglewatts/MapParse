using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
		public const double Epsilon = 1E-15;

		public const int NEWLINE = 0x0A;
		public const int CARRAIGE_RETURN = 0x0D;
		public const int QUOTATION_MARK = 0x22;
		public const int LEFT_PARENTHESIS = 0x28;
		public const int RIGHT_PARENTHESIS = 0x29;
		public const int LEFT_BRACKET = 0x5B;
		public const int RIGHT_BRACKET = 0x5D;
		public const int LEFT_BRACE = 0x7B;
		public const int RIGHT_BRACE = 0x7D;
		public const int SPACE = 0x20;
	}
}
