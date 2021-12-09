using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Utilities.Bots;
using Utilities.Entities;

namespace Advent_Of_Code
{
	class Program
	{
		static void Main(string[] args)
		{
			/*			Console.WriteLine("Hello World!");

						Console.WriteLine(new Day01().GetIndex());
						Console.WriteLine(new Day01().InputFilePath);

						Console.WriteLine(ConfigEntity.Get("config.json").Session);

						DataBot.LoadIInput(1, 2021, new Day01().InputFilePath);

						Console.WriteLine(new Day01().RawData);*/

			//Console.WriteLine(Solver.LoadAllSolutions(Assembly.GetExecutingAssembly()).Count());

			Testy();

			//Test();

			//Console.WriteLine(new Day03().TestSolution());
		}

		public static async void Testy()
		{
			await Solver.SolveAll();
		}

		public async static void Test()
		{
			await Solver.SolvePart(true, new Day04());
		}
	}
}
