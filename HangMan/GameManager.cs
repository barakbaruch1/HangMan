using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography.Core;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace HangMan
{
    class GameManager
    {

        //init constructor
        private Canvas _gameCanvas;
        private ToggleSwitch _toggleSwitchLevels;
        private Button _buttonNewWord;
        private TextBlock _textBlockGuessWord;
        private TextBlock _textBlockCommentHard;

        //keyboard 
        private KeyBoard _keyBoard;
        private HangManFrames _pictureFrames;
        private GuessString _word;
        private RandomWord _randomWord;
        private bool _isHard = false;


        private int _maxWrongGuesses;

        //attribute const
        //max Wrong guesses need to divided integer by _prictureFrame.ImagesCount
        private const int MaxWrongGuessesEasy = 20;
        private const int MaxWrongGuessesHard = 20;
        private const int FontSizeTextBlocks = 40;
        public GameManager(Canvas gameCanvas,ToggleSwitch difficultyLevel,Button buttonNewWord, TextBlock textBlockGuessWord,TextBlock textBlockCommentHard)
        {

            #region init Xaml attribute
            _gameCanvas = gameCanvas;
           
            _toggleSwitchLevels = difficultyLevel;
            _toggleSwitchLevels.Toggled += toggleSwitchLevel_Toggled;


            _buttonNewWord = buttonNewWord;
            _buttonNewWord.Click += buttonNewWord_Click;


            _textBlockGuessWord = textBlockGuessWord;

            _textBlockCommentHard = textBlockCommentHard;
            _textBlockCommentHard.Foreground =  new SolidColorBrush(Colors.Red); 
            _textBlockCommentHard.FontSize = 20;
            #endregion

            _keyBoard = new KeyBoard(_gameCanvas, new System.Drawing.Rectangle(25, 25, 75, 75));
            _keyBoard.AddClickEventHandler(KeyBoard_Click);

            _pictureFrames = new HangManFrames(_gameCanvas, new System.Drawing.Rectangle(600, 400, 250, 250), @"/Assets/HangManImage/");

            _randomWord = new RandomWord();
            EasyGame();
            _textBlockGuessWord.FontSize = FontSizeTextBlocks;
        }

        private  void  KeyBoard_Click(object sender, RoutedEventArgs e)
        {
             WrongGuessUpdateFrame((KeyBoard)sender);
             UpdateTextBox();
             CheckEndGame();
        }
        private void buttonNewWord_Click(object sender, RoutedEventArgs e)
        {
            Restart();
        }
        private void toggleSwitchLevel_Toggled(object sender, RoutedEventArgs e)
        {
            if(_toggleSwitchLevels.IsOn)
            {
                _isHard = true;
                _maxWrongGuesses = MaxWrongGuessesHard;
                _textBlockCommentHard.Text = "Word start with uppercase";
            }
            else
            {
                _isHard = false;
                _maxWrongGuesses = MaxWrongGuessesEasy;
                _textBlockCommentHard.Text = "";
            }
            Restart();
        }             
        private void UpdateTextBox()
        {
            _textBlockGuessWord.Text = _word.ToString();
        }
        private void WrongGuessUpdateFrame(KeyBoard key)
        {
            char keyContent = (char)key.Content;
            bool ignoreUpperCaseSpecialKey = !(keyContent.Equals('>') || keyContent.Equals('<'));
            if (ignoreUpperCaseSpecialKey && !_word.GuessChar(keyContent))
            {
                UpdateHangManPicture();

            }
        }
        private async  void CheckEndGame()
        {
            if (_word.GameWin)
            {
                MessageDialog dialog = new MessageDialog("You Win the.The Word is:"+_word.ActualString);
                await dialog.ShowAsync();
                Restart();
            }
            else if (_word.GameLost)
            {
                MessageDialog dialog = new MessageDialog($"You Lost GameOver.\nYour Result:{_word.HiddenString}.\nThe actual word is:{_word.ActualString}");
                await dialog.ShowAsync();
                Restart();
            }
            
        }
        private void Restart()
        {
            _pictureFrames.RestartFrames();
            _keyBoard.RestartKeyBoard();
            if (_isHard)
                HardGame();
            else
                EasyGame();
        }
        private void HardGame()
        {
            _maxWrongGuesses = MaxWrongGuessesHard;
            _word = new GuessString(_randomWord.GetRandomHardWord(), _maxWrongGuesses);
            _textBlockGuessWord.Text = _word.ToString();
        }
        private void EasyGame()
        {
            _maxWrongGuesses = MaxWrongGuessesEasy;
            _word = new GuessString(_randomWord.GetRandomEasyWord(), _maxWrongGuesses);
            _textBlockGuessWord.Text = _word.ToString();
            _keyBoard.SpecialKeyDisableToUpper();
        }
        private void UpdateHangManPicture()
        {
            //if max Wrong guesses greater then images change algorithem
            if (_maxWrongGuesses > _pictureFrames.ImagesCount)
            {
                // if _maxWrongGuesse greater then frame 
                //example: 10 guesses 5 frames each 2 wrong guesses get the next fame
                // the divided must to be integer without data loss
                int framePerGuesses = _maxWrongGuesses / (_pictureFrames.ImagesCount - 1);
                if (_word.CountWrongGuess % framePerGuesses == 0)
                    _pictureFrames.NextPicture();
            }
            else
            {
                // if _maxWrongGuesse smaller then frame skip frames
                //example frames 10 guesses 5 each wrong guess skip 2 frames
                // the divided must to be integer without data loss
                int framePerGuesses = (int)Math.Floor( (_pictureFrames.ImagesCount - 1) / (decimal) _maxWrongGuesses);
                    for(int i = 0; i < framePerGuesses;i++)
                        _pictureFrames.NextPicture();
            }
        }
    }
}
 