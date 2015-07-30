using System;
using System.Collections.Generic;

namespace SuecaSolver
{
    public class Trick
    {

        public int LeadSuit;
        private List<Move> moves;
        private int trump;

        public Trick(int trumpSuit)
        {
            moves = new List<Move>(4);
            LeadSuit = (int)Suit.None;
            trump = trumpSuit;
        }

        public List<Move> GetMoves()
        {
            return moves;
        }

        //nome infeliz para o metodo!
        public int GetTrickSize()
        {
            return moves.Count;
        }

        public void ApplyMove(Move move)
        {
            if (moves.Count == 0)
            {
                LeadSuit = Card.GetSuit(move.Card);
            }

            moves.Add(move);
        }

        public void UndoMove()
        {
            int currentMove = moves.Count - 1;
            moves.RemoveAt(currentMove);
        }

        public int GetLastPlayerId()
        {
            if (moves.Count == 0)
            {
                Console.WriteLine("Trick trouble at GetLastPlayerId!!!");
                return -1;
            }
            return moves[moves.Count - 1].PlayerId;
        }


        public bool IsEmpty()
        {
            if (moves.Count == 0)
            {
                return true;
            }
            return false;
        }

        public bool IsFull()
        {
            if (moves.Count == moves.Capacity)
            {
                return true;
            }
            return false;
        }


        public int HighestCardOnCurrentTrick()
        {
            int result = 0;
            bool cut = false;
            foreach (var move in moves)
            {
                int card = move.Card;
                int cardSuit = Card.GetSuit(card);
                int cardRank = Card.GetRank(card);

                if (!cut)
                {
                    if (cardRank > result)
                    {
                        result = cardRank;
                    }
                    else if (cardRank < 0)
                    {
                        result = cardRank;
                        cut = true;
                    }
                }
                else if (cut && cardRank < result)
                {
                    result = cardRank;
                }
            }
            return result;
        }

        public void PrintTrick()
        {
            foreach (Move m in moves)
            {
                Console.WriteLine(m);
            }
        }

        public int[] GetTrickWinnerAndPoints()
        {
            int winningSuit = Card.GetSuit(moves[0].Card);
            int highestValueFromWinningSuit = Card.GetValue(moves[0].Card);
            int highestRankFromWinningSuit = Card.GetRank(moves[0].Card);
            int winningPlayerId = moves[0].PlayerId;
            int points = highestValueFromWinningSuit;


            for (int i = 1; i < moves.Count; i++)
            {
                int moveSuit = Card.GetSuit(moves[i].Card);
                int moveRank = Card.GetRank(moves[i].Card);
                int moveValue = Card.GetValue(moves[i].Card);

                if (moveSuit == trump && winningSuit != trump)
                {
                    winningSuit = trump;
                    highestValueFromWinningSuit = moveValue;
                    highestRankFromWinningSuit = moveRank;
                    winningPlayerId = moves[i].PlayerId;
                }
                else if (moveSuit == winningSuit &&
                         moveValue >= highestValueFromWinningSuit &&
                         moveRank > highestRankFromWinningSuit)
                {
                    highestValueFromWinningSuit = moveValue;
                    highestRankFromWinningSuit = moveRank;
                    winningPlayerId = moves[i].PlayerId;
                }

                points += moveValue;
            }

            if (winningPlayerId == 1 || winningPlayerId == 3)
            {
                points = -1 * points;
            }
            return new int[] { winningPlayerId, points };
        }
    }
}