using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2015
{
    public sealed class Task10 : BaseTask
    {
        /*
        --- Day 10: Elves Look, Elves Say ---
        Today, the Elves are playing a game called look-and-say. 
        They take turns making sequences by reading aloud the previous sequence and using that reading as the next sequence. 
        For example, 211 is read as "one two, two ones", which becomes 1221 (1 2, 2 1s).

        Look-and-say sequences are generated iteratively, using the previous value as input for the next step. 
        For each step, take the previous value, and replace each run of digits (like 111) with the number of digits (3) followed by the digit itself (1).

        For example:
            - 1 becomes 11 (1 copy of digit 1).
            - 11 becomes 21 (2 copies of digit 1).
            - 21 becomes 1211 (one 2 followed by one 1).
            - 1211 becomes 111221 (one 1, one 2, and two 1s).
            - 111221 becomes 312211 (three 1s, two 2s, and one 1).

        Task: Starting with the digits in your puzzle input, apply this process 40 times. What is the length of the result?
        Solution: 492982

        --- Part Two ---
        Neat, right? You might also enjoy hearing John Conway talking about this sequence (that's Conway of Conway's Game of Life fame).

        Now, starting again with the digits in your puzzle input, apply this process 50 times. 
        
        Task: What is the length of the new result?
        Solution: 6989950

        */

        public override string Part1()
        {
            string input = ReadInputToEnd(@"Year2015\Input\Input10.txt");
            return PlayLookAndSayInMultipleRounds(input, 40).Length.ToString();
        }

        public override string Part2()
        {
            string input = ReadInputToEnd(@"Year2015\Input\Input10.txt");
            return PlayLookAndSayInMultipleRounds(input, 50).Length.ToString();
        }

        private string PlayLookAndSayInMultipleRounds(string startWord, int rounds)
        {
            string playWord = startWord;

            for (int i = 0; i < rounds; i++)
            {
                playWord = PlayOneRoundLookAndSay(playWord);
            }

            return playWord;
        }

        private string PlayOneRoundLookAndSay(string startWord)
        {
            StringBuilder result = new StringBuilder();
            char? previouslyChar = null;
            int countOfpreviouslyChar = 0;

            foreach (char c in startWord)
            {
                if (previouslyChar.HasValue == false)
                {
                    previouslyChar = c;
                    countOfpreviouslyChar++;
                    continue;
                }
                else
                {
                    if (previouslyChar == c)
                    {
                        countOfpreviouslyChar++;
                    }
                    else
                    {
                        result.Append($"{countOfpreviouslyChar}{previouslyChar}");

                        previouslyChar = c;
                        countOfpreviouslyChar = 1;
                    }
                }
            }

            if (previouslyChar.HasValue == true)
            {
                result.Append($"{countOfpreviouslyChar}{previouslyChar}");
            }

            return result.ToString();
        }
    }
}
