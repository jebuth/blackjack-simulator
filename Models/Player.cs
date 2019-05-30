using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Blackjack_Simulator.Models
{
    class Player
    {
        public ObservableCollection<Card> masterHand { get; set; }

        public IList<Hand> hands { get; set; }

        //private bool _split = false;
        public bool split { get; set; }

        public int focusedHandIndex { get; set; }

        public int handCount { get; set; }


        public Player()
        {
            masterHand = new ObservableCollection<Card>();
            hands = new List<Hand>();
            hands.Add(new Hand());
            focusedHandIndex = 0; // default hand
            handCount = 1;
        }

        public bool BustedAllHands()
        {
            foreach(Hand hand in this.hands)
            {
                if (!hand.busted)
                {
                    return false;
                }
            }
            return true;
        }

        public void Split()
        {
            //determine where to place blank card
            int count = masterHand.Count;

            // insert blank card between the initial two to separate hands
            masterHand.Insert(count - 1, new Card(-1, CardSuit.Blank));
            split = true;
            handCount++;

            // add a second hand
            hands.Add(new Hand());

            // get duplicate
            var duplicateCard = hands[focusedHandIndex].hand.ElementAt(1);

            // remove 2nd card (duplicate) from hand1 
            hands[focusedHandIndex].hand.RemoveAt(1);

            ///hands.RemoveAt(0);
            // masterHand[1] is blank

            // ad card 2 of 2 to second had
            hands[focusedHandIndex+1].hand.Add(duplicateCard);

        }

        public bool HasPair()
        {
            return hands[focusedHandIndex].HasPair();
        }

        public bool isLastHand()
        {
            return (focusedHandIndex + 1) == hands.Count ? true : false;
        }

        public void endHand()
        {
            focusedHandIndex++;
        }


        public ObservableCollection<Card> GetHand()
        {
            return this.masterHand;
        }

        public void Draw(Card card)
        {
            if (!split)
            {
                masterHand.Add(card);
                hands[focusedHandIndex].Add(card);
            }
            else
            {

                // player chose to split.
                // determine which hand (1st or 2nd) to operate on
                if (focusedHandIndex == 0)
                {
                    // focus 1st hand
                    //get index of blank card
                    Card blank = masterHand.Where(x => x.Suit == CardSuit.Blank).LastOrDefault();

                    int blankIndex = masterHand.IndexOf(blank);
                    masterHand.Insert(blankIndex, card); // -1

                    hands[focusedHandIndex].Add(card);

                }
                // focus second hand
                else
                {

                    int indexOfNewCard = 0;
                    for(int i = 0; i < focusedHandIndex; i++)
                    {
                        indexOfNewCard += hands[i].hand.Count;
                    }

                    // figure this out?
                    indexOfNewCard += focusedHandIndex;

                    masterHand.Insert(indexOfNewCard + 1, card);
                    //masterHand.Add(card);

                    hands[focusedHandIndex].Add(card);
                }

            }
        }

        public void Clear()
        {
            masterHand.Clear();
            hands.Clear();
            hands.Add(new Hand());
            focusedHandIndex = 0;
            // todo: clear other index
            split = false;
        }

        public string handValue()
        {
            List<int> handValue = new List<int>();

            handValue.Add(this.masterHand.Sum(card => card.Value));

            // if a single hand
            if (handCount == 1)
            {
                //handValue.Add(this.masterHand.Sum(card => card.Value));

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
            // player has split x times
            else
            {
                return hands[focusedHandIndex].handValue();
            }
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

        public Hand currentHand()
        {
            return hands[focusedHandIndex];
        }

        public bool containsAce
        {
            get
            {
                var ace = this.masterHand.Where(card => card.Value == 1).ToList();
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

    }
}
