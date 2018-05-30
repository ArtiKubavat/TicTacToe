using TicTacToe.Entity;

namespace TicTacToe.Interfaces
{
	public interface IBoardService
	{
		string[,] GetCurrentBoard();
		void UpdateAndPrintBoard(string [,] board);
		
	}
}
