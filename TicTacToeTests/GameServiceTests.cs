using System;
using Moq;
using NUnit.Framework;

using TicTacToe.Constants;
using TicTacToe.Entity;
using TicTacToe.Interfaces;
using TicTacToe.Services;
using TicTacToeTests.Helpers;

namespace TicTacToeTests
{

	[TestFixture]
	public class GameServiceTests
	{
		private Mock<IBoardService> _boardServiceMock;
		private GameService _gameService;

		[SetUp]
		public void Setup()
		{
			_boardServiceMock = new Mock<IBoardService>();
			_gameService = new GameService(_boardServiceMock.Object);
		}

		[Test]
		public void Check_default_has_been_properly_set_on_initialisation_of_GameService()
		{
			//Checking Player 1 is the default Player
			Assert.AreEqual(_gameService._currentPlayer.PlayerName, GameConstants.PLAYER1);
			Assert.AreEqual(_gameService._currentPlayer.PlayerSign, GameConstants.PLAYER1_SIGN);

			//Checking default coordinates values
			Assert.IsTrue(_gameService._defaultCoordValues.ContainsValue(0));
			Assert.IsTrue(_gameService._defaultCoordValues.ContainsValue(1));
			Assert.IsTrue(_gameService._defaultCoordValues.ContainsValue(-1));
		}


		[Test]
		public void MakeAMove_should_make_a_move_and_return_GameState_NotFinished()
		{
			string[,] currentBoard = GameHelper.SetBoardSizeAndValues(3, GameConstants.DEFAULT_SIGN);
			_boardServiceMock.Setup(x => x.GetCurrentBoard()).Returns(currentBoard);

			GameState gameState = _gameService.MakeAMove(0,2);


			//Checking Game is not finished yet
			Assert.AreEqual(GameState.NotFinished, gameState);

			//Current Board has been updated
			Assert.AreEqual(_gameService._gameBoard[0,2], GameConstants.PLAYER1_SIGN);

			//It's turn for Player 2
			Assert.AreEqual(_gameService._currentPlayer.PlayerName, GameConstants.PLAYER2);
			Assert.AreEqual(_gameService._currentPlayer.PlayerSign, GameConstants.PLAYER2_SIGN);
		}


		[Test]
		public void MakeAMove_should_return_with_Invalid_status()
		{
			string[,] currentBoard = GameHelper.SetBoardSizeAndValues(3, GameConstants.DEFAULT_SIGN);
			currentBoard[0,2] = GameConstants.PLAYER1_SIGN;
			_boardServiceMock.Setup(x => x.GetCurrentBoard()).Returns(currentBoard);

			GameState gameState = _gameService.MakeAMove(0,2);
			
			Assert.AreEqual(gameState, GameState.InValid);
		}

		[Test]
		public void ParseAndValidateInput_should_return_Invalid_Input_Coordinates_when_input_is_a_random_string()
		{
			string input = "fdsfdgd";

			Tuple<int, int> resulTuple = _gameService.ParseAndValidateInput(input);

			Assert.AreEqual(resulTuple.Item1, -1);
			Assert.AreEqual(resulTuple.Item2, -1);
		}

		[Test]
		public void ParseAndValidateInput_should_return_Invalid_Input_Coordinates_when_input_is_a_random_numbers()
		{
			string input = "2456486423468";

			Tuple<int, int> resulTuple = _gameService.ParseAndValidateInput(input);

			Assert.AreEqual(resulTuple.Item1, -1);
			Assert.AreEqual(resulTuple.Item2, -1);
		}

		[Test]
		public void ParseAndValidateInput_should_return_Invalid_Input_Coordinates_when_input_is_a_random_coordinates()
		{
			string input = "1,3435";

			Tuple<int, int> resulTuple = _gameService.ParseAndValidateInput(input);

			Assert.AreEqual(resulTuple.Item1, -1);
			Assert.AreEqual(resulTuple.Item2, -1);
		}

		[Test]
		public void ParseAndValidateInput_should_return_valid_Input_Coordinates_when_input_is_valid_with_space()
		{
			string input = "1, 2"; //coordinates with space

			Tuple<int, int> resulTuple = _gameService.ParseAndValidateInput(input);

			Assert.AreEqual(resulTuple.Item1, 1);
			Assert.AreEqual(resulTuple.Item2, 2);
		}

		[Test]
		public void ParseAndValidateInput_should_return_valid_Input_Coordinates_when_input_is_valid_without_space()
		{
			string input = "1,2"; //coordinates without space

			Tuple<int, int> resulTuple = _gameService.ParseAndValidateInput(input);

			Assert.AreEqual(resulTuple.Item1, 1);
			Assert.AreEqual(resulTuple.Item2, 2);
		}

		[Test]
		public void ValidateInput_should_return_true_when_coordinates_are_in_range()
		{
			int coordiante = 2;

			bool isValid =_gameService.ValidateInput(coordiante);

			Assert.IsTrue(isValid);
		}

