using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;


namespace Utilities.Bots
{
	public static class Solver
	{
		private record ElapsedTime(double constructor, double Part1, double Part2);

		public static async Task SolveAll()
		{
			var totalElapsedTime = new List<ElapsedTime>();
			var table = GetTable();

			await AnsiConsole.Live(table)
				.StartAsync(async fn =>
				{
					var sw = new Stopwatch();

					foreach (Type problemType in LoadAllSolutions(Assembly.GetEntryAssembly()!))
					{
						sw.Restart();
						var potentialProblem = Activator.CreateInstance(problemType);
						sw.Stop();

						if (potentialProblem is BaseDay problem)
						{
							totalElapsedTime.Add(await SolveProblem(problem, table, CalculateElapsedMillis(sw)));
							fn.Refresh();
						}
					}
				});

			RenderOverallResultsPanel(totalElapsedTime);
		}

		private static async Task<ElapsedTime> SolveProblem(BaseDay day, Table table, double constructorElapsedTime)
		{
			var problemIndex = day.GetIndex();
			var problemTitle = problemIndex != default
				? $"Day {problemIndex}"
				: $"{day.GetType().Name}";

			RenderRow(table, problemTitle, $"{day.GetType().Name}()", "-----------", constructorElapsedTime);

			(string solution1, double elapsedMillisPart1) = await SolvePart(isPart1: true, day);
			RenderRow(table, problemTitle, "Part 1", solution1, elapsedMillisPart1);

			(string solution2, double elapsedMillisPart2) = await SolvePart(isPart1: false, day);
			RenderRow(table, problemTitle, "Part 2", solution2, elapsedMillisPart2);

			RenderRow(table, problemTitle, "[bold]Total[/]", "-----------", constructorElapsedTime + elapsedMillisPart1 + elapsedMillisPart2);

			table.AddEmptyRow();

			return new ElapsedTime(constructorElapsedTime, elapsedMillisPart1, elapsedMillisPart2);
		}

		public static async Task<(string solution, double elapsedTime)> SolvePart(bool isPart1, BaseDay day)
		{
			Stopwatch stopwatch = new();
			var solution = string.Empty;

			try
			{
				Func<ValueTask<string>> solve = isPart1
					? day.Solve1
					: day.Solve2;

				stopwatch.Start();
				solution = await solve();
			}
			catch (NotImplementedException)
			{
				solution = "[[Not Implemented]]";
			}
			catch (Exception ex)
			{
				solution = ex.Message + Environment.NewLine + ex.StackTrace;
			}
			finally
			{
				stopwatch.Stop();
			}

			var elapsedMillisecods = CalculateElapsedMillis(stopwatch);

			return (solution, elapsedMillisecods);
		}

		private static double CalculateElapsedMillis(Stopwatch stopwatch)
		{
			return 1000 * stopwatch.ElapsedTicks / (double)Stopwatch.Frequency;
		}

		internal static IEnumerable<Type> LoadAllSolutions(Assembly assembly)
		{
			return assembly.GetTypes()
				.Where(type => typeof(BaseDay).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);
		}

		private static Table GetTable()
		{
			return new Table()
				.AddColumns(
					"[bold]Day[/]",
					"[bold]Part[/]",
					"[bold]Solution[/]",
					"[bold]Elapsed Time[/]"
				)
				.RoundedBorder()
				.BorderColor(Color.Grey);
		}

		private static void RenderRow(Table table, string problemTitle, string part, string solution, double elapsedTime)
		{
			var formattedTime = FormatTime(elapsedTime);

			table.AddRow(problemTitle, part, solution, formattedTime);
		}

		private static string FormatTime(double elapsedMilliseconds, bool useColor = true)
		{
			var message = elapsedMilliseconds switch
			{
				< 1 => $"{elapsedMilliseconds:F} ms",
				< 1_000 => $"{Math.Round(elapsedMilliseconds)} ms",
				< 60_000 => $"{0.001 * elapsedMilliseconds:F} s",
				_ => $"{Math.Floor(elapsedMilliseconds / 60_000)} min {Math.Round(0.001 * (elapsedMilliseconds % 60_000))} s",
			};

			if (useColor)
			{
				var color = elapsedMilliseconds switch
				{
					< 1 => Color.Blue,
					< 10 => Color.Green1,
					< 100 => Color.Lime,
					< 500 => Color.GreenYellow,
					< 1_000 => Color.Yellow1,
					< 10_000 => Color.OrangeRed1,
					_ => Color.Red1
				};

				return $"[{color}]{message}[/]";
			}
			else
			{
				return message;
			}
		}

		private static void RenderOverallResultsPanel(List<ElapsedTime> totalElapsedTime)
		{
			var totalConstructors = totalElapsedTime.Select(time => time.constructor).Sum();
			var totalPart1 = totalElapsedTime.Select(time => time.Part1).Sum();
			var totalPart2 = totalElapsedTime.Select(time => time.Part2).Sum();
			var total = totalPart1 + totalPart2 + totalConstructors;

			var grid = new Grid()
				.AddColumn(new GridColumn().NoWrap().PadRight(4))
				.AddColumn()
				.AddRow()
				.AddRow($"[bold] Total ({totalElapsedTime.Count}) days[/]", FormatTime(total));

			grid
				.AddRow($"Total Constructors: ", FormatTime(totalConstructors, useColor: false));

			grid
				.AddRow("Total part 1: ", FormatTime(totalPart1, useColor: false))
				.AddRow("Total part 2: ", FormatTime(totalPart2, false))
				.AddRow()
				.AddRow($"[bold] Mean time per day:[/] ", FormatTime(total / totalElapsedTime.Count));

			grid.AddRow("Mean constructors: ", FormatTime(totalElapsedTime.Select(t => t.constructor).Average()));

			grid
				.AddRow("Mean parts 1: ", FormatTime(totalElapsedTime.Select(t => t.Part1).Average()))
				.AddRow("Mean parts 2: ", FormatTime(totalElapsedTime.Select(t => t.Part2).Average()));

			AnsiConsole.Write(
				   new Panel(grid)
					   .Header("[b] Overall results [/]", Justify.Center));
		}
	}
}
