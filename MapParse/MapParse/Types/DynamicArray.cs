using System;

namespace MapParse.Types
{
	/// <summary>
	/// An array that can be added to at runtime.
	/// </summary>
	public class DynamicArray<T>
	{
		public T[] Elements { get; set; }
		public int Length
		{
			get
			{
				return Elements.Length;
			}
		}
		
		public DynamicArray()
		{
			this.Elements = new T[0];
		}
		public DynamicArray(int length)
		{
			this.Elements = new T[length];
		}
		
		/// <summary>
		/// Add an element to the end of the array.
		/// </summary>
		public void Add(T element)
		{
			T[] newArray = new T[Length + 1];
			for (int i = 0; i < Length; i++)
			{
				newArray[i] = Elements[i];
			}
			newArray[Length] = element;
			Elements = newArray;
		}
		
		public T this[int index]
		{
			get
			{
				return this.Elements[index];
			}
			set
			{
				this.Elements[index] = value;
			}
		}

		public static bool operator== (DynamicArray<T> a, DynamicArray<T> b)
		{
			if (a.Length == b.Length)
			{
				if (a.Elements == b.Elements)
				{
					return true;
				}
			}
			return false;
		}

		public static bool operator!= (DynamicArray<T> a, DynamicArray<T> b)
		{
			if (a == b)
			{
				return false;
			}
			return true;
		}
	}
}