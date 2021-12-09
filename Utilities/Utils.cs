using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities
{
	public static partial class Utils
	{
		public static (List<TSource> TruthyValues, List<TSource> FalsyValues) Bifurcate<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> fn)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));
			if (fn == null)
				throw new ArgumentNullException(nameof(fn));

			List<TSource> truthyValues = new();
			List<TSource> falsyValues = new();

			foreach (var element in source)
			{
				if (fn(element))
					truthyValues.Add(element);
				else
					falsyValues.Add(element);
			}

			return (truthyValues, falsyValues);
		}

		public static string MostCommon(string[] sequence)
			=> sequence.GroupBy(v => v)
					.OrderByDescending(g => g.Count())
					.First()
					.Key;

		public static IEnumerable<int> Sequence(int start, int stop)
			=> Sequence(start, stop, start < stop ? 1 : -1);

		public static IEnumerable<int> Sequence(int start, int stop, int step)
		{
			int current = start;

			while (step >= 0 ? stop >= current
							 : stop <= current)
			{
				yield return current;
				current += step;
			}
		}
	}
}
