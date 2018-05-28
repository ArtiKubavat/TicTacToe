using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using TicTacToe.Interfaces;
using TicTacToe.Services;

namespace TicTacToe
{
	public class GameModule : NinjectModule
	{
		public override void Load()
		{
			Bind<IGameService>().To<GameService>();
			Bind<IBoardService>().To<BoardService>();
		}
	}
}
