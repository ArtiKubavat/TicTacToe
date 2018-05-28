using TicTacToe.Entity;

namespace TicTacToe.Interfaces
{
	interface IBoardService
	{
		string[,] GetCurrentBoard();
		void UpdateAndPrintBoard(string [,] board);
		
	}
}
