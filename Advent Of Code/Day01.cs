using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace Advent_Of_Code
{
	public class Day01 : BaseDay
	{
		public Day01() : base()
		{
		}

		public override ValueTask<string> Solve1()
		{
			var depths = Utils.ToIntArray(RawData);
			long part1 = depths.Skip(1)
				.Select((x, i) => x > depths[i])
				.Count(x => x);

			return new ValueTask<string>(part1.ToString());
		}

		public override ValueTask<string> Solve2()
		{
			var depths = Utils.ToIntArray(RawData);
			long part2 = depths.Skip(3)
				.Select((x, i) => x > depths[i])
				.Count(x => x);

			return new ValueTask<string>(part2.ToString());
		}
	}
}
