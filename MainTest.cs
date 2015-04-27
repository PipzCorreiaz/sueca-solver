using System;
using System.Collections.Generic;

namespace SuecaSolver
{
	public class MainTest
	{

		public static void Main ()
		{
			Deck deck = new Deck();
			List<Card> hand = deck.GetHand(10);
			SuecaGame.PrintCards("P0", hand);
			InformationSet infoSet = new InformationSet(hand, Suit.Clubs);
			Console.WriteLine("P1 is going to play " + deck.deck[0]);
			infoSet.AddPlay(1, deck.deck[0]);
			Console.WriteLine("P2 is going to play " + deck.deck[1]);
			infoSet.AddPlay(2, deck.deck[1]);
			Console.WriteLine("P3 is going to play " + deck.deck[2]);
			infoSet.AddPlay(3, deck.deck[2]);
			Console.WriteLine("I am going to play " + hand[0]);
			infoSet.AddMyPlay(hand[0]);
			Console.WriteLine("P1 is going to play " + deck.deck[3]);
			infoSet.AddPlay(1, deck.deck[3]);
			Console.WriteLine("P2 is going to play " + deck.deck[4]);
			infoSet.AddPlay(2, deck.deck[4]);
			Console.WriteLine("P3 is going to play " + deck.deck[5]);
			infoSet.AddPlay(3, deck.deck[5]);

			List<List<Card>> list = infoSet.Sample();
			SuecaGame.PrintCards("P0", list[0]);
			SuecaGame.PrintCards("P1", list[1]);
			SuecaGame.PrintCards("P2", list[2]);
			SuecaGame.PrintCards("P3", list[3]);
			// PIMC pimc = new PIMC(1);
			// pimc.ExecuteTestVersion(infoSet, hand);
		}
	}
}