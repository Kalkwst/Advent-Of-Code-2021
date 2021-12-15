using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Advent_Of_Code
{
	public class Day08 : BaseDay
	{
		public override ValueTask<string> Solve1()
		{
			string[] outputs = ExtractOutputValues();
			int uniqueSegments = 0;

			foreach (string segment in outputs)
			{
				if (segment.Length == 2 ||
					segment.Length == 3 ||
					segment.Length == 4 ||
					segment.Length == 7)

					uniqueSegments += 1;
			}

			return new ValueTask<string>(uniqueSegments.ToString());
		}

		public override ValueTask<string> Solve2()
		{
			var data = Utils.ToStringArray(RawData);

			var signalList = new List<List<string>>();
			var outputList = new List<List<string>>();

			foreach(var line in data)
			{
				var result = line.Split("|");

				if(result.Length > 1)
				{
					signalList.Add(result[0].Split(" ").ToList());
					outputList.Add(result[0].Split(" ").ToList());
				}
			}

			long answer = 0;

			for(int i = 0; i < signalList.Count; i++)
			{
				var segment = new Decoder(signalList[i]);
				answer += segment.GetOutputValue(outputList[i]);
			}

			return new ValueTask<string>(answer.ToString());
		}

		private string[] ExtractOutputValues()
		{
			string[] inputLines = RawData.Split("\n");
			List<string> outputs = new();

			foreach (string line in inputLines)
			{
				var result = line.Split("|");
				if (result.Length > 1)
				{
					string[] segments = result[1].Split(" ").ToArray();
					outputs.AddRange(segments);
				}
			}

			return outputs.ToArray();
		}
	}

	public class Decoder
	{
		private string codeOne = string.Empty;
		private string codeThree = string.Empty;
		private string codeFour = string.Empty;
		private string codeSix = string.Empty;
		private string codeSeven = string.Empty;
		private string codeEight = string.Empty;
		private string codeNine = string.Empty;

		readonly string[] Segments = new string[7];
		readonly List<string> SixCountCodes = new();
		readonly List<string> FiveCountCodes = new();


		public Decoder(List<string> signals)
		{
			Decode(signals);
		}

		private void Decode(List<string> signals)
		{
			foreach (var signal in signals)
			{
				if (signal.Length == 2)
					codeOne = signal;
				else if (signal.Length == 3)
					codeSeven = signal;
				else if (signal.Length == 4)
					codeFour = signal;
				else if (signal.Length == 7)
					codeEight = signal;
				else
				{
					if (signal.Length == 6) 
						SixCountCodes.Add(signal);
					else if (signal.Length == 5)
						FiveCountCodes.Add(signal);
				}
			}

			Segments[0] = string.Concat(codeSeven.Except(codeOne));

			foreach (var signal in SixCountCodes)
			{
				if (signal.Except(codeSeven).Except(codeFour).Count() == 1)
				{
					codeNine = signal;
				}

				if (signal.Except(codeSeven).Count() == 4)
				{
					codeSix = signal;
				}
			}

			Segments[4] = string.Concat(codeEight.Except(codeNine));
			Segments[6] = string.Concat(codeEight.Except(codeSeven).Except(codeFour).Except(Segments[4]));

			foreach (var signal in FiveCountCodes)
			{
				var segment = string.Concat(signal.Except(codeSeven).Except(Segments[6]));

				if (segment.Length == 1)
				{
					codeThree = signal;
					Segments[3] = segment;
					break;
				}
			}

			Segments[1] = string.Concat(codeEight.Except(codeThree).Except(Segments[4]));
			Segments[2] = string.Concat(codeEight.Except(codeSix));
			Segments[5] = string.Concat(codeOne.Except(Segments[2]));
		}

		public long GetOutputValue(List<string> outputList)
		{
			var outputString = string.Empty;
			foreach(var output in outputList)
			{
				if (!string.IsNullOrEmpty(output))
					outputString += DecodeOutput(output);
			}

			return long.Parse(outputString);
		}

		private string DecodeOutput(string output)
		{
			if (output.Length == 2)
				return "1";
			else if (output.Length == 3)
				return "7";
			else if (output.Length == 4)
				return "4";
			else if (output.Length == 7)
				return "8";
			else
			{
				if (output.Length == 6)
				{
					if (!output.Contains(Segments[3]))
						return "0";
					else if (!output.Contains(Segments[2]))
						return "6";
					else
						return "9";
				}
				else if (output.Length == 5) //2,3,5
				{
					if (!output.Contains(Segments[5]))
						return "2";
					else if (!output.Contains(Segments[1]))
						return "3";
					else
						return "5";
				}
			}

			throw new Exception($"Cannot decode {output}");
		}
	}
}


