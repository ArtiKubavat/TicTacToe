using System;
using System.Collections.Generic;
using NLog;

using TicTacToe.Constants;
using TicTacToe.Entity;
using TicTacToe.Interfaces;

namespace TicTacToe.Services
{
	public class GameService : IGameService
	{
		private static IBoardService _boardService;
		public string[,] _gameBoard;
		public Player _currentPlayer;

		public Dictionary<string, int> _defaultCoordValues = new Dictionary<string, int>();
		private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

		public GameService(IBoardService boardService)
		{
			_boardService = boardService;
			SetDefaultValuesForCoodinates();
			_currentPlayer = SetCurrentPlayer(GameConstants.PLAYER1);
		}

		public void SetDefaultValuesForCoodinates()
		{
			_defaultCoordValues = new Dictionary<string, int>
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
				Logger.Error(e);
				return InvalidInputMessage();
			}
		}

		public Tuple<int, int> InvalidInputMessage()
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
			if (gameState == GameState.Win)
			{
				return gameState;
			}

			gameState = CheckColumnWin();
			if (gameState == GameState.Win)
			{
				return gameState;
			}

			gameState = CheckDiagonal();

			if (gameState == GameState.NotFinished)
				gameState = CheckDraw();

			_currentPlayer = _currentPlayer.PlayerName == GameConstants.PLAYER1 && gameState == GameState.NotFinished
				? SetCurrentPlayer(GameConstants.PLAYER2)
				: SetCurrentPlayer(GameConstants.PLAYER1);

			return gameState;
		}

		private GameState CheckDraw()
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
			return GameState.Draw;
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
				sum += _defaultCoordValues[_gameBoard[i, 2 - i]];
			}

			return ReturnGameState(sum);
		}

		public static GameState ReturnGameState(int sum)
		{
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
				sum += _defaultCoordValues[_gameBoard[i, i]];
			}

			return ReturnGameState(sum);
		}

		private GameState CheckColumnWin()
		{
			for (int i = 0; i < 3; i++)
			{
				var sum = 0;

				for (int j = 0; j < 3; j++)
				{
					sum += _defaultCoordValues[_gameBoard[j, i]];
				}

				return ReturnGameState(sum);
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
					sum += _defaultCoordValues[_gameBoard[i, j]];
				}

				return ReturnGameState(sum);
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
