using System;

namespace SuecaSolver
{
	public class MinPlayer : Player
	{

		public MinPlayer(int id, Card[] hand) : base(id, hand)
		{
		}

		override public int PlayGame(GameState gameState, int alfa, int beta, Card card = null)
		{
			int worstMove = Int32.MaxValue;
			Card[] moves;

			if (gameState.IsEndGame())
			{
				return gameState.EvalGame();
			}

			if (card == null)
			{
				moves = SuecaGame.PossibleMoves(Hand, gameState.GetLeadSuit());
			} else {
				moves = new Card[1];
				moves[0] = card;
			}

			foreach (Card move in moves)
			{
				gameState.ApplyMove(new Move(Id, move));
				int moveValue = gameState.GetNextPlayer().PlayGame(gameState, alfa, beta);

				if (moveValue < worstMove)
				{
					worstMove = moveValue;
				}

				if (moveValue < beta) 
				{
					beta = moveValue;
				}

				gameState.UndoMove();

				if (worstMove <= alfa) 
				{
					// Console.WriteLine("Alfa prunning!");
					break;
				}
			}

			return worstMove;
		}

		// override public int PlayTrick(GameState gameState)
		// {
		// 	if (gameState.IsEndTrick())
		// 	{
		// 		return gameState.EvalTrick();
		// 	}

		// 	int bestMove = Int32.MaxValue;
		// 	Card[] moves = SuecaGame.PossibleMoves(Hand, gameState.GetLeadSuit());
		// 	foreach (Card move in moves)
		// 	{
		// 		gameState.ApplyMove(new Move(Id, move));
		// 		int moveValue = NextPlayer.PlayTrick(gameState);
		// 		if (moveValue < bestMove)
		// 		{
		// 			bestMove = moveValue;
		// 		}
		// 		gameState.UndoMove();
		// 	}
		// 	return bestMove;
		// }

		override public int PlayTrick(GameState gameState, Card card = null)
		{
			if (gameState.IsEndTrick())
			{
				return gameState.GetTrickPoints();
			}

			int bestMove = Int32.MaxValue;
			Card[] moves;

			if (card == null)
			{
				moves = SuecaGame.PossibleMoves(Hand, gameState.GetLeadSuit());
			} else {
				Console.WriteLine("WEIRD!");
				moves = new Card[1];
				moves[0] = card;
			}

			foreach (Card move in moves)
			{
				gameState.ApplyMove(new Move(Id, move));
				int moveValue = gameState.GetNextPlayer()
				.PlayTrick(gameState);
				if (moveValue < bestMove)
				{
					bestMove = moveValue;
				}
				gameState.UndoMove();
			}
			return bestMove;
		}
	}
}