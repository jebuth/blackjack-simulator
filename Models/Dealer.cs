using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack_Simulator.Models
{
    class Dealer 
    {
        public ObservableCollection<Card> hand { get; set; }
        public Card faceDownCard { get; set; }

        public Dealer()
        {
            hand = new ObservableCollection<Card>();
        }

        public ObservableCollection<Card> GetHand()
        {
            return this.hand;
        }

        public void Draw(Card card)
        {
            // deal first card face down
            if(this.hand.Count == 0)
            {
                hand.Add(new Card(-1, CardSuit.FaceDown));
                this.faceDownCard = card;
            }
            else
            {
                hand.Add(card);
            }
        }

        public void RevealCard()
        {
            this.hand[0] = faceDownCard;
        }

        public void Clear()
        {
            hand.Clear();
        }

        public string handValue()
        {
            List<int> handValue = new List<int>();

            // take face down into consideration
            if (this.hand[0].Value == -1)
            {
                handValue.Add(this.hand.Sum(card => card.Value) + 1);
            }
            else
            {
                handValue.Add(this.hand.Sum(card => card.Value));
            }

            // ace in hand
            if (this.containsAce)
            {
                handValue.Add(handValue[0] + 10);
            }

            if(handValue.Count == 1)
            {
                return handValue[0].ToString();
            }
            else
            {
                return handValue[0].ToString() + "/" + handValue[1].ToString();
            };
            
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

        // test this method
        public bool underSeventeen()
        {
            return this.handValueInt() < 17 ? true : false; 
        }

        public bool containsAce
        {
            get
            {
                var ace = this.hand.Where(card => card.Value == 1).ToList();
                return ace.Count > 0 ? true : false;
            }

            set {; }
        }

        // test this method
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
    }
}
