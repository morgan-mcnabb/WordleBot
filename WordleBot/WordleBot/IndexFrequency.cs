using System.Collections.Generic;
using System.Linq;

namespace WordleBot
{
    public class IndexFrequency
    {
        public char Character { get; set; }
        public Dictionary<int, int> IndexFreq { get; set; }

        public int MostFrequentIndex { get; set; }

        public int HighestFrequency { get; set; }

        public IndexFrequency(char character)
        {
            Character = character;
            IndexFreq = new Dictionary<int, int>()
                {
                    { 0, 0 },
                    { 1, 0 },
                    { 2, 0 },
                    { 3, 0 },
                    { 4, 0 },
                };
        }

        public void SetHighestFrequencyAndMostFrequentIndex()
        {
            HighestFrequency = IndexFreq.Values.Max();
            MostFrequentIndex = IndexFreq.Where(p => p.Value == HighestFrequency).Select(p => p.Key).FirstOrDefault();
        }
        public static List<IndexFrequency> BuildLetterFrequencies(string[] words)
        {
            char[] allCharacters = new char[] {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
                                                            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};
            List<IndexFrequency> charactersAtIndex = new();
            foreach (var character in allCharacters)
                charactersAtIndex.Add(new IndexFrequency(character));

            foreach (var word in words)
            {
                for (int i = 0; i < word.Length; i++)
                {
                    charactersAtIndex.Where(w => w.Character == word[i]).ToList().ForEach(s => s.IndexFreq[i]++);
                }
            }

            foreach (var indexfrequency in charactersAtIndex)
            {
                indexfrequency.SetHighestFrequencyAndMostFrequentIndex();
            }

            return charactersAtIndex;
        }
    }
}
