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
		private List<BingoCard> cards = new();

		public override ValueTask<string> Solve1()
		{
			var data = Utils.ToStringArray(RawData);
			var bingoNumbers = Utils.ToIntArray(data[0], ",");

			var cardNumbers = data.ToList();
			cardNumbers.RemoveAt(0);
			cardNumbers.RemoveAt(0);

			GenerateBingoCards(cardNumbers.ToArray());

			foreach (var number in bingoNumbers)
			{
				foreach (var card in cards)
				{
					card.RegisterNumber(number);

					if (CheckForBingo(number, card))
						return new ValueTask<string>((card.CalculatePoints() * number).ToString());
				}
			}

			throw new NotImplementedException();
		}

		public override ValueTask<string> Solve2()
		{
			var data = Utils.ToStringArray(RawData);
			var bingoNumbers = Utils.ToIntArray(data[0], ",");

			List<long> scores = new();
			cards = new();

			var cardNumbers = data.ToList();
			cardNumbers.RemoveAt(0);
			cardNumbers.RemoveAt(0);

			GenerateBingoCards(cardNumbers.ToArray());

			foreach (var number in bingoNumbers)
			{
				foreach (var card in cards)
				{
					card.RegisterNumber(number);

					if (!card.Done && card.CheckForBingo())
					{
						scores.Add(card.CalculatePoints() * number);
						card.Done = true;
					}
						
				}
			}

			long score = scores.Where(x => x > 0).LastOrDefault();

			return new ValueTask<string>(score.ToString());
		}

		private void GenerateBingoCards(string[] data)
		{
			var card = new BingoCard();
			var idx = 0;

			for (int i = 0; i < data.Length; i++)
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
		}

		private bool CheckForBingo(int number, BingoCard card)
			=> card.CheckForBingo();


	}

	public class BingoCard
	{
		private int[,] Board { get; set; }
		public bool Done { get; set; } = false;
		public int WinningNumber { get; set; }

		public BingoCard()
		{
			Board = new int[5, 5];
		}

		public void SetCardRow(string row, int idx)
		{
			var cells = row.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);

			for (int i = 0; i < 5; i++)
				Board[idx, i] = int.Parse(cells[i]);
		}

		public void RegisterNumber(int n)
		{
			if (Done)
				return;

			for (int row = 0; row <= 4; row++)
				for (int col = 0; col <= 4; col++)
					if (Board[row, col] == n)
						Board[row, col] = -1;
		}

		public bool CheckForBingo()
		{
			for (int row = 0; row <= 4; row++)
			{
				int marked = 0;
				for (int col = 0; col <= 4; col++)
					if (Board[row, col] == -1)
						marked++;

				if (marked == 5)
					return true;
					

			}

			for (int row = 0; row <= 4; row++)
			{
				int marked = 0;
				for (int col = 0; col <= 4; col++)
					if (Board[col, row] == -1)
						marked++;

				if (marked == 5)
					return true;
			}

			return false;
		}

		public long CalculatePoints()
		{
			long total = 0;

			for (int row = 0; row < 5; row++)
				for (int col = 0; col < 5; col++)
					if (Board[row, col] != -1)
						total += Board[row, col];

			return total;
		}
	}
}