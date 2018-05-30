using System;
using TicTacToe.Constants;

namespace TicTacToeTests.Helpers
{
	public class GameHelper
	{
		public static string[,] SetBoardSizeAndValues(int size, string value)
		{
			string[,] _board = new string[size, size];

			for (int i = 0; i < size; i++)
			{
				Console.Write("\n");
				for (int j = 0; j < size; j++)
				{
					_board[i, j] = value;
				}
			}

			return _board;
		}
	}
}
