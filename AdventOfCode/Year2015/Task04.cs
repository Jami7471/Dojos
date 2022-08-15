using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2015
{
    public sealed class Task04 : BaseTask
    {
        /*
        --- Day 4: The Ideal Stocking Stuffer ---
        Santa needs help mining some AdventCoins (very similar to bitcoins) to use as gifts for all the economically forward-thinking little girls and boys.

        To do this, he needs to find MD5 hashes which, in hexadecimal, start with at least five zeroes. 
        The input to the MD5 hash is some secret key (your puzzle input, given below) followed by a number in decimal. 
        
        Task: To mine AdventCoins, you must find Santa the lowest positive number (no leading zeroes: 1, 2, 3, ...) that produces such a hash.
        Solution: 117946

        For example:
            - If your secret key is abcdef, the answer is 609043, because the MD5 hash of abcdef609043 starts with five zeroes (000001dbbfa...), and it is the lowest such number to do so.
            - If your secret key is pqrstuv, the lowest number it combines with to make an MD5 hash starting with five zeroes is 1048970; that is, the MD5 hash of pqrstuv1048970 looks like 000006136ef....
         
        
        --- Part Two ---
        Task: Now find one that starts with six zeroes.
        Solution: 3938038 

        */

        public override string Part1()
        {
            string input = ReadInputToEnd(@"Year2015\Input\Input04.txt");
            return GetLowestHashNumberByNZeroes(input, 5).ToString();
        }

        public override string Part2()
        {
            string input = ReadInputToEnd(@"Year2015\Input\Input04.txt");
            return GetLowestHashNumberByNZeroes(input, 6).ToString();
        }

        private string CreateMD5(string input)
        {
            using System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes);
        }

        private int GetLowestHashNumberByNZeroes(string myPuzzleInput, int countOfZeroes)
        {
            bool stopLoop = false;
            int loopCount = 0;
            string zeroCompareValue = CreateZeroCompareValue(countOfZeroes);

            do
            {
                string md5Hash = CreateMD5($"{myPuzzleInput}{loopCount}");

                if (md5Hash.StartsWith(zeroCompareValue))
                {
                    stopLoop = true;
                }
                else
                {
                    loopCount++;
                }
            }
            while (stopLoop == false);

            return loopCount;
        }

        private string CreateZeroCompareValue(int countOfZeroes)
        {
            string zeroCompareValue = string.Empty;

            for (int i = 0; i < countOfZeroes; i++)
            {
                zeroCompareValue += "0";
            }

            return zeroCompareValue;
        }
    }
}
