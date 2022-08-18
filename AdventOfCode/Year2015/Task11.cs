using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2015
{
    public sealed class Task11 : BaseTask
    {
        /*
        
        --- Day 11: Corporate Policy ---
        Santa's previous password expired, and he needs help choosing a new one.

        To help him remember his new password after the old one expires, Santa has devised a method of coming up with a password based on the previous one. 
        Corporate policy dictates that passwords must be exactly eight lowercase letters (for security reasons), so he finds his new password by incrementing his old password string repeatedly until it is valid.

        Incrementing is just like counting with numbers: xx, xy, xz, ya, yb, and so on. 
        Increase the rightmost letter one step; if it was z, it wraps around to a, and repeat with the next letter to the left until one doesn't wrap around.

        Unfortunately for Santa, a new Security-Elf recently started, and he has imposed some additional password requirements:
            - Passwords must include one increasing straight of at least three letters, like abc, bcd, cde, and so on, up to xyz. They cannot skip letters; abd doesn't count.
            - Passwords may not contain the letters i, o, or l, as these letters can be mistaken for other characters and are therefore confusing.
            - Passwords must contain at least two different, non-overlapping pairs of letters, like aa, bb, or zz.

        For example:
            - hijklmmn meets the first requirement (because it contains the straight hij) but fails the second requirement requirement (because it contains i and l).
            - abbceffg meets the third requirement (because it repeats bb and ff) but fails the first requirement.
            - abbcegjk fails the third requirement, because it only has one double letter (bb).
            - The next password after abcdefgh is abcdffaa.
            - The next password after ghijklmn is ghjaabcc, because you eventually skip all the passwords that start with ghi..., since i is not allowed.

        Task: Given Santa's current password (your puzzle input), what should his next password be?
        Solution: hepxxyzz

        --- Part Two ---
        Santa's password expired again. 
        
        Task: What's the next one?
        Solution: 

        */

        public override string Part1()
        {
            string input = ReadInputToEnd(@"Year2015\Input\Input11.txt");
            char[] forbiddenChars = new char[] { 'i', 'o', 'l' };

            return GetNewPassword(input, forbiddenChars);
        }

        public override string Part2()
        {
            string input = ReadInputToEnd(@"Year2015\Input\Input11.txt");
            char[] forbiddenChars = new char[] { 'i', 'o', 'l' };

            return GetNewPassword(GetNewPassword(input, forbiddenChars), forbiddenChars);
        }

        private string GetNewPassword(string oldPassword, char[] forbiddenChars)
        {
            bool found = false;
            char? newChar = null;
            string newPassword = oldPassword;

            while (found == false)
            {
                if (newChar == null)
                {
                    newChar = GetNewChar(oldPassword[^1], forbiddenChars);
                }
                else
                {
                    newChar = GetNewChar((char)newChar, forbiddenChars);
                }

                newPassword = newPassword[0..^1] + newChar;

                if(newChar == 'a')
                {
                    newPassword = SpinCharInWord(newPassword, oldPassword.Length - 2, oldPassword, forbiddenChars);

                    if (string.IsNullOrWhiteSpace(newPassword))
                    {
                        return string.Empty;
                    }
                }

                if (HasOneIncreasingStraightOfAtLeastThreeLetters(newPassword, forbiddenChars)
                       && HasAtLeastTwoDifferentNonOverlappingPairsOfLetters(newPassword))
                {
                    found = true;
                }
            }

            return newPassword;
        }

        private string SpinCharInWord(string word, int index, string compareWord, char[] forbiddenChars)
        {
            char newChar = GetNewChar(word[index], forbiddenChars);

            if(newChar == 'a')
            {
                if (index == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return SpinCharInWord(word[..index] + newChar + word[(index + 1)..], index - 1, compareWord, forbiddenChars);
                }
            }
            else
            {
                if (index == 0)
                {
                    return newChar + word[1..];
                }
                else
                {
                    return word[..index] + newChar + word[(index + 1)..];
                }
            }  
        }           

        private char GetNewChar(char oldChar, char[] forbiddenChars)
        {
            if (oldChar == 'z')
            {
                return 'a';
            }

            char newChar = (char)((int)oldChar + 1);

            if (forbiddenChars.Contains(newChar))
            {
                newChar = (char)((int)newChar + 1);
            }

            return newChar;
        }

        private bool HasOneIncreasingStraightOfAtLeastThreeLetters(string word, char[] forbiddenChars)
        {
            int[] forbiddenCharValues = new int[forbiddenChars.Length];

            for (int i = 0; i < forbiddenChars.Length; i++)
            {
                forbiddenCharValues[i] = (int)forbiddenChars[i];
            }

            for (int i = 0; i < word.Length - 2; i++)
            {
                int c1 = (int)word[i];
                int c2 = (int)word[i + 1];
                int c3 = (int)word[i + 2];

                if (c2 - c1 == 1 && c3 - c1 == 2
                    || forbiddenCharValues.Contains(c2) && c2 - c1 == 2 && c3 - c1 == 3
                    || forbiddenCharValues.Contains(c3) && c2 - c1 == 1 && c3 - c1 == 3)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasAtLeastTwoDifferentNonOverlappingPairsOfLetters(string word)
        {
            char? pairChar = null;

            for (int i = 0; i < word.Length - 1; i++)
            {
                if (word[i] == word[i + 1])
                {
                    if (pairChar == null)
                    {
                        pairChar = word[i + 1];
                        i++;
                    }
                    else
                    {
                        if (word[i] != pairChar)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        IEnumerable<string> Passwords(string pwd) =>
            from word in Words(pwd)
            let straigth = Enumerable.Range(0, word.Length - 2).Any(i => word[i] == word[i + 1] - 1 && word[i] == word[i + 2] - 2)
            let reserved = "iol".Any(ch => word.Contains(ch))
            let pairs = Enumerable.Range(0, word.Length - 1).Select(i => word.Substring(i, 2)).Where(sword => sword[0] == sword[1]).Distinct()
            where straigth && !reserved && pairs.Count() > 1
            select word;

        IEnumerable<string> Words(string word)
        {
            while (true)
            {
                var sb = new StringBuilder();
                for (var i = word.Length - 1; i >= 0; i--)
                {
                    var ch = word[i] + 1;
                    if (ch > 'z')
                    {
                        ch = 'a';
                        sb.Insert(0, (char)ch);
                    }
                    else
                    {
                        sb.Insert(0, (char)ch);
                        sb.Insert(0, word.Substring(0, i));
                        i = 0;
                    }
                }
                word = sb.ToString();
                yield return word;
            }
        }
    }
}
