using System;
using System.Collections.Generic;

namespace MapParse.Types
{
	public class Entity
	{
		public Dictionary<string, string> Properties { get; set; }
		public DynamicArray<Brush> Brushes { get; set; }
		public int NumberOfBrushes
		{
			get
			{
				return Brushes.Length;
			}
		}
		
		/// <summary>
		/// Get a property value from this entity by property name.
		/// </summary>
		public string GetPropertyValue(string propertyName)
		{
			// TODO: implement this method
		}
		
		/// <summary>
		/// Gets a property's value from it's name, and returns true if it found the property.
		/// </summary>
		public bool GetPropertyValue(string propertyName, ref propertyValue)
		{
			// TODO: implement this method
		}
		
		public Entity()
		{
			Properties = new Dictionary<string, string>();
			Brushes = new DynamicArray<Brush>();
		}
	}
}