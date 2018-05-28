using System;
using TicTacToe.Constants;
using TicTacToe.Entity;

namespace TicTacToe.Interfaces
{
	interface IGameService
	{
		GameState MakeAMove(int x, int y);
		Tuple<int, int> ParseAndValidateInput(string nextMove);
		GameState CheckGameState();
		string ReadInputFromPlayer();
	}
}
