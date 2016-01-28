using System;
using System.Collections.Generic;

namespace SuecaSolver
{
    public class MaxNode : PlayerNode
    {

        public MaxNode(int id, List<int> hand)
            : base(id, hand)
        {
        }

        public override void ApplyMove(Move move)
        {
            if (move.PlayerId == Id)
            {
                Hand.Remove(move.Card);
            }
            else
            {
                //keep track of other players state
            }
        }

        public override void UndoMove(Move move)
        {
            if (move.PlayerId == Id)
            {
                Hand.Add(move.Card);
            }
            else
            {
                //keep track of other players state
            }
        }

        public override int PlayGame(PerfectInformationGame pig, int alpha, int beta, int depthLimit, int card)
        {
            int v = Int16.MinValue;
            List<int> cards;

            if (pig.reachedDepthLimit(depthLimit) || pig.IsOtherTeamWinning() || pig.IsEndGame())
            {
                return pig.EvalGame();
            }

            if (card == -1)
            {
                cards = Sueca.PossibleMoves(Hand, pig.GetLeadSuit());
                pig.orderPossibleMoves(cards, Id);
            }
            else
            {
                cards = new List<int>();
                cards.Add(card);
            }

            foreach (int c in cards)
            {
                Move move = new Move(Id, c);
                pig.ApplyMove(move);
                int moveValue = pig.GetNextPlayer().PlayGame(pig, alpha, beta, depthLimit);

                if (moveValue > v)
                {
                    v = moveValue;
                }

                pig.UndoMove(move);

                if (v >= beta)
                {
                    NumCuts++;
                    return v;
                }

                if (v > alpha)
                {
                    alpha = v;
                }
            }

            return v;
        }
    }
}