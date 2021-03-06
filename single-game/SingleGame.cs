using System;
using System.Collections.Generic;

namespace SuecaSolver
{
    public class MainSuecaSolver
    {

        public static void Main()
        {
            string input;
            string[] playersNames = new string[4];
            playersNames[0] = "Bot";
            int firstPlayerID;
            Console.WriteLine("");
            Console.WriteLine("|||||||||||||||||||| SUECA GAME ||||||||||||||||||||");
            Console.WriteLine("");

            Console.WriteLine("Player 0: Bot");
            Console.Write("Player 1: ");
            playersNames[1] = Console.ReadLine();
            Console.Write("Player 2: ");
            playersNames[2] = Console.ReadLine();
            Console.Write("Player 3: ");
            playersNames[3] = Console.ReadLine();
            Console.Write("First player ID: ");
            input = Console.ReadLine();
            firstPlayerID = Convert.ToInt16(input);
            Console.WriteLine("");

            int seed = Guid.NewGuid().GetHashCode();
            Random randomNumber = new Random(seed);
            Deck deck = new Deck();
            List<List<int>> playersHand = new List<List<int>>(
                new List<int>[] {
                    new List<int>(10),
                    new List<int>(10),
                    new List<int>(10),
                    new List<int>(10) });
            deck.SampleHands(ref playersHand);
            List<int> currentHand;
            int trumpCardPlayer = (firstPlayerID - 1 + 4) % 4;
            int trumpCard = playersHand[trumpCardPlayer][0];
            int cardIndex, currentPlayerID = firstPlayerID;

            SmartPlayer artificialPlayer = new SmartPlayer(0, playersHand[0], trumpCard, trumpCardPlayer);
            SuecaGame game = new SuecaGame(Card.GetSuit(trumpCard), firstPlayerID);
            //Console.WriteLine("---------hands---------");
            //sueca.printhand(playershand[0]);
            //sueca.printhand(playershand[1]);
            //sueca.printhand(playershand[2]);
            //sueca.printhand(playershand[3]);
            //console.writeline("-----------------------");

            for (int i = 0; i < 40; i++)
            {
                currentHand = playersHand[currentPlayerID];
                Console.WriteLine("||||||||||||||||||||||||||||||||||||||||||||||||||||");
                Console.WriteLine("                 Trick " + (i / 4));
                Console.WriteLine("                 Player " + currentPlayerID + " - " + playersNames[currentPlayerID]);
                Console.WriteLine("                 Trump is " + (Suit) Card.GetSuit(trumpCard));
                Console.WriteLine("");

                game.PrintLastTrick();
                game.PrintCurrentTrick();
                Sueca.PrintCurrentHand(currentHand);
                int chosenCard;

                if (currentPlayerID != 0)
                {
                    Console.Write("Pick the card you want to play by its index: ");
                    input = Console.ReadLine();
                    cardIndex = Convert.ToInt16(input);
                    chosenCard = currentHand[cardIndex];
                    artificialPlayer.AddPlay(currentPlayerID, chosenCard);
                }
                else
                {
                    chosenCard = artificialPlayer.Play();
                }

                game.PlayCard(currentPlayerID, chosenCard);
                currentHand.Remove(chosenCard);
                currentPlayerID = game.GetNextPlayerId();
            }

            Console.WriteLine("|||||||||||||||||||||||| END |||||||||||||||||||||||");
            Console.WriteLine("");
            int[] points = game.GetGamePoints();
            Console.WriteLine("Team " + playersNames[0] + " and " + playersNames[2] + " - " + points[0] + " points");
            Console.WriteLine("Team " + playersNames[1] + " and " + playersNames[3] + " - " + points[1] + " points");
            // game.PrintPoints(playersNames);
            Console.WriteLine("");
            Console.ReadLine();

        }
    }
}