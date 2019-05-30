using Blackjack_Simulator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack_Simulator
{
    class Round
    {
        public Dealer dealer { get; set; }
        public Player player { get; set; }

        public IList<string> Winner;

        private bool InProg { get; set; }

        public Round(Dealer dealer, Player player)
        {
            this.dealer = dealer;
            this.player = player;
            this.InProg = true;

            Winner = new List<string>();
        }

        // initially checks if dealer has face up face
        public void CheckInitialBlackjack()
        {
            bool dealerHasBlackjack = false;

            // check player blackjack first and foremost
            if (player.handValueInt() == 21 && player.containsAce)
            {
                SetResult(RoundResult.PlayerBlackjack);
                dealer.RevealCard();
                return;
            }


            // if dealer has face face up
            if (dealer.hand.ElementAt(1).Value > 9)
            {
                // check if facedown is ACE
                if (dealer.faceDownCard.Value == 1)
                {
                    //SetResult(RoundResult.Dealerblackjack);
                    dealerHasBlackjack = true;
                    dealer.RevealCard();
                }
            }
            // ace face up, check insurance?
            else if (dealer.hand.ElementAt(1).Value == 1)
            {
                if(dealer.faceDownCard.Value == 10)
                {
                    //SetResult(RoundResult.Dealerblackjack);
                    dealerHasBlackjack = true;
                    dealer.RevealCard();
                }
            }

            if(dealerHasBlackjack && player.handValueInt() != 21)
            {
                SetResult(RoundResult.Dealerblackjack);
            }
            else if(dealerHasBlackjack && player.handValueInt() == 21)
            {
                SetResult(RoundResult.Push);
            }
        }

        // at this point, dealer is above 17 or x|17
        public void CompareHands()
        {

            int dealerValue = dealer.handValueInt();
            //int playerValue = player.handValueInt();

            int handCounter = 1;
            foreach(Hand playerHand in player.hands)
            {
                // if player busted a hand, no need to compare to dealer's
                if (playerHand.busted)
                {
                    Console.WriteLine("Player busted hand {0}", handCounter);
                    SetResult(RoundResult.PlayerBust);
                }
                else
                {
                    if (dealerValue < 22)
                    {
                        if (dealerValue > playerHand.handValueInt())
                        {
                            Console.WriteLine("CompareHands() dealer win.");
                            SetResult(RoundResult.DealerWins);
                        }
                        else if (dealerValue < playerHand.handValueInt())
                        {
                            Console.WriteLine("CompareHands() player win.");
                            SetResult(RoundResult.PlayerWins);
                        }
                        else
                        {
                            Console.WriteLine("CompareHands() push.");
                            SetResult(RoundResult.Push);
                        }
                    }
                    else
                    {
                        Console.WriteLine("CompareHands() Dealer bust.");
                        SetResult(RoundResult.DealerBust);
                    }
                }

                handCounter++;
            }
        }

        public void SetResult(RoundResult result)
        {

            // i think all of this is for splits/ multiple hands

            //var currentResult = Winner.ElementAt(handIndex);
            Console.WriteLine("SetResult(result)");
            switch (result)
            {
                case RoundResult.Dealerblackjack:
                    this.Winner.Add("Dealer blackjack");
                    break;
                case RoundResult.DealerWins:
                    this.Winner.Add("Dealer wins");
                    break;
                case RoundResult.PlayerBust:
                    //Console.WriteLine("set result dealer win 108");
                    this.Winner.Add("Player bust");
                    break;
                case RoundResult.DealerBust:
                    this.Winner.Add("Dealer bust");
                    break;
                case RoundResult.PlayerBlackjack:
                    this.Winner.Add("Player blackjack");
                    break;
                case RoundResult.PlayerWins:
                    //Console.WriteLine("set result player win 114");
                    this.Winner.Add("Player wins");
                    break;
                case RoundResult.Push:
                    this.Winner.Add("Push!");
                    break;
            }

            InProg = false;
        }

        public IList<string> GetWinner()
        {
            return Winner;
        }

        public bool InProgress()
        {
            return InProg;
        }

    }
}

// bug when player should bust but doesn't