using System;
using System.Collections.Generic;

namespace SuecaSolver
{
    public class InformationSet
    {
        private int id;
        private List<int> hand;
        public int Trump;
        private Deck deck;
        private Dictionary<int, List<int>> suitHasPlayer;
        private Dictionary<int, List<int>> othersPointCards;
        public int MyTeamPoints;
        public int OtherTeamPoints;
        private int remainingTrumps;
        private List<Trick> tricks;


        public InformationSet(int id, List<int> currentHand, int trumpSuit)
        {
            this.id = id;
            Trump = trumpSuit;
            hand = new List<int>(currentHand);
            suitHasPlayer = new Dictionary<int,List<int>>
            {
                { (int)Suit.Clubs, new List<int>(4){ 0, 1, 2, 3 } },
                { (int)Suit.Diamonds, new List<int>(4){ 0, 1, 2, 3 } },
                { (int)Suit.Hearts, new List<int>(4){ 0, 1, 2, 3 } },
                { (int)Suit.Spades, new List<int>(4){ 0, 1, 2, 3 } }
            };
            othersPointCards = new Dictionary<int, List<int>>
            {
                { (int)Suit.Clubs, new List<int>(5){ (int) Rank.Ace, (int) Rank.Seven, (int) Rank.King, (int) Rank.Jack, (int) Rank.Queen } },
                { (int)Suit.Diamonds, new List<int>(5){ (int) Rank.Ace, (int) Rank.Seven, (int) Rank.King, (int) Rank.Jack, (int) Rank.Queen } },
                { (int)Suit.Hearts, new List<int>(5){ (int) Rank.Ace, (int) Rank.Seven, (int) Rank.King, (int) Rank.Jack, (int) Rank.Queen } },
                { (int)Suit.Spades, new List<int>(5){ (int) Rank.Ace, (int) Rank.Seven, (int) Rank.King, (int) Rank.Jack, (int) Rank.Queen } }
            };

            //remove my point cards from the dictionary othersPointCards
            for (int i = 0; i < hand.Count; i++)
            {
                int card = hand[i];
                int suit = Card.GetSuit(card);
                int rank = Card.GetRank(card);
                othersPointCards[suit].Remove(rank);
            }
            suitHasPlayer[(int)Suit.Clubs].Remove(id);
            suitHasPlayer[(int)Suit.Diamonds].Remove(id);
            suitHasPlayer[(int)Suit.Hearts].Remove(id);
            suitHasPlayer[(int)Suit.Spades].Remove(id);
            deck = new Deck(currentHand);
            MyTeamPoints = 0;
            OtherTeamPoints = 0;
            remainingTrumps = 10;
            
            tricks = new List<Trick>();
            tricks.Add(new Trick(trumpSuit));
        }


        public int GetHandSize()
        {
            return hand.Count;
        }


        public List<int> GetPossibleMoves()
        {
            Trick currentTrick = tricks[tricks.Count - 1];
            return Sueca.PossibleMoves(hand, currentTrick.LeadSuit);
        }

