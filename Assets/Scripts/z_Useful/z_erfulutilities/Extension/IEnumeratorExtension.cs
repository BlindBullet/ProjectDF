using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class IEnumeratorExtension
{
	public static IEnumerable<T> NotNull<T>(IEnumerable<T> source) where T: class
	{
		foreach(var elem in source)
		{
			if(elem == null)
			{
				continue;
			}
			yield return elem;
		}
	}

	public static IEnumerable<T> ToSafe<T>(this IEnumerable<T> source)
	{
		return source ?? Enumerable.Empty<T>();
	}
}