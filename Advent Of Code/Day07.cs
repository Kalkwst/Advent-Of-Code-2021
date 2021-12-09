using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Advent_Of_Code
{
	public class Day07 : BaseDay
	{
		public override ValueTask<string> Solve1()
		{
			var positions = Utils.ToIntArray(RawData, ",");

			var minimum = positions.Min();
			var maximum = positions.Max();

			var range = Enumerable.Range(minimum, maximum - minimum + 1);

			var bestPosition = range.Min(pos => positions.Sum(position => Utils.EuclideanDistance1D(position, pos)));

			return new ValueTask<string>(bestPosition.ToString());
		}

		public override ValueTask<string> Solve2()
		{
			var positions = Utils.ToIntArray(RawData, ",");

			var minimum = positions.Min();
			var maximum = positions.Max();

			var range = Enumerable.Range(minimum, maximum - minimum + 1);

			var bestPosition = range.Min(pos => positions.Sum(position => Utils.PartialSum(Utils.EuclideanDistance1D(position, pos))));

			return new ValueTask<string>(bestPosition.ToString());
		}
	}
}
