using System;
using Ninject;

using TicTacToe.Constants;
using TicTacToe.Interfaces;

namespace TicTacToe
{
	class Program
	{
		private static IGameService _gameService;

		static void Main(string[] args)
		{
			PlayGame();
		}

		private static void PlayGame()
		{
			try
			{
				InitServices();

				while (true)
				{
					string input = _gameService.ReadInputFromPlayer();

					if (input == GameConstants.GIVEUP || input == GameConstants.GIVEUP.ToUpper())
					{
						return;
					}

					Tuple<int, int> parsedInput = _gameService.ParseAndValidateInput(input);

					if (parsedInput.Item1 != -1)
					{
						var gameState = _gameService.MakeAMove(parsedInput.Item1, parsedInput.Item2);

						switch (gameState)
						{
							case GameState.Win:
								Console.WriteLine("Well done you've won the game!");
								ReplayGame();
								break;
							case GameState.InValid:
								Console.WriteLine("Oops that spot is taken. Try again");
								continue;
							case GameState.Draw:
								Console.WriteLine("There's a Draw");
								ReplayGame();
								break;
							case GameState.NotFinished:
								break;
						}
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		private static void ReplayGame()
		{
			Console.WriteLine("\nPlease Press r to replay the game or q to give up");
			string option = Console.ReadLine();

			if (option == GameConstants.REPLAY || option == GameConstants.REPLAY.ToUpper())
			{
				Console.Clear();
				PlayGame();
			}
			else if (option == GameConstants.GIVEUP || option == GameConstants.GIVEUP.ToUpper())
			{
				Environment.Exit(0);
			}
			else
			{
				Console.WriteLine("\nPlease valid input");
				ReplayGame();
			}
		}


		private static void InitServices()
		{
			StandardKernel kernel = new StandardKernel();
			kernel.Load(new GameModule());

			_gameService = kernel.Get<IGameService>();
		}
	}
}
