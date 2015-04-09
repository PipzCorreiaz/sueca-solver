using System;

namespace SuecaSolver
{
	public class MaxPlayer : Player
	{
		private int possibleGamesCounter = 0;

		public MaxPlayer(int id, Card[] hand) : base(id, hand)
		{
		}

		override public int PlayGame(GameState gameState)
		{
			if (gameState.IsEndGame())
			{
				possibleGamesCounter++;
				if (possibleGamesCounter < 2)
				{
					return gameState.EvalGame();
				} else {
					System.Environment.Exit(1);
				}
				return gameState.EvalGame();
			}

			int bestMove = 0;
			Card[] moves = PossibleMoves(gameState);
			foreach (Card move in moves)
			{
				gameState.ApplyMove(move);
				int moveValue = NextPlayer.PlayGame(gameState);
				if (moveValue > bestMove)
				{
					bestMove = moveValue;
				}
				gameState.UndoMove();
			}
			return bestMove;
		}

		override public int PlayTrick(GameState gameState)
		{
			if (gameState.IsEndTrick())
			{
				return gameState.EvalGame();
			}

			int bestMove = -10000;
			Card[] moves = PossibleMoves(gameState);
			foreach (Card move in moves)
			{
				gameState.ApplyMove(move);
				int moveValue = NextPlayer.PlayTrick(gameState);
				if (moveValue > bestMove)
				{
					bestMove = moveValue;
				}
				gameState.UndoMove();
			}
			return bestMove;
		}

		override public int PlayTrick(GameState gameState, Card move)
		{
			if (gameState.IsEndTrick())
			{
				return gameState.EvalTrick();
			}

			int bestMove = -10000;

			gameState.ApplyMove(move);
			int moveValue = NextPlayer.PlayTrick(gameState);
			if (moveValue > bestMove)
			{
				bestMove = moveValue;
			}
			gameState.UndoMove();

			return bestMove;
		}
	}
}