        public void AddPlay(int playerID, int card)
        {
            Trick currentTrick = tricks[tricks.Count - 1];
            if (currentTrick.IsFull())
            {
                int[] winnerPoints = currentTrick.GetTrickWinnerAndPoints();
                if (winnerPoints[0] == id || winnerPoints[0] == (id + 2) % 4)
                {
                    MyTeamPoints += winnerPoints[1];
                }
                else
                {
                    //TODO checks valence of points!!!
                    OtherTeamPoints += winnerPoints[1];
                }
                tricks.Add(new Trick(Trump));
                currentTrick = tricks[tricks.Count - 1];
            }
            currentTrick.ApplyMove(new Move(playerID, card));

            //check if player has the leadSuit
            int leadSuit = currentTrick.LeadSuit;
            int cardSuit = Card.GetSuit(card);
            int cardValue = Card.GetValue(card);
            if (playerID != id && cardSuit != leadSuit && leadSuit != (int)Suit.None)
            {
                if (suitHasPlayer[leadSuit].Contains(playerID))
                {
                    suitHasPlayer[leadSuit].Remove(playerID);
                }
                //else
                //{
                //    Console.WriteLine("InformationSet:AddPlay >> Player has renounced");
                //    suitHasPlayer = new Dictionary<int, List<int>>
                //    {
                //        { (int)Suit.Clubs, new List<int>(4){ 0, 1, 2, 3 } },
                //        { (int)Suit.Diamonds, new List<int>(4){ 0, 1, 2, 3 } },
                //        { (int)Suit.Hearts, new List<int>(4){ 0, 1, 2, 3 } },
                //        { (int)Suit.Spades, new List<int>(4){ 0, 1, 2, 3 } }
                //    };
                //    suitHasPlayer[(int)Suit.Clubs].Remove(id);
                //    suitHasPlayer[(int)Suit.Diamonds].Remove(id);
                //    suitHasPlayer[(int)Suit.Hearts].Remove(id);
                //    suitHasPlayer[(int)Suit.Spades].Remove(id);
                //}
            }

            if (cardSuit == Trump)
            {
                remainingTrumps--;
            }
            if (playerID == id)
            {
                hand.Remove(card);
            }
            else
            {
                deck.RemoveCard(card);
                if (cardValue > 0)
                {
                    othersPointCards[cardSuit].Remove(Card.GetRank(card));
                }
            }
        }

        public void RemovePlay(int playerId, int card)
        {
            Trick currentTrick = tricks[tricks.Count - 1];
            if (currentTrick.IsFull())
            {
                int[] winnerPoints = currentTrick.GetTrickWinnerAndPoints();
                if (winnerPoints[0] == id || winnerPoints[0] == (id + 2) % 4)
                {
                    MyTeamPoints -= winnerPoints[1];
                }
                else
                {
                    //TODO checks valence of points!!!
                    OtherTeamPoints -= winnerPoints[1];
                }
            }
            currentTrick.UndoMove();
            if (currentTrick.IsEmpty())
            {
                tricks.RemoveAt(tricks.Count - 1);
            }

            if (playerId == id)
            {
                hand.Add(card);
            }
            else
            {
                deck.Add(card);
                if (Card.GetValue(card) > 0)
                {
                    othersPointCards[Card.GetSuit(card)].Add(Card.GetRank(card));
                }
                if (Card.GetSuit(card) == Trump)
                {
                    remainingTrumps++;
                }
            }
        }

        public int predictTrickPoints()
        {
            Trick currentTrick = tricks[tricks.Count - 1];
            return currentTrick. GetTrickWinnerAndPoints()[1];
        }

        private bool checkPlayersHaveAllSuits(Dictionary<int,List<int>> suitHasPlayer)
        {
            if (suitHasPlayer[0].Count == 3 &&
                suitHasPlayer[1].Count == 3 &&
                suitHasPlayer[2].Count == 3 &&
                suitHasPlayer[3].Count == 3)
            {
                return true;
            }
            return false;
        }

        public List<List<int>> Sample()
        {
            List<List<int>> hands = new List<List<int>>();
            int myHandSize = hand.Count;
            int[] handSizes = new int[3] { myHandSize, myHandSize, myHandSize };
            int currentTrickSize = tricks[tricks.Count - 1].GetCurrentTrickSize();
            if (currentTrickSize > 3)
            {
                currentTrickSize = 0;
            }
            for (int i = 0; i < currentTrickSize; i++)
            {
                handSizes[2 - i]--;
            }

            hands.Add(new List<int>(hand));
            List<List<int>> sampledHands;

            if (checkPlayersHaveAllSuits(suitHasPlayer))
            {
                sampledHands = deck.SampleHands(handSizes);
            }
            else
            {
                sampledHands = deck.SampleHands(suitHasPlayer, handSizes);
                if (sampledHands == null)
                {
                    suitHasPlayer = new Dictionary<int, List<int>>
                    {
                        { (int)Suit.Clubs, new List<int>(3){ 1, 2, 3 } },
                        { (int)Suit.Diamonds, new List<int>(3){ 1, 2, 3 } },
                        { (int)Suit.Hearts, new List<int>(3){ 1, 2, 3 } },
                        { (int)Suit.Spades, new List<int>(3){ 1, 2, 3 } }
                    };
                    sampledHands = deck.SampleHands(handSizes);
                }
            }

            for (int i = 0; i < 3; i++)
            {
                hands.Add(sampledHands[i]);
            }

            //SuecaGame.PrintCards("p0", hands[0]);
            //SuecaGame.PrintCards("p1", hands[1]);
            //SuecaGame.PrintCards("p2", hands[2]);
            //SuecaGame.PrintCards("p3", hands[3]);
            return hands;
        }

