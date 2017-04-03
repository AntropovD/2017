using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlindSignature
{
	public interface ISthRotator
	{
		IEnumerable<T> RotateRight<T>(IEnumerable<T> source, int shift);
		IEnumerable<T> RotateLeft<T>(IEnumerable<T> source, int shift);
	}

	public class TemplateRotator : ISthRotator
	{
		public IEnumerable<T> RotateRight<T>(IEnumerable<T> source, int shift)
		{
			var sourceList = source.ToList();
			shift %= sourceList.Count;

			foreach (var element in sourceList.Skip(shift))
			{
				yield return element;
			}
			foreach (var element in sourceList.Take(shift))
			{
				yield return element;
			}
		}

		public IEnumerable<T> RotateLeft<T>(IEnumerable<T> source, int shift)
		{
			var sourceList = source.ToList();
			return RotateRight(sourceList, sourceList.Count - shift);
		}
	}
}
