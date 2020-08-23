using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Foundation;
using Windows.Networking.Sockets;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace HangMan
{
    class KeyBoard : Button 
    {
        private Canvas _gameCanvas;

        //Twp dimition array for /n the char ',' represent enter
        private static KeyBoard[,] _buttons;
        private const string keyboardKeysLowerCase = "abcdefghi,jklmnopqr,stuvwxyz>";
        private const string keyboardKeysUpperCase = "ABCDEFGHI,JKLMNOPQR,STUVWXYZ<";
        private const int spaceBetweenBtn = 10;
        private const int fontSize = 30;

        private List<char> _disableKeys;
        private int _caseChangeCount = 1;

        private readonly static string[] _spliteKeyBoardLowerCase;
        private readonly static string[] _spliteKeyBoardUpperCase;


        private Rectangle _rectChar;

        public int IndexI { get; set; }
        public int IndexJ { get; set; }
        
        static KeyBoard()
        {
            _spliteKeyBoardLowerCase = keyboardKeysLowerCase.Split(",");
            _spliteKeyBoardUpperCase = keyboardKeysUpperCase.Split(",");
            _buttons = new KeyBoard[_spliteKeyBoardLowerCase.Length, _spliteKeyBoardLowerCase[0].Length];
        }
        public KeyBoard()
        {
            _disableKeys = new List<char>();
        }
        public KeyBoard(Canvas gameCanvas,Rectangle rectChar) : this()
        {
            _gameCanvas = gameCanvas;
            _rectChar = rectChar;
            InitButtons();
        }
        private void InitButtons()
        {

            for(int i = 0; i < _buttons.GetLength(0) ;i++)
            {
                for (int j = 0; j < _buttons.GetLength(1); j++)
                {
                    _buttons[i, j] = new KeyBoard();
                    _buttons[i, j].IndexI = i;
                    _buttons[i, j].IndexJ = j;
                    _buttons[i, j].Width = _rectChar.Width;
                    _buttons[i, j].Height = _rectChar.Height;
                    _buttons[i, j].FontSize = fontSize;
                    _buttons[i, j].Click += KeyBoard_Click;
                   // _buttons[i, j].Background = new SolidColorBrush(Windows.UI.Color.FromArgb(25, 0, 100, 0));
                    Canvas.SetLeft(_buttons[i, j], _rectChar.X + (j * (_rectChar.Width + spaceBetweenBtn)));
                    Canvas.SetTop(_buttons[i, j], _rectChar.Y +  (i * (_rectChar.Height + spaceBetweenBtn)));
                    _buttons[i, j].Content = _spliteKeyBoardLowerCase[i][j];
                    _gameCanvas.Children.Add(_buttons[i, j]);
                }
            }
        }

        //add event click function to all buttons
        public void AddClickEventHandler(Windows.UI.Xaml.RoutedEventHandler myMethodName)
        {
            foreach(KeyBoard key in _buttons)
            {
                key.Click += myMethodName;
            }
        }
        
        // rest disable key and Keyboard
        public void RestartKeyBoard()
        {
            _disableKeys.Clear();
            AllKeysEnable();
        }
        private void KeyBoard_Click(object sender, RoutedEventArgs e)
        {
            KeyBoard key = (KeyBoard)sender;
            bool SpecialKey = !(key.IndexI == _buttons.GetLength(0) - 1 && key.IndexJ == _buttons.GetLength(1) - 1);
            if (SpecialKey)
            {
                _disableKeys.Add((char)key.Content);

            }
            else
                ChangeCase();

            UpdateKeyBoard();
        }
        private void ChangeCase()
        {
                  _caseChangeCount++;
                for (int i = 0; i < _buttons.GetLength(0); i++)
                {
                    for (int j = 0; j < _buttons.GetLength(1); j++)
                    {
                        if ( _caseChangeCount % 2 == 0)
                            _buttons[i, j].Content = _spliteKeyBoardUpperCase[i][j];
                        else
                            _buttons[i, j].Content = _spliteKeyBoardLowerCase[i][j];
                    }
                }
            UpdateKeyBoard();
        }
        public void SpecialKeyDisableToUpper()
        {
            //check if Upper Case
            if (IsUpperCase())
                ChangeCase();
            //disable Key
            _disableKeys.Add('>');
            UpdateKeyBoard();
        }
        private bool IsUpperCase()
        {
            return _caseChangeCount % 2 == 0;
        }
        
        //refresh KeyBoard disablekeys
        private void UpdateKeyBoard()
        {
            AllKeysEnable();
            DisableKeys();
        }
        
        // disable keys that added to list<char> _disablekeys
        private void DisableKeys()
        {
            foreach (var disablekey in _disableKeys)
            {
                foreach (var button in _buttons)
                {
                    if (button.Content.Equals(disablekey))
                    {
                        button.IsEnabled = false;
                    }
                }
            }
        }
        private void AllKeysEnable()
        {
            foreach (var button in _buttons)
            {
                button.IsEnabled = true;
            }
        }



    }
}
