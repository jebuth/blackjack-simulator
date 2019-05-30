using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Blackjack_Simulator
{
    public enum CardSuit
    {
        Clubs, 
        Hearts,
        Spades,
        Diamonds,
        FaceDown,
        Blank
    }

    public enum CardFace
    {
        Ten,
        Jack,
        Queen,
        King,
        None
    }

    public enum RoundResult
    {
        PlayerBlackjack,
        PlayerWins,
        PlayerBust,
        Dealerblackjack,
        DealerWins,
        DealerBust,
        Push
    }

    public enum ButtonState
    {
        CanSplit
    }

    class Card 
    {
        public int Value { get; set; }
        public CardSuit Suit { get; set; }

        public CardFace face { get; set; }

        // 11 = j
        // 12 = q
        // 13 = k
        public Card(int value, CardSuit suit)
        {
            switch (value)
            {
                case 10:
                    face = CardFace.Ten;
                    break;
                case 11:
                    face = CardFace.Jack;
                    break;
                case 12:
                    face = CardFace.Queen;
                    break;
                case 13:
                    face = CardFace.King;
                    break;
                default:
                    face = CardFace.None;
                    break;
            }

            this.Value = value > 10 ? 10 : value;
            this.Suit = suit;

            this._ImageData = LoadImage(@"assets/" + value + suit + ".png");
        }


        
        private BitmapImage LoadImage(string filename)
        {
            return new BitmapImage(new Uri(filename, UriKind.Relative));
        }

        public string ImagePath()
        {
            string path = Value.ToString() + Suit;
            return "./assets/" + path + ".png";
        }


        private BitmapImage _ImageData;
        public BitmapImage ImageData
        {
            get { return this._ImageData; }
            set { this._ImageData = value; }
        }


        public bool Ace()
        {
            return this.Value == 1 ? true : false;
        }
    }
}
