using System.Collections.Generic;
using System.Linq;

namespace WordleBot
{
    public class WordData
    {
        public string Word { get; set;}

        // lower the better
        public int FrequencyScore { get; set; } = 0;

        // lower the better
        public int IndexScore { get; set; } = 0;

        public double Weight { get; set;} = 0;

        public WordData(string word)
        {
            Word = word;
        }

        public void SetFrequencyAndIndexScores(List<IndexFrequency> characterData)
        {
            // for calculating frequency score
            var highestFreq = characterData.OrderByDescending(x => x.HighestFrequency).ToList();
            
            // for calculating index score
            Dictionary<int, char> mostCommonLettersByIndex = new();
            for (int i = 0; i < Word.Length; i++)
            {
                var highestFreqByIndex = characterData.Where(x => x.MostFrequentIndex == i).OrderByDescending(x => x.HighestFrequency).First();
                mostCommonLettersByIndex[i] = highestFreqByIndex.Character;
            }

            for(int i = 0; i < Word.Length; i++)
            { 
                FrequencyScore += highestFreq.FindIndex(x => char.ToLower(x.Character) == char.ToLower(Word[i]));

                if(char.ToLower(mostCommonLettersByIndex[i]) == char.ToLower(Word[i]))
                    IndexScore++;
            }
        }
    }
}
