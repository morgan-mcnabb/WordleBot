using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WordleBot
{
    struct CorrectCharactersAtIndex
    {
        public char character;
        public int index;
    }
    struct GuessResult
    {
        public bool foundAnswer;
        public int guesses;
    }

    class WordleBot
    {
#if DEBUG
        public string _wordToGuess { get; set; }
#endif
        const int MAX_GUESSES = 6;

        List<WordData> _wordData = new();
        List<CorrectCharactersAtIndex> _correctCharactersAtIndex = new();
        List<char> _incorrectCharacters = new();
        List<char> _correctCharacters = new();
        string[] _testOptimalGuesses = new string[] { "roate", "files" };
        List<string> _guessedWords = new();

        public WordleBot(List<IndexFrequency> characterFrequencies)
        {
            BuildWordData(characterFrequencies);
        }

#if DEBUG
        public WordleBot(string wordToGuess, List<IndexFrequency> characterFrequencies)
        {
            _wordToGuess = wordToGuess;
            BuildWordData(characterFrequencies);
        }
#endif
        private void BuildWordData(List<IndexFrequency> characterFrequencies)
        {
            var data = File.ReadAllLines("words2.txt").ToArray();
            WordData wordData;
            foreach(var d in data)
            {
                string[] parsedData = d.Split(":");
                wordData = new WordData(parsedData[0]);
                wordData.FrequencyScore = int.Parse(parsedData[1]);
                wordData.IndexScore = int.Parse(parsedData[2]);
                wordData.Weight = double.Parse(parsedData[3]);
                _wordData.Add(wordData);
            }
        }
        public string GetGuess(int currentGuesses)
        {
            Random random = new(Guid.NewGuid().GetHashCode());
            if (currentGuesses < 3)
                return _testOptimalGuesses[currentGuesses - 1];

            var potentialGuesses = new List<WordData>();

            var filteredWords = _wordData.Where(s => s.Word.IndexOfAny(_incorrectCharacters.ToArray()) == -1 && new string(_correctCharacters.ToArray()).All(s.Word.Contains));
            bool addFlag;
            foreach (var wordData in filteredWords)
            {
                addFlag = true;
                foreach (CorrectCharactersAtIndex correctEntry in _correctCharactersAtIndex)
                {
                    if (wordData.Word[correctEntry.index] != correctEntry.character)
                    {
                        addFlag = false;
                        break;
                    }
                }
                if (addFlag && !_guessedWords.Contains(wordData.Word.ToLower()))
                    potentialGuesses.Add(wordData);
            }

            return potentialGuesses.OrderByDescending(x => x.Weight).ThenByDescending(x => x.IndexScore).First().Word.ToLower();
            //return potentialGuesses.OrderByDescending(x => x.IndexScore).First().Word.ToLower();
        }

        public GuessResult FindWordByGuessing()
        {
            int currentGuesses = 0;
            bool foundAnswer = false;
            while (!foundAnswer && currentGuesses < MAX_GUESSES)
            {
                currentGuesses++;
                foundAnswer = ValidateGuess(GetGuess(currentGuesses));
            }
            return new GuessResult { foundAnswer = foundAnswer, guesses = currentGuesses };
        }

        public bool ValidateGuess(string guess)
        {
            _guessedWords.Add(guess);
            if (guess == _wordToGuess)
                return true;

            for (int i = 0; i < guess.Length; i++)
            {
                if (_wordToGuess.Contains(guess[i]))
                {
                    if (!_correctCharacters.Contains(guess[i]))
                        _correctCharacters.Add(guess[i]);

                    if (guess[i] == _wordToGuess[i])
                        if (!_correctCharactersAtIndex.Contains(new CorrectCharactersAtIndex { character = guess[i], index = i }))
                            _correctCharactersAtIndex.Add(new CorrectCharactersAtIndex { character = guess[i], index = i });
                }
                else
                    _incorrectCharacters.Add(guess[i]);
            }

            return false;
        }

        public void Reset(string word)
        {
            _correctCharactersAtIndex.Clear();
            _correctCharacters.Clear();
            _incorrectCharacters.Clear();
            _guessedWords.Clear();
            _wordToGuess = word;
        }
    }
}
