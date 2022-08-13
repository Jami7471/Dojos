using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2015
{
    public class Task05 : BaseTask
    {
        /*
        --- Day 5: Doesn't He Have Intern-Elves For This? ---
        Santa needs help figuring out which strings in his text file are naughty or nice.

        A nice string is one with all of the following properties:
            - It contains at least three vowels (aeiou only), like aei, xazegov, or aeiouaeiouaeiou.
            - It contains at least one letter that appears twice in a row, like xx, abcdde (dd), or aabbccdd (aa, bb, cc, or dd).
            - It does not contain the strings ab, cd, pq, or xy, even if they are part of one of the other requirements.

        For example:
            - ugknbfddgicrmopn is nice because it has at least three vowels (u...i...o...), a double letter (...dd...), and none of the disallowed substrings.
            - aaa is nice because it has at least three vowels and a double letter, even though the letters used by different rules overlap.
            - jchzalrnumimnmhp is naughty because it has no double letter.
            - haegwjzuvuyypxyu is naughty because it contains the string xy.
            - dvszwmarrgswjxmb is naughty because it contains only one vowel.

        Task: How many strings are nice?
        Solution: 255

        --- Part Two ---
        Realizing the error of his ways, Santa has switched to a better model of determining whether a string is naughty or nice. 
        None of the old rules apply, as they are all clearly ridiculous.

        Now, a nice string is one with all of the following properties:
            - It contains a pair of any two letters that appears at least twice in the string without overlapping, like xyxy (xy) or aabcdefgaa (aa), but not like aaa (aa, but it overlaps).
            - It contains at least one letter which repeats with exactly one letter between them, like xyx, abcdefeghi (efe), or even aaa.

        For example:
            - qjhvhtzxzqqjkmpb is nice because is has a pair that appears twice (qj) and a letter that repeats with exactly one letter between them (zxz).
            - xxyxx is nice because it has a pair that appears twice and a letter that repeats with one between, even though the letters used by each rule overlap.
            - uurcxstgmygtbstg is naughty because it has a pair (tg) but no repeat with a single letter between them.
            - ieodomkazucvgmuy is naughty because it has a repeating letter with one between (odo), but no pair that appears twice.

        Task: How many strings are nice under these new rules?
        Solution: 55
        */

        public override string Part1()
        {
            List<string> input = ReadInputLines(@"Year2015\Input\Input05.txt");

            int countOfNiceStrings = 0;

            string[] forbiddenStrings = new string[] { "ab", "cd", "pq", "xy" };

            foreach (string stringValue in input)
            {
                if (HasLeastNumberOfVowels(stringValue, 3)                          // It contains at least three vowels(aeiou only), like aei, xazegov, or aeiouaeiouaeiou.
                    && HasLeastNumberOfLetterAppearsTwiceInARow(stringValue, 1)     // It contains at least one letter that appears twice in a row, like xx, abcdde(dd), or aabbccdd(aa, bb, cc, or dd).
                    && DoesNotContainASpecificString(stringValue, forbiddenStrings) // It does not contain the strings ab, cd, pq, or xy, even if they are part of one of the other requirements.
                    )
                {
                    countOfNiceStrings++;
                }
            }

            return countOfNiceStrings.ToString();
        }

        public override string Part2()
        {
            List<string> input = ReadInputLines(@"Year2015\Input\Input05.txt");

            int countOfNiceStrings = 0;

            foreach (string stringValue in input)
            {
                if (HasLeastTwicePairOfTwoLetters(stringValue)                          // It contains a pair of any two letters that appears at least twice in the string without overlapping, like xyxy(xy) or aabcdefgaa(aa), but not like aaa(aa, but it overlaps).
                    && HasLeastOneLetterWithExactlyOneLetterBetweenThem(stringValue)    // It contains at least one letter which repeats with exactly one letter between them, like xyx, abcdefeghi (efe), or even aaa.
                    )
                {
                    countOfNiceStrings++;
                }
            }

            return countOfNiceStrings.ToString();
        }

        private bool HasLeastNumberOfVowels(string stringValue, int leastNumberOfVowels)
        {
            char[] vowels = new char[] { 'a', 'e', 'i', 'o', 'u' };
            int numberOfVowels = 0;

            foreach (char s in stringValue.ToLower())
            {
                if (vowels.Contains(s))
                {
                    numberOfVowels++;
                }

                if (numberOfVowels >= leastNumberOfVowels)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasLeastNumberOfLetterAppearsTwiceInARow(string stringValue, int leastLetterAppearsTwiceInARow)
        {
            int numberOfLetterAppearsTwiceInARow = 0;
            char? previouslyChar = null;

            foreach (char s in stringValue.ToLower())
            {
                if (previouslyChar.HasValue == false)
                {
                    previouslyChar = s;
                    continue;
                }
                else
                {
                    if (previouslyChar == s)
                    {
                        numberOfLetterAppearsTwiceInARow++;

                        if (numberOfLetterAppearsTwiceInARow >= leastLetterAppearsTwiceInARow)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        previouslyChar = s;
                    }
                }
            }

            return false;
        }

        private bool DoesNotContainASpecificString(string stringValue, string[] forbiddenStrings)
        {
            foreach (string forbiddenString in forbiddenStrings)
            {
                if (stringValue.Contains(forbiddenString))
                {
                    return false;
                }
            }

            return true;
        }

        private bool HasLeastTwicePairOfTwoLetters(string stringValue)
        {
            for (int i = 0; i < stringValue.Length - 3; i++)
            {
                string comparePair = stringValue.Substring(i, 2);
                string remainingString = stringValue[(i + 2)..];

                if (remainingString.Contains(comparePair))
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasLeastOneLetterWithExactlyOneLetterBetweenThem(string stringValue)
        {
            for (int i = 0; i < stringValue.Length - 2; i++)
            {
                if (stringValue[i] == stringValue[i + 2])
                {
                    return true;
                }
            }

            return false;
        }
    }
}
