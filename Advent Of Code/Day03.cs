using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;

namespace Advent_Of_Code
{
	public class Day03 : BaseDay
	{
		public override ValueTask<string> Solve1()
		{
			var data = Utils.ToStringArray(RawData);

			var gammaRate = "";
			var epsilonRate = "";

			for (int i = 0; i < data[0].Length; i++)
			{
				(List<string> ones, List<string> zeros) = Utils.Bifurcate(GetBitsAt(data, i), IsOne);

				if (ones.Count > zeros.Count)
				{
					gammaRate += "1";
					epsilonRate += "0";
				}
				else
				{
					gammaRate += "0";
					epsilonRate += "1";
				}
			}

			var result =  (Convert.ToInt32(gammaRate, 2) * Convert.ToInt32(epsilonRate, 2)).ToString();
			return new ValueTask<string>(result);
		}

		public override ValueTask<string> Solve2()
		{
			var data = Utils.ToStringArray(RawData);

			string oxygen = GetOxygenRating(data, 0);
			string co2 = GetCO2Rating(data, 0);

			var result = (Convert.ToInt32(oxygen, 2) * Convert.ToInt32(co2, 2)).ToString();

			return new ValueTask<string>(result);
		}

		public string TestSolution()
		{
			RawData = "00100\n11110\n10110\n10111\n10101\n01111\n00111\n11100\n10000\n11001\n00010\n01010";
			Console.WriteLine(GetOxygenRating(Utils.ToStringArray(RawData), 0));
			Console.WriteLine(GetCO2Rating(Utils.ToStringArray(RawData), 0));
			return GetOxygenRating(Utils.ToStringArray(RawData), 0);
		}

		private static string[] GetBitsAt(string[] sequence, int index)
		{
			if (index > sequence[0].Length)
				throw new ArgumentOutOfRangeException(nameof(index));

			string[] bits = new string[sequence.Length];

			for (int i = 0; i < sequence.Length; i++)
			{
				bits[i] = sequence[i][index].ToString();
			}

			return bits;
		}

		private static bool IsOne(string bit)
			=> bit.Equals("1");

		private static bool HasMostCommon(string sequence, string bit, int position)
			=> sequence[position].ToString().Equals(bit);

		private static string GetOxygenRating(string[] sequences, int index)
		{
			if (sequences.Length == 1)
				return sequences[0];

			string mostCommon = Utils.MostCommon(GetBitsAt(sequences, index));

			List<string> common = new List<string>();
			List<string> uncommon = new List<string>();

			foreach(var sequence in sequences)
			{
				if(HasMostCommon(sequence, mostCommon, index))
				{
					common.Add(sequence);
				}
				else
				{
					uncommon.Add(sequence);
				}
			}

			if (common.Count == uncommon.Count)
				if (uncommon[0][index].ToString().Equals("1"))
					return GetOxygenRating(uncommon.ToArray(), index + 1);

			return GetOxygenRating(common.ToArray(), index+1);
		}

		private static string GetCO2Rating(string[] sequences, int index)
		{
			if (sequences.Length == 1)
				return sequences[0];

			string mostCommon = Utils.MostCommon(GetBitsAt(sequences, index));

			List<string> common = new List<string>();
			List<string> uncommon = new List<string>();

			foreach (var sequence in sequences)
			{
				if (HasMostCommon(sequence, mostCommon, index))
				{
					common.Add(sequence);
				}
				else
				{
					uncommon.Add(sequence);
				}
			}

			if (common.Count == uncommon.Count)
				if (common[0][index].ToString().Equals("0"))
					return GetCO2Rating(common.ToArray(), index + 1);

			return GetCO2Rating(uncommon.ToArray(), index + 1);
		}
	}
}
