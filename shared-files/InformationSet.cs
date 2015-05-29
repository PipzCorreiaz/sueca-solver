using System;
using System.Collections;
using System.Collections.Generic;

namespace SuecaSolver
{
    public class InformationSet
    {
        private List<int> hand;
        private List<Move> currentTrick;
        public int Trump;
        private Dictionary<int,int> dictionary;
        private Deck deck;
        private Dictionary<int,List<int>> suitHasPlayer;

        //        private object deckLock;


        public InformationSet(List<int> currentHand, int trumpSuit)//, object lockMyDeck)
        {
            Trump = trumpSuit;
            hand = new List<int>(currentHand);
            dictionary = new Dictionary<int,int>();
            suitHasPlayer = new Dictionary<int,List<int>>
            {
                { (int)Suit.Clubs, new List<int>(3){ 1, 2, 3 } },
                { (int)Suit.Diamonds, new List<int>(3){ 1, 2, 3 } },
                { (int)Suit.Hearts, new List<int>(3){ 1, 2, 3 } },
                { (int)Suit.Spades, new List<int>(3){ 1, 2, 3 } }
            };
            currentTrick = new List<Move>();
            deck = new Deck(currentHand);
//            deckLock = lockMyDeck;
        }

        public int GetHandSize()
        {
            return hand.Count;
        }


        public List<int> GetPossibleMoves()
        {
            return SuecaGame.PossibleMoves(hand, GetLeadSuit());
        }

        public int GetLeadSuit()
        {
            if (currentTrick.Count == 0)
            {
                return (int)Suit.None;
            }

            return Card.GetSuit(currentTrick[0].Card);
        }

        public List<Move> GetCardsOnTable()
        {
            return currentTrick;
        }

        public int GetHighestCardIndex()
        {
            int bestCard = -1;
            int bestValue = Int32.MinValue;

            foreach (KeyValuePair<int, int> cardValue in dictionary)
            {
                if (cardValue.Value > bestValue)
                {
                    bestValue = cardValue.Value;
                    bestCard = cardValue.Key;
                }
            }

            if (bestCard == -1)
            {
                Console.WriteLine("Trouble at InformationSet.GetHighestCardIndex()");
            }

            return bestCard;
        }

        public void AddPlay(int playerID, int card)
        {
            int leadSuit = GetLeadSuit();
            if (Card.GetSuit(card) != leadSuit && leadSuit != (int)Suit.None)
            {
                suitHasPlayer[leadSuit].Remove(playerID);
            }

            if (currentTrick.Count == 3)
            {
                currentTrick.Clear();
            }
            else
            {
                currentTrick.Add(new Move(playerID, card));
            }
            deck.RemoveCard(card);
        }

        public void AddMyPlay(int card)
        {
            if (currentTrick.Count == 3)
            {
                currentTrick.Clear();
            }
            else
            {
                currentTrick.Add(new Move(0, card));
            }
            hand.Remove(card);
        }

        public void CleanCardValues()
        {
            dictionary.Clear();
        }

        public void AddCardValue(int card, int val)
        {
            if (dictionary.ContainsKey(card))
            {
                dictionary[card] += val;
            }
            else
            {
                dictionary[card] = val;
            }
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
//            Console.WriteLine("[" + System.Threading.Thread.CurrentThread.ManagedThreadId + "] - hand.Count " + hand.Count + " currentTrick.Count " + currentTrick.Count);
//            Console.Out.Flush();
            int[] handSizes = new int[3] { myHandSize, myHandSize, myHandSize };
            int currentTrickSize = currentTrick.Count;

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
//                lock (deckLock)
//                {
                sampledHands = deck.SampleHands(suitHasPlayer, handSizes);
//                }
            }

            for (int i = 0; i < 3; i++)
            {
                hands.Add(sampledHands[i]);
            }

            return hands;
        }


        //        public List<List<int>> SampleThree(int n)
        //        {
        //            List<List<int>> hands = new List<List<int>>();
        //            hands.Add(deck.GetHand(n));
        //            hands.Add(deck.GetHand(n));
        //            hands.Add(deck.GetHand(n));
        //            return hands;
        //        }


        //        public List<List<int>> SampleAll(int n)
        //        {
        //            return deck.SampleAll(n);
        //        }


        private void printDictionary(string name)
        {
            string str = name + " -";
            foreach (KeyValuePair<int, int> cardValue in dictionary)
            {
                str += " <" + Card.ToString(cardValue.Key) + "," + cardValue.Value + ">";
            }
            Console.WriteLine(str);
        }


        public void PrintInfoSet()
        {
            Console.WriteLine("------------------INFOSET------------------");
            SuecaGame.PrintCards("Hand", hand);
            Console.WriteLine("Trump - " + Trump);
            printDictionary("Dictionary");
            Console.WriteLine("-------------------------------------------");
        }
    }
}