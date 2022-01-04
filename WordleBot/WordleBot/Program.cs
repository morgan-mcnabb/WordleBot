using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WordleBot
{
    class Program
    {
        static void Main(string[] args)
        {
            // all this for some preliminary testing i tell ya what
            Random r = new Random(Guid.NewGuid().GetHashCode());
            var words = File.ReadAllLines("words.txt").ToArray();
            string word = words[r.Next(words.Length)];

            List<IndexFrequency> characterData = IndexFrequency.BuildLetterFrequencies(words);
            WordleBot bot = new(word, characterData);

            int runs = 0;
            int max = 5000;
            int successes = 0;
            int totalGuesses = 0;
            while (runs < max)
            {
                var result = bot.FindWordByGuessing();
                if (result.foundAnswer)
                    successes++;
                totalGuesses += result.guesses;
                bot.Reset(words[r.Next(words.Length)]);
                runs++;
                Console.Write("\rProcessed run: {0}", runs);
            }
            Console.WriteLine();

            Console.WriteLine("Successes: {0} Fails: {1} Percentage Success: {2}", successes, max - successes, ((double)successes / (double)max));
            Console.WriteLine("Total Number of Guesses: {0} Average Number of Guesses per Attempt: {1}", totalGuesses, (double)totalGuesses / (double)(max));
        }
    }
}
