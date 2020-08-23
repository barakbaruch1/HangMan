using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HangMan
{
    class  RandomWord
    {
        private const int WordLessCharsThen = 7;
        private const int WordMoreCharsThen = 4;
       // public bool IsHard { get; private set; }
        private List<string> _words;
        private Random random;
        public RandomWord()
        {
            _words = new List<string>();
            AddFromFileWordsToList();
            random = new Random();

        }
        public string GetRandomHardWord()
        {
            int randomIndexFromHardsList = random.Next(0, _words.Count);
            return UpperCaseFirstCharInString(_words[randomIndexFromHardsList]);
        }
        public string GetRandomEasyWord()
        {
            int randomIndexFromEasyList = random.Next(0, _words.Count);
            return _words[randomIndexFromEasyList];
        }
        private void AddFromFileWordsToList()
        {

            string currentDirectory = Directory.GetCurrentDirectory();
            string wordsPath = currentDirectory + @"\Assets\Words.txt";
            using (StreamReader sr = File.OpenText(wordsPath))
            {
                string word;
                while ((word = sr.ReadLine()) != null)
                {
                    if (WordMoreCharsThen <= word.Length  && word.Length <= WordLessCharsThen )
                    {
                         word = word.ToLower();
                        _words.Add(word);
                    }
                }
            }
        }
        private string UpperCaseFirstCharInString(string str)
        {
            if (str == null)  return ""; 
            
            string result;
            if (str.Length > 1)
            {
                string upperCaseWord = str.Substring(0,1).ToUpper();
                result = upperCaseWord + str.Substring(1);
            }
            else
            {
                result = str.ToUpper(); 
            }
            return result;
        }
    }
}
