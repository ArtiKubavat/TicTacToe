using NUnit.Framework;

using TicTacToe.Services;
using TicTacToe.Constants;


namespace TicTacToeTests
{
	[TestFixture]
	public class BoardServiceTests
	{
		private BoardService _boardService;

		[SetUp]
		public void Setup()
		{
			_boardService = new BoardService();
		}


		[Test]
		public void GameBoard_inialises_with_default_constant_size_and_symbol()
		{
			Assert.AreEqual(_boardService._board.Length, GameConstants.BOARD_SIZE * GameConstants.BOARD_SIZE);
			Assert.AreEqual(_boardService._board[0, 0], GameConstants.DEFAULT_SIGN);
			Assert.AreEqual(_boardService._board[1, 2], GameConstants.DEFAULT_SIGN);
			Assert.AreEqual(_boardService._board[2, 2], GameConstants.DEFAULT_SIGN);
		}

		[Test]
		public void InitialiseBoard_should_give_you_nXn_array_board()
		{
			string[,] originalGameBoard = _boardService._board;
			_boardService.InitialiseBoard(5);

			Assert.AreNotEqual(originalGameBoard.Length, _boardService._board.Length);
			Assert.AreEqual(_boardService._board.Length, 25);
			Assert.AreEqual(_boardService._board[0, 0], GameConstants.DEFAULT_SIGN);
			Assert.AreEqual(_boardService._board[3, 4], GameConstants.DEFAULT_SIGN);
			Assert.AreEqual(_boardService._board[4, 2], GameConstants.DEFAULT_SIGN);
		}

		[Test]
		public void UpdateAndPrintBoard_should_update_and_print_the_board()
		{
			string[,] newBoard = new string[GameConstants.BOARD_SIZE, GameConstants.BOARD_SIZE];
			newBoard[2, 1] = GameConstants.PLAYER1_SIGN;
			newBoard[0, 0] = GameConstants.PLAYER2_SIGN;

			_boardService.UpdateAndPrintBoard(newBoard);

			Assert.AreEqual(_boardService._board[2, 1], newBoard[2, 1]);
			Assert.AreEqual(_boardService._board[0, 0], newBoard[0, 0]);
			Assert.AreNotEqual(_boardService._board[2, 2], newBoard[2, 1]);
			Assert.AreNotEqual(_boardService._board[0, 2], newBoard[0, 0]);
		}
	}
}
