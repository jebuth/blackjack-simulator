using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack_Simulator
{
    // Represents a stndard 52 card deck
    class SingleDeck
    {
        private IList<Card> Deck { get; set; }

        public SingleDeck()
        {
            Deck = new List<Card>();
            // initialize cards
            // 11 = j
            // 12 = q
            // 13 = k
            for(int i = 1; i < 14; i++)
            {
                Deck.Add(new Card(i, CardSuit.Clubs));
                Deck.Add(new Card(i, CardSuit.Diamonds));
                Deck.Add(new Card(i, CardSuit.Hearts));
                Deck.Add(new Card(i, CardSuit.Spades));
            }
        }

        public IList<Card> GetSingleDeck()
        {
            return this.Deck;
        }


        public int RemainingCardCount()
        {
            return Deck.Count;
        }
    }
}
