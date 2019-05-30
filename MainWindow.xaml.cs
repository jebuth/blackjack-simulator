using System;
using System.Windows;
using System.Collections.ObjectModel;
using Blackjack_Simulator.Models;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Blackjack_Simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private Shoe shoe = null;
        private Dealer dealer = null;
        private Player player = null;
        private Round round = null;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //private ObservableCollection<String> playerValues = null;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            DataContext = this;
            shoe = new Shoe(1);
            
            dealer = new Dealer();
            player = new Player();

            this.DealerSide.ItemsSource = dealer.GetHand();
            this.PlayerSide.ItemsSource = player.GetHand();

            ConsoleAllocator.ShowConsoleWindow();

        }


        #region Game Logic

        private void Deal()
        {
            DealButton.IsEnabled = false;
            //dealer.Draw(new Card(1, CardSuit.Hearts));
            dealer.Draw(shoe.Draw());
            player.Draw(shoe.Draw());

            //player.Draw(new Card(6, CardSuit.Hearts));
            //dealer.Draw(new Card(13, CardSuit.Hearts));
            dealer.Draw(shoe.Draw());
            player.Draw(shoe.Draw());
            // player.Draw(new Card(6, CardSuit.Clubs));


            // tests: force dealer cards
            //dealer.hand.Clear();
            //dealer.Draw(new Card(1, CardSuit.Hearts));
            //dealer.Draw(new Card(5, CardSuit.Hearts));
            // ==========================================

            //player.hand.Clear();
            //player.Draw(new Card(10, CardSuit.Hearts));
            //player.Draw(new Card(1, CardSuit.Hearts));

            // pass initial hand 
            round = new Round(dealer, player);

            round.CheckInitialBlackjack();

            // test these canges
            DealerPrompt.Text = dealer.handValue().ToString();
            PlayerPrompt.Text = player.handValue().ToString();

            EnableButtons();
            if (player.HasPair())
            {
                SplitButton.IsEnabled = true;
            }

            if (!round.InProgress())
            {
                EndRound();
            }
        }
 
        private void PlayerHit()
        {
            try
            {
                //player.Draw(shoe.Draw());
                player.Draw(new Card(6, CardSuit.Spades));

                //PlayerPrompt.Text = player.handValue().ToString();

                // player did not split
                if (!player.split)
                {
                    PlayerPrompt.Text = player.handValue().ToString();

                    // check bust
                    if (player.busted)
                    {
                        round.SetResult(RoundResult.PlayerBust);
                        EndRound();
                    }
                }
                // player did split
                else
                {
                    //var currentHand = player.hands[player.focusedHandIndex];
                    var currentHand = player.currentHand();
                    
                    // check which hand to focus
                    PlayerPrompt.Text = currentHand.handValueInt().ToString();
                    

                    if (currentHand.busted)
                    {
                       // round.SetResult(RoundResult.PlayerBust);
                        
                        // if current hand is th elast hand, end round
                        if(player.isLastHand())
                        {
                            Console.WriteLine("Player bust on last hand.");
                            // may need to change the condition up here
                            //EndRound();
                            EndPlayerTurn();
                        }
                        else
                        {
                            Console.WriteLine(string.Format("Player bust on hand {0}.", player.focusedHandIndex+1));
                            //round.SetResult(RoundResult.PlayerBust); uncesseary extra
                            player.endHand();

                            //current hand busted, hit for the next hand
                            PlayerHit();
                        }
                    }
                }

                if (player.HasPair())
                {
                    this.SplitButton.IsEnabled = true;
                }
                else
                {
                    this.SplitButton.IsEnabled = false;
                }


            }
            catch (Exception ex)
            {
                var stop = ex.Message;
            }
        }

        private void Stand()
        {
            // end turn if player didn't split
            if (!player.split)
            {
                EndPlayerTurn();
            }
            // if they did split, check when hand they are standing on
            else
            {
                // if focus in on the last hand, EndTurn()
                if (player.focusedHandIndex != player.hands.Count - 1)
                {
                    //player.focusedHandIndex++;
                    player.endHand();
                    PlayerHit();
                }
                else
                {
                    EndPlayerTurn();
                }
            }
        }

        private void Split()
        {
            // create two hands for player
            player.Split();
            PlayerHit();

            if (player.HasPair())
            {
                this.SplitButton.IsEnabled = true;
            }
            else
            {
                this.SplitButton.IsEnabled = false;
            }
        }

   

        private void EndPlayerTurn()
        {

            // if player busted all hands, round is over. reveal dealer hand
            if (player.BustedAllHands())
            {
                dealer.RevealCard();
                round.CompareHands();
                EndRound();
            }
            else
            {
                // if playe busted some hands, start deal turn but when comparing hands, set playe bust for busted hands.
                // only compare valid player hands
                
                //reveal dealer card
                dealer.RevealCard();

                // dealer stand on 17
                // test this
                while (dealer.underSeventeen())
                {
                    dealer.Draw(shoe.Draw());
                    //dealer.Draw(new Card(1, CardSuit.Spades)); // ok
                    //dealer.Draw(new Card(2, CardSuit.Spades)); // ok
                    //dealer.Draw(new Card(3, CardSuit.Spades)); // ok
                    //dealer.Draw(new Card(4, CardSuit.Spades)); // ok
                    // dealer.Draw(new Card(5, CardSuit.Spades)); // ok after changing compareHands line 80 to 22 from 21
                    //dealer.Draw(new Card(6, CardSuit.Spades)); // ok
                    //dealer.Draw(new Card(7, CardSuit.Spades)); // ok
                    //dealer.Draw(new Card(8, CardSuit.Spades)); // ok
                    //dealer.Draw(new Card(9, CardSuit.Spades)); // ok
                    //dealer.Draw(new Card(10, CardSuit.Spades)); // ok
                    //dealer.Draw(new Card(11, CardSuit.Spades)); // ok
                    DealerPrompt.Text = dealer.handValue().ToString(); // can move this outside while loop
                }

                // dealer will either bust or it's time to compare hands
                if (dealer.busted)
                {
                    Console.WriteLine("EndPlayerTurn() dealer bust.");
                    DealerPrompt.Text = dealer.handValue().ToString();
                    //round.SetResult(RoundResult.DealerBust);
                    round.CompareHands();
                    EndRound();
                }
                else
                {
                    Console.WriteLine("EndPlayerTurn() dealer no bust.");
                    DealerPrompt.Text = dealer.handValue().ToString();
                    round.CompareHands();
                    EndRound();
                }
            }
        }


        private void EndRound()
        {
            string results = "";
            foreach(string result in round.GetWinner())
            {
                results += result + "\n";
            }

            GamePrompt.Text = results;
            DisableButtons();
        }

        #endregion

        #region cleanup

        private void DisableButtons()
        {
            this.HitButton.IsEnabled = false;
            this.Standbutton.IsEnabled = false;
            this.DoubleButton.IsEnabled = false;
            this.SplitButton.IsEnabled = false;

            this.DealButton.IsEnabled = false;
        }

        private void EnableButtons()
        {
            this.HitButton.IsEnabled = true;
            this.Standbutton.IsEnabled = true;
            this.DoubleButton.IsEnabled = true;
            this.DealButton.IsEnabled = true;
        }


        private void ClearTable()
        {
            GamePrompt.Text = "";
            DealerPrompt.Text = "";
            PlayerPrompt.Text = "";
            dealer.Clear();
            player.Clear();
            round = null;

            DisableButtons();
            this.DealButton.IsEnabled = true;
        }

        #endregion

        #region Button Events

        private void Deal_Button_Click(object sender, RoutedEventArgs e)
        {
            Deal();

        }
        private void Hit_Button_Click(object sender, RoutedEventArgs e)
        {
            PlayerHit();
        }

        private void Double_Button_Click(object sender, RoutedEventArgs e)
        {
            PlayerHit();
            EndPlayerTurn();
        }

        private void Stand_button_Click(object sender, RoutedEventArgs e)
        {
            Stand();

        }

        private void Split_Button_Click(object sender, RoutedEventArgs e)
        {
            Split();
        }

        private void Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            ClearTable();
        }

        #endregion

    }
}
