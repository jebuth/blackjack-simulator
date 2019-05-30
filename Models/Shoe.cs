using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack_Simulator
{
    class Shoe
    {
        private IList<Card> RemainingShoe { get; set; }

        // Creates an n-deck shoe
        public Shoe(int n)
        {
            RemainingShoe = new List<Card>();

            for(int i = 0; i < n; i++)
            {
                SingleDeck deck = new SingleDeck();
                foreach(Card card in deck.GetSingleDeck())
                {
                    RemainingShoe.Add(card);
                }
                
            }

            this.Shuffle();
        }

        public void Shuffle()
        {
            IList<Card> ShuffledShoe = new List<Card>();

            Random rand = new Random();
            int randomIndex = 0;
            while(RemainingShoe.Count > 0)
            {
                randomIndex = rand.Next(0, RemainingShoe.Count);
                ShuffledShoe.Add(RemainingShoe.ElementAt(randomIndex));
                RemainingShoe.RemoveAt(randomIndex);
            }

            // copy the shuffledShoe back to Remainingshoe
            foreach(Card card in ShuffledShoe)
            {
                RemainingShoe.Add(card);
            }

        }

        public Card Draw()
        {
            if (RemainingShoe.Count() > 0)
            {
                Card victim = RemainingShoe.ElementAt(0);
                RemainingShoe.Remove(victim);
                return victim;
            }
            else
                throw new Exception("Empty shoe");
        }

        public IList<Card> RemainingCards()
        {
            return RemainingShoe;
        }
    }
}
