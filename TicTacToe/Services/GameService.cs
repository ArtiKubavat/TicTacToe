using System;
using System.Collections.Generic;
using TicTacToe.Constants;
using TicTacToe.Entity;
using TicTacToe.Interfaces;

namespace TicTacToe.Services
{
	class GameService : IGameService
	{
		private static IBoardService _boardService;
		private static string[,] _gameBoard;
		private Player _currentPlayer;

		Dictionary<string, int> dictionary = new Dictionary<string, int>();

		public GameService(IBoardService boardService)
		{
			_boardService = boardService;
			SetDefaultValuesForCoodinates();
			_currentPlayer = SetCurrentPlayer(GameConstants.PLAYER1);
		}

		public void SetDefaultValuesForCoodinates()
		{
			dictionary = new Dictionary<string, int>
			{
				{ GameConstants.DEFAULT_SIGN, 0 },
				{ GameConstants.PLAYER1_SIGN, 1 },
				{ GameConstants.PLAYER2_SIGN, -1 }
			};
		}

		public GameState MakeAMove(int x, int y)
		{
			_gameBoard = _boardService.GetCurrentBoard();
			if (_gameBoard[x, y] == GameConstants.DEFAULT_SIGN)
			{
				_gameBoard[x, y] = _currentPlayer.PlayerSign;
				_boardService.UpdateAndPrintBoard(_gameBoard);

				return CheckGameState();
			}

			return GameState.InValid;
		}

		public Tuple<int, int> ParseAndValidateInput(string nextMove)
		{
			try
			{

				if (nextMove != GameConstants.GIVEUP && !nextMove.Contains(","))
				{
					return InvalidInputMessage();
				}

				string[] positions = nextMove.Split(',');
				if (positions[0] == string.Empty && positions[1] == string.Empty)
				{
					return InvalidInputMessage();
				}

				int coordinateX = Convert.ToInt16(positions[0]);
				int coordinateY = Convert.ToInt16(positions[1]);

				if (!(ValidateInput(coordinateX) && ValidateInput(coordinateY)))
				{
					return new Tuple<int, int>(-1, -1);
				}
				return new Tuple<int, int>(coordinateX, coordinateY);
			}
			catch (Exception e)
			{
				Console.WriteLine("InValid input " + e);
				throw;
			}
		}

		private static Tuple<int, int> InvalidInputMessage()
		{
			Console.WriteLine("Oops! Invalid input. Please try again...");
			return new Tuple<int, int>(-1, -1);
		}

		public bool ValidateInput(int coordinate)
		{
			if (coordinate >= 0 && coordinate <= 2)
			{
				return true;
			}
			else
			{
				Console.WriteLine("Oops! Invalid move. Please try again...");
				return false;
			}
		}

		public GameState CheckGameState()
		{
			var gameState = CheckRowWin();
			if (gameState != GameState.NotFinished)
			{
				return gameState;
			}

			gameState = CheckColumnWin();
			if (gameState != GameState.NotFinished)
			{
				return gameState;
			}

			gameState = CheckDiagonal();

			if (gameState != GameState.Win)
				gameState = CheckTie();

			_currentPlayer = _currentPlayer.PlayerName == GameConstants.PLAYER1 && gameState == GameState.NotFinished
				? SetCurrentPlayer(GameConstants.PLAYER2)
				: SetCurrentPlayer(GameConstants.PLAYER1);

			return gameState;
		}

		private GameState CheckTie()
		{
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					if (_gameBoard[i, j] == GameConstants.DEFAULT_SIGN)
					{
						return GameState.NotFinished;
					}
				}
			}
			return GameState.Tie;
		}

		private GameState CheckDiagonal()
		{
			var gameState = CheckForwardDiagonal();
			if (gameState != GameState.NotFinished)
			{
				return gameState;
			}

			return CheckBackwardDiagonal();
		}

		private GameState CheckBackwardDiagonal()
		{
			var sum = 0;
			for (int i = 0; i < 3; i++)
			{
				sum += dictionary[_gameBoard[i, 2 - i]];
			}

			switch (sum)
			{
				case 3:
					return GameState.Win;
				case -3:
					return GameState.Win;
				default:
					return GameState.NotFinished;
			}
		}

		private GameState CheckForwardDiagonal()
		{
			var sum = 0;
			for (int i = 0; i < 3; i++)
			{
				sum += dictionary[_gameBoard[i, i]];
			}
			switch (sum)
			{
				case 3:
					return GameState.Win;
				case -3:
					return GameState.Win;
				default:
					return GameState.NotFinished;
			}

		}

		private GameState CheckColumnWin()
		{
			for (int i = 0; i < 3; i++)
			{
				var sum = 0;

				for (int j = 0; j < 3; j++)
				{
					sum += dictionary[_gameBoard[j, i]];
				}
				switch (sum)
				{
					case 3:
						return GameState.Win;
					case -3:
						return GameState.Win;
				}
			}
			return GameState.NotFinished;
		}

		private GameState CheckRowWin()
		{
			for (int i = 0; i < 3; i++)
			{
				var sum = 0;

				for (int j = 0; j < 3; j++)
				{
					sum += dictionary[_gameBoard[i, j]];
				}
				switch (sum)
				{
					case 3:
						return GameState.Win;
					case -3:
						return GameState.Win;
				}
			}
			return GameState.NotFinished;
		}

		public Player SetCurrentPlayer(string currentPlayer)
		{
			Player player = new Player();

			switch (currentPlayer)
			{
				case GameConstants.PLAYER1:
					player.PlayerName = GameConstants.PLAYER1;
					player.PlayerSign = GameConstants.PLAYER1_SIGN;
					break;

				case GameConstants.PLAYER2:
					player.PlayerName = GameConstants.PLAYER2;
					player.PlayerSign = GameConstants.PLAYER2_SIGN;
					break;
			}

			return player;
		}

		public string ReadInputFromPlayer()
		{
			Console.Write($"\n{_currentPlayer.PlayerName} enter a coord x,y to place your {_currentPlayer.PlayerSign}or enter 'q' to give up: ");
			return Console.ReadLine();
		}
	}

}
