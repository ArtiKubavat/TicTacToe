using System;
using TicTacToe.Constants;
using TicTacToe.Interfaces;

namespace TicTacToe.Services
{
	public class BoardService : IBoardService
	{
		public string[,] _board;

		public BoardService()
		{
			_board = InitialiseBoard(GameConstants.BOARD_SIZE);
		}

		public string[,] InitialiseBoard(int size)
		{
			Console.WriteLine("Welcome to Tic Tac Toe!");
			Console.WriteLine("Here's the current board:");

			_board = new string[size, size];

			for (int i = 0; i < size; i++)
			{
				Console.Write("\n");
				for (int j = 0; j < size; j++)
				{
					_board[i, j] = GameConstants.DEFAULT_SIGN;
					Console.Write(_board[i, j]);
				}
			}

			Console.Write("\n\n");
			return _board;
		}

		public string[,] GetCurrentBoard()
		{
			return _board;
		}

		public void UpdateAndPrintBoard(string[,] board)
		{
			_board = board;
			Console.Write("Move accepted, here's the current board:");

			for (int i = 0; i < 3; i++)
			{
				Console.Write("\n");
				for (int j = 0; j < 3; j++)
				{
					Console.Write(_board[i, j]);
				}
			}

			Console.Write("\n");
		}
	}
}