		[Test]
		public void ValidateInput_should_return_true_when_coordinates_are_out_of_range()
		{
			int coordiante = 10;

			bool isValid = _gameService.ValidateInput(coordiante);

			Assert.IsFalse(isValid);
		}

		[Test]
		public void CheckGameState_should_return_GameState_NotFinished_when_board_is_not_fully_filled()
		{
			string[,] currentBoard = GameHelper.SetBoardSizeAndValues(3, GameConstants.DEFAULT_SIGN);
			currentBoard[0,2] = GameConstants.PLAYER1_SIGN;
			currentBoard[2,2] = GameConstants.PLAYER2_SIGN;
			currentBoard[1,0] = GameConstants.PLAYER1_SIGN;
			currentBoard[0,1] = GameConstants.PLAYER2_SIGN;

			_gameService.SetCurrentPlayer(GameConstants.PLAYER1);
			_gameService._gameBoard = currentBoard;
			GameState resultState = _gameService.CheckGameState();

			Assert.AreEqual(GameState.NotFinished, resultState);
			
			//If Game state is not finished then Player switches
			Assert.AreEqual(GameConstants.PLAYER2, _gameService._currentPlayer.PlayerName);
			Assert.AreEqual(GameConstants.PLAYER2_SIGN, _gameService._currentPlayer.PlayerSign);
		}

		[Test]
		public void CheckGameState_should_return_GameState_Draw_when_board_is_fully_filled_but_no_one_won()
		{
			string[,] currentBoard = GameHelper.SetBoardSizeAndValues(3, GameConstants.PLAYER1_SIGN);
			currentBoard[0,0] = GameConstants.PLAYER2_SIGN;
			currentBoard[0,2] = GameConstants.PLAYER2_SIGN;
			currentBoard[1,2] = GameConstants.PLAYER2_SIGN;
			currentBoard[2,1] = GameConstants.PLAYER2_SIGN;

			_gameService._gameBoard = currentBoard;
			GameState resultState = _gameService.CheckGameState();

			Assert.AreEqual(GameState.Draw, resultState);
		}


		[Test]
		public void CheckGameState_should_return_GameState_Win_when_Forward_diagonal_coordinates_matches()
		{
			string[,] currentBoard = GameHelper.SetBoardSizeAndValues(3, GameConstants.PLAYER1_SIGN);
			currentBoard[0,0] = GameConstants.PLAYER2_SIGN;
			currentBoard[1,1] = GameConstants.PLAYER2_SIGN;
			currentBoard[2,2] = GameConstants.PLAYER2_SIGN;

			_gameService._gameBoard = currentBoard;
			GameState resultState = _gameService.CheckGameState();

			Assert.AreEqual(GameState.Win, resultState);
		}

		[Test]
		public void CheckGameState_should_return_GameState_Win_when_Backward_diagonal_coordinates_matches()
		{
			string[,] currentBoard = GameHelper.SetBoardSizeAndValues(3, GameConstants.PLAYER1_SIGN);
			currentBoard[0,2] = GameConstants.PLAYER2_SIGN;
			currentBoard[1,1] = GameConstants.PLAYER2_SIGN;
			currentBoard[2,0] = GameConstants.PLAYER2_SIGN;

			_gameService._gameBoard = currentBoard;
			GameState resultState = _gameService.CheckGameState();

			Assert.AreEqual(GameState.Win, resultState);
		}

		[Test]
		public void CheckGameState_should_return_GameState_Win_when_Column_coordinates_matches()
		{
			string[,] currentBoard = GameHelper.SetBoardSizeAndValues(3, GameConstants.PLAYER1_SIGN);
			currentBoard[0,1] = GameConstants.PLAYER2_SIGN;
			currentBoard[1,1] = GameConstants.PLAYER2_SIGN;
			currentBoard[2,1] = GameConstants.PLAYER2_SIGN;

			_gameService._gameBoard = currentBoard;
			GameState resultState = _gameService.CheckGameState();

			Assert.AreEqual(GameState.Win, resultState);
		}

		[Test]
		public void CheckGameState_should_return_GameState_Win_when_Row_coordinates_matches()
		{
			string[,] currentBoard = GameHelper.SetBoardSizeAndValues(3, GameConstants.PLAYER1_SIGN);
			currentBoard[0,0] = GameConstants.PLAYER2_SIGN;
			currentBoard[0,1] = GameConstants.PLAYER2_SIGN;
			currentBoard[0,2] = GameConstants.PLAYER2_SIGN;

			_gameService._gameBoard = currentBoard;
			GameState resultState = _gameService.CheckGameState();

			Assert.AreEqual(GameState.Win, resultState);
		}

		[Test]
		public void SetCurrentPlayer_should_set_current_player_and_sign()
		{
			Player player = new Player();
			 player = _gameService.SetCurrentPlayer(GameConstants.PLAYER1);

			Assert.AreEqual(player.PlayerName, GameConstants.PLAYER1);
			Assert.AreEqual(player.PlayerSign, GameConstants.PLAYER1_SIGN);
		}



	}
}
