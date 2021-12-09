using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Advent_Of_Code
{
	public class Day06 : BaseDay
	{
		public override ValueTask<string> Solve1()
		{
			var data = Utils.ToLongArray(RawData, ",");
			var vector = InitializeVector(data);
			var population = 0L;

			for (int i = 1; i <= 80; i++)
				population = ProgressDay(vector);

			return new ValueTask<string>(population.ToString());
		}

		public override ValueTask<string> Solve2()
		{
			var data = Utils.ToLongArray(RawData, ",");
			var vector = InitializeVector(data);
			var population = 0L;

			for (int i = 1; i <= 256; i++)
				population = ProgressDay(vector);

			return new ValueTask<string>(population.ToString());
		}

		private long ProgressDay(long[] vector)
		{
			var newInstances = vector[0];

			for (int i = 1; i < vector.Length; i++)
			{
				var previous = vector[i];
				vector[i - 1] = previous;
			}

			vector[6] += newInstances;
			vector[8] = newInstances;

			return vector.Sum();
		}

		private long[] InitializeVector(long[] data)
		{
			long[] freq = new long[9];
			for (int i = 0; i < freq.Length; i++)
			{
				freq[i] = 0;
			}

			for (int i = 0; i < data.Length; i++)
			{
				freq[data[i]]++;
			}

			return freq;
		}
	}
}
