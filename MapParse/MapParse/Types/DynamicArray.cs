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
			T[] newArray = new T[Length + 1]; // maybe need to +2 here due to zero-indexing
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
	}
}