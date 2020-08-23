using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace HangMan
{
    class HangManFrames
    { 
        private static readonly string  _PreFix = $"ms-appx://";

        private static readonly string _imgEndingPrefix = "png";

        private static readonly string _nameOfFrame = "HangMan"; 

        private  List<string> _imagesUrls;

        private int frameNumber = 0;

        private const int StartFrame = 0;
        public int ImagesCount { get { return _imagesUrls.Count; } }

        private Canvas _gameCanvas;

        private Image _img;
        public HangManFrames(Canvas gameCanvas,Rectangle rect)
        {
            _img = new Image();
            _imagesUrls = new List<string>();
            _img.Width = rect.Width;
            _img.Height = rect.Height;
            Canvas.SetLeft(_img, rect.Left);
            Canvas.SetTop(_img, rect.Top);
            _gameCanvas = gameCanvas;
            _gameCanvas.Children.Add(_img);

        }

        public HangManFrames(Canvas gameCanvas, Rectangle rect,string folderPicturesShortUrl) : this(gameCanvas,rect)
        {
            AddFolderOfPicture(folderPicturesShortUrl);
        }

        public void AddFolderOfPicture(string folderPicturesShortUrl)
        {
            string path = Directory.GetCurrentDirectory();
            string fullPath = path + ChangeSlash(folderPicturesShortUrl);
            int fileFolderCount = Directory.GetFiles(fullPath, "*", SearchOption.TopDirectoryOnly).Length;
            string tmpDirectoryImage = "";
            for (int i = 0; i < fileFolderCount; i++)
            {

                 tmpDirectoryImage = _PreFix + folderPicturesShortUrl + $"{_nameOfFrame}{i}.{_imgEndingPrefix}";

                _imagesUrls.Add(tmpDirectoryImage);
            }
            _img.Source = new BitmapImage(new Uri($"{_imagesUrls[0]}"));
            
        }
        public void NextPicture()
        {

            frameNumber++;
            if (frameNumber < _imagesUrls.Count)
            {
                _img.Source = new BitmapImage(new Uri($"{_imagesUrls[frameNumber]}"));


            }
            else
            {
                RestartFrames();
            }
        }
        public void RestartFrames()
        {
            frameNumber = StartFrame;
            _img.Source = new BitmapImage(new Uri($"{_imagesUrls[frameNumber]}"));
        }
        private string ChangeSlash(string str)
        {
            StringBuilder stb = new StringBuilder(str);
            for (int i = 0; i < str.Length; i++)
            {
                if (stb[i] == '/')
                {
                    stb[i] = '\\';
                }
            }
            return stb.ToString();
        }
    }
}
