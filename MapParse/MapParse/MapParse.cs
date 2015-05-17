using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MapParse.Types;
using MapParse.Util;

namespace MapParse
{
	public static class MapParse
	{
		private enum ParseState
		{
			FILE,
			ENTITY,
			PROPERTY_KEY,
			PROPERTY_VAL,
			BRUSH,
			VEC3,
			PLANE,

		}
		
		private static StringBuilder sb = new StringBuilder();

		private static ParseState state;
		private static ParseState lastState;
		
		public static MapFile Parse(string mapFileContents)
		{
			MapFile map = new MapFile();
			state = ParseState.FILE;
			for (int i = 0; i < mapFileContents.Length; i++)
			{
				setState(mapFileContents[i]);


			}
		}

		private static void setState(char c)
		{
			// this is either the start of an entity or a brush
			if (c == '{')
			{
				if (state == ParseState.FILE)
				{
					// this is the start of an entity
					updateState(ParseState.ENTITY);

				}
				else if (state == ParseState.ENTITY)
				{
					// this is the start of a brush
					updateState(ParseState.BRUSH);

				}
			}
			else if (c == '"')
			{
				if (state == ParseState.ENTITY)
				{
					// property key
					updateState(ParseState.PROPERTY_KEY);
				}
				else if (state == ParseState.PROPERTY_KEY)
				{
					// property value
					updateState(ParseState.PROPERTY_VAL);
				}
			}
			else if (c == '(')
			{
				if (state == ParseState.BRUSH)
				{
					// vec3
					updateState(ParseState.VEC3);
				}
			}
			else if (c == '[')
			{
				if (state == ParseState.BRUSH)
				{
					// plane
					updateState(ParseState.PLANE);
				}
			}
		}

		private static void updateState(ParseState newState)
		{
			lastState = state;
			state = newState;
		}
	}
}
