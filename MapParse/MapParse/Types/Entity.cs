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
			return Properties[propertyName];
		}
		
		/// <summary>
		/// Gets a property's value from it's name, and returns true if it found the property.
		/// </summary>
		public bool GetPropertyValue(string propertyName, out string propertyValue)
		{
			return Properties.TryGetValue(propertyName, out propertyValue);
		}

		/// <summary>
		/// Add a property to this entity.
		/// </summary>
		public void AddProperty(string key, string val)
		{
			Properties.Add(key, val);
		}
		public void AddProperty(Property p)
		{
			Properties.Add(p.Key, p.Value);
		}
		
		public Entity()
		{
			Properties = new Dictionary<string, string>();
			Brushes = new DynamicArray<Brush>();
		}
	}
}