        private List<int> getHighestPerSuit(List<int> cards)
        {
            cards.Sort();
            List<int> list = new List<int>();

            int firstCard = cards[0];
            int lastCard = cards[cards.Count - 1];
            if (Card.GetSuit(firstCard) == Card.GetSuit(lastCard))
            {
                list.Add(lastCard);
                return list;
            }

            int lastSuit = Card.GetSuit(firstCard);
            for (int i = 0; i < cards.Count; i++)
			{
                int card = cards[i];
                int cardSuit = Card.GetSuit(card);

                if (cardSuit != lastSuit)
                {
                    lastSuit = cardSuit;
                    list.Add(cards[i - 1]);
                }
                if (i == cards.Count - 1)
                {
                    list.Add(card);
                }
            }
            return list;
        }

        private List<int> counterPerSuit(List<int> cards)
        {
            List<int> list = new List<int>();
            int lastSuit = (int) Suit.None;
            for (int i = 0; i < cards.Count; i++)
            {
                int cardSuit = Card.GetSuit(cards[i]);
                if (cardSuit != lastSuit)
                {
                    lastSuit = cardSuit;
                    list.Add(1);
                }
                else
                {
                    list[list.Count - 1]++;
                }
            }
            return list;
        }

        public int RuleBasedDecision()
        {
            List<int> possibleMoves = GetPossibleMoves();
            if (possibleMoves.Count == 1)
            {
                return possibleMoves[0];
            }

            List<int> highestPerSuit = getHighestPerSuit(possibleMoves);

            if (highestPerSuit.Count == 1)
            {
                possibleMoves.Sort();
                int highestCard = possibleMoves[possibleMoves.Count - 1];
                if (shouldPlay(highestCard))
                {
                    return highestCard;
                }
                else
                {
                    return possibleMoves[0];
                }
            }
            else
            {
                int trickSize = tricks[tricks.Count - 1].GetCurrentTrickSize();
                if (trickSize == 4 || trickSize == 0)
                {
                    List<int> counterList = counterPerSuit(possibleMoves);

                    //debug
                    if(counterList.Count != highestPerSuit.Count)  
                    {
                        Console.WriteLine("PROBLEM!!! RuleBasedDecision");
                    }

                    for (int i = 0; i < highestPerSuit.Count; i++)
                    {
                        int highestCard = highestPerSuit[i];
                        if (shouldPlay(highestCard))
                        {
                            if (counterList[i] > 5 && Card.GetSuit(highestCard) == Trump)
                            {
                                return highestCard;
                            }
                            else if (counterList[i] > 5 && Card.GetSuit(highestCard) != Trump)
                            {
                                break;
                            }
                            else
                            {
                                return highestCard;
                            }
                        }
                    }
                    return possibleMoves[0];
                }
                else
                {
                    //we may check if our mate has cut the trick and chose an highest card
                    possibleMoves.Sort(new DescendingComparer());
                    return possibleMoves[0];
                }
            }
        }

        private bool shouldPlay(int highestCard)
        {
            int highestCardSuit = Card.GetSuit(highestCard);
            int highestCardRank = Card.GetRank(highestCard);

            int othersHighestRankFromSuit;
            if (othersPointCards[highestCardSuit].Count > 0)
            {
                othersHighestRankFromSuit = othersPointCards[highestCardSuit][0];
            }
            else
            {
                othersHighestRankFromSuit = 0;
            }

            int winninCard = tricks[tricks.Count - 1].GetCurrentWinningCard();
            int highestRankOnTable = Card.GetRank(winninCard);

            if (highestRankOnTable >= 0 && highestCardRank > highestRankOnTable && highestCardRank > othersHighestRankFromSuit)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}