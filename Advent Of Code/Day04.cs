using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Advent_Of_Code
{
	class Day04 : BaseDay
	{
		public override ValueTask<string> Solve1()
		{

			var data = Utils.ToStringArray(RawData);
			var bingo = Utils.ToIntArray(data[0], ",");

			int winningPoints = 0;

			List<BingoCard> cards = new();
			var idx = 0;

			var card = new BingoCard();
			for (int i = 2; i < data.Length; i++)
			{
				if (!string.IsNullOrWhiteSpace(data[i]))
				{
					card.SetCardRow(data[i].Replace("\r", ""), idx);
					idx++;
				}
				else
				{
					cards.Add(card);
					card = new BingoCard();
					idx = 0;
				}
			}

			cards.Add(card);
			var j = 5;
			for (j = 5; j < bingo.Length; j++)
			{
				int[] result = new int[j];
				Array.Copy(bingo, result, j);

				foreach (var bingoCard in cards)
				{
					if (bingoCard.IsWinning(result))
					{
						
						winningPoints = bingoCard.WinningPoints;
						return new ValueTask<string>(winningPoints.ToString());
					}
				}
			}

			
			return new ValueTask<string>(winningPoints.ToString());
		}

		public override ValueTask<string> Solve2()
		{
			var data = Utils.ToStringArray(RawData);
			var bingo = Utils.ToIntArray(data[0], ",");

			int winningPoints = 0;

			List<BingoCard> cards = new();
			var idx = 0;

			var card = new BingoCard();
			for (int i = 2; i < data.Length; i++)
			{
				if (!string.IsNullOrWhiteSpace(data[i]))
				{
					card.SetCardRow(data[i].Replace("\r", ""), idx);
					idx++;
				}
				else
				{
					cards.Add(card);
					card = new BingoCard();
					idx = 0;
				}
			}

			cards.Add(card);
			var j = 5;

			List<int> points = new();
			for (j = 5; j < bingo.Length; j++)
			{
				int[] result = new int[j];
				Array.Copy(bingo, result, j);

				for(int i = 0; i < cards.Count; i++)
				{
					if(cards[i].IsWinning(result))
					{
						winningPoints = cards[i].WinningPoints;
						cards.RemoveAt(i);
						points.Add(winningPoints);
						break;
					}
				}

				/*foreach (var bingoCard in cards)
				{
					if (bingoCard.IsWinning(result))
					{

						winningPoints = bingoCard.WinningPoints;
						cards.Remove(bingoCard);
					}
				}*/
			}

			return new ValueTask<string>(winningPoints.ToString());
		}
	}

	public class BingoCard
	{
		public int[,] Values { get; set; } = new int[5, 5];
		public int[] Bingo { get; set; }

		private int winningRow = -1;
		private int winningColumn = -1;

		public int WinningPoints { get; private set; } = -1;

		public void SetCardRow(string row, int idx)
		{
			var cells = row.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < Values.GetLength(0); i++)
			{
				Values[idx, i] = int.Parse(cells[i]);
			}
		}

		public bool IsWinning(int[] bingo)
		{
			Bingo = bingo;

			var rows = CheckRows();
			var columns = CheckColumns();

			if (rows.Item1 || columns.Item1)
			{
				winningRow = rows.Item2;
				winningColumn = columns.Item2;

				CalculateWinningPoints(bingo);

				return true;
			}

			return false;
		}

		private void CalculateWinningPoints(int[] bingo)
		{
			var finalPoint = bingo[bingo.Length - 1];

			if (winningRow != -1)
				for (int i = 0; i < Values.GetLength(0); i++)
					Values[winningRow, i] = 0;

			if (winningColumn != -1)
				for (int i = 0; i < Values.GetLength(1); i++)
					Values[i, winningColumn] = 0;

			for(int i = 0; i < Values.GetLength(0); i++)
			{
				for(int j = 0; j < Values.GetLength(1); j++)
				{
					if(bingo.Contains(Values[i, j]))
					{
						Values[i, j] = 0;
					}
				}
			}

			WinningPoints = Values.Cast<int>().Sum() * finalPoint;
		}

		public (bool, int) CheckRows()
		{
			for (int idx = 0; idx < Values.GetLength(0); idx++)
				if (!GetRow(idx).Except(Bingo).Any())
					return (true, idx);

			return (false, -1);
		}

		private (bool, int) CheckColumns()
		{
			for (int idx = 0; idx < Values.GetLength(1); idx++)
				if (!GetColumn(idx).Except(Bingo).Any())
					return (true, idx);

			return (false, -1);
		}

		private int[] GetRow(int rowIndex)
			=> Enumerable.Range(0, Values.GetLength(1))
				.Select(x => Values[rowIndex, x])
				.ToArray();

		private int[] GetColumn(int columnIndex)
			=> Enumerable.Range(0, Values.GetLength(0))
				.Select(x => Values[x, columnIndex])
				.ToArray();

		public void PrintCard()
		{
			for (int i = 0; i < Values.GetLength(1); i++)
			{
				for (int j = 0; j < Values.GetLength(0); j++)
				{
					Console.Write(Values[i, j] + "\t");
				}

				Console.Write("\n");
			}
		}
	}
}
