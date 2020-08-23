using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;

namespace HangMan
{
    public class GuessString
    {
        public string ActualString { get; }

        private StringBuilder _hiddenString;
        public string HiddenString { get { return _hiddenString.ToString(); } }

        private readonly int _maxWorngGuesses;
        public int CountGuess { get; private set; } = 0;
        public int CountSuccessesGuess { get; private set; } = 0;
        public int CountWrongGuess { get; private set; } = 0;
        public bool GameWin { get; private set; } = false;
        public bool GameLost { get; private set; } = false;
        private GuessString(string str)
        {
            ActualString = str;
            InitHiddenString();
        }
        public GuessString(string str,int maxWorngGuesses = 10) : this(str)
        {
            _maxWorngGuesses = maxWorngGuesses;
        }
        public bool GuessChar(char input)
        {
            bool successGuess = false;

            for (int i = 0; i < ActualString.Length;i++)
            {
                if(ActualString[i].Equals(input))
                {

                    successGuess = true;
                    _hiddenString[i] = input;
                }
            }

            if(successGuess)
                CountSuccessesGuess++;
            else
                CountWrongGuess++;

            EndGameResultCheck();
            CountGuess++;
            return successGuess;
        }
        public override string ToString()
        {
            return _hiddenString.ToString();
        }
        private void InitHiddenString()
        {
            _hiddenString = new StringBuilder();
            for(int i = 0; i < ActualString.Length; i++)
            {

                if(IsEnglishLetter(ActualString[i]))
                {
                    _hiddenString.Append("*");
                }
                else
                {
                    _hiddenString.Append(ActualString[i]);
                }
            }
        }
        private bool IsEnglishLetter(char letter)
        {
            int letterNum = (int)letter;
            int representAAscii = 65;
            int representZAscii = 90;
            int representaAscii = 97;
            int representzAscii = 122;

            if((representAAscii <= letterNum && letterNum  <= representZAscii) || 
                                    (representaAscii <= letterNum && letterNum <= representzAscii))
                return true;
            return false;
        }
        private bool CheckhiddenStringIsExpose()
        {
            foreach(var hiddenChars in _hiddenString.ToString())
            {
                if(hiddenChars.Equals('*'))
                {
                    return false;
                }
            }
            return true;
        }
        private void EndGameResultCheck()
        {
            if(CountWrongGuess < _maxWorngGuesses)
            {
                if(CheckhiddenStringIsExpose())
                {
                    GameWin = true;
                }
            }
            else
            {
                if(!GameWin)
                  GameLost = true;
            }
        }
    }
}
