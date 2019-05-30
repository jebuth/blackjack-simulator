using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack_Simulator.Models
{
    class Hand
    {
        public List<Card> hand { get; set; }


        public Hand()
        {
            hand = new List<Card>();
        }

        public Hand(Card card)
        {
            hand = new List<Card>();
            hand.Add(card);
        }

        public Hand(IList<Card> card)
        {
            hand = new List<Card>();
            hand = card.ToList();
        }

        public void Add(Card card)
        {
            hand.Add(card);
        }

        
        
        public int handValueInt()
        {
            // if no ace, return value
            if (!this.containsAce)
            {
                return Int32.Parse(this.handValue());
            }
            // ace
            else
            {
                string softValue = this.handValue();
                int slashIndex = softValue.IndexOf('/');
                int minValue = Int32.Parse(softValue.Substring(0, slashIndex));
                int maxValue = Int32.Parse(softValue.Substring(slashIndex + 1, softValue.Length - slashIndex - 1));
                return maxValue > 21 ? minValue : maxValue;
            }
        }

        public string handValue()
        {
            List<int> handValue = new List<int>();

            handValue.Add(this.hand.Sum(card => card.Value));

            // ace in hand
            if (this.containsAce)
            {
                handValue.Add(handValue[0] + 10);
            }

            if (handValue.Count == 1)
            {
                return handValue[0].ToString();
            }
            else
            {
                return handValue[0].ToString() + "/" + handValue[1].ToString();
            };
        }

        public int split { get; set; }

        public bool containsAce
        {
            get
            {
                var ace = this.hand.Where(card => card.Value == 1).ToList();
                return ace.Count > 0 ? true : false;
            }

            set {; }
        }


        private bool _busted;
        public bool busted
        {
            get
            {
                return this.handValueInt() > 21 ? true : false;
            }
            set
            {
                _busted = value;
            }
        }

        public bool HasPair()
        {
            int valueOne = hand[0].Value;
            int valueTwo = hand[1].Value;

            CardFace faceOne = hand[0].face;
            CardFace faceTwo = hand[1].face;

            bool sameValues = valueOne == valueTwo ? true : false;
            bool sameFaces = faceOne == faceTwo ? true : false;

            return sameValues && sameFaces && hand.Count == 2;
        }
    }
}


// Hand will be a list of Cards
// in case of split there are multiple hands
// each hand will be treated independently of one another