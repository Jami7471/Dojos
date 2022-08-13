using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2015
{
    public class Task01 : BaseTask
    {
        /*         
        --- Day 1: Not Quite Lisp ---
        Santa was hoping for a white Christmas, but his weather machine's "snow" function is powered by stars, and he's fresh out! To save Christmas, he needs you to collect fifty stars by December 25th.

        Collect stars by helping Santa solve puzzles. Two puzzles will be made available on each day in the Advent calendar; the second puzzle is unlocked when you complete the first. Each puzzle grants one star. Good luck!

        Here's an easy puzzle to warm you up.

        Santa is trying to deliver presents in a large apartment building, but he can't find the right floor - the directions he got are a little confusing. He starts on the ground floor (floor 0) and then follows the instructions one character at a time.

        An opening parenthesis, (, means he should go up one floor, and a closing parenthesis, ), means he should go down one floor.

        The apartment building is very tall, and the basement is very deep; he will never find the top or bottom floors.

        For example:

        (()) and ()() both result in floor 0.
        ((( and (()(()( both result in floor 3.
        ))((((( also results in floor 3.
        ()) and ))( both result in floor -1 (the first basement level).
        ))) and )())()) both result in floor -3.
        
        Task: To what floor do the instructions take Santa?
        Solution: 74

        --- Part Two ---
        Now, given the same instructions, find the position of the first character that causes him to enter the basement (floor -1). The first character in the instructions has position 1, the second character has position 2, and so on.

        For example:

        ) causes him to enter the basement at character position 1.
        ()()) causes him to enter the basement at character position 5.
        
        Task: What is the position of the character that causes Santa to first enter the basement?
        Solution: 1795
         */


        public override string Part1()
        {
            string input = ReadInputToEnd(@"Year2015\Input\Input01.txt");

            int floor = 0;

            foreach (char step in input)
            {
                if (step == '(')
                {
                    floor++;
                }
                else
                {
                    floor--;
                }
            }

            return floor.ToString();
        }

        public override string Part2()
        {
            string input = ReadInputToEnd(@"Year2015\Input\Input01.txt");

            int floor = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '(')
                {
                    floor++;
                }
                else
                {
                    floor--;
                }

                if (floor == -1)
                {
                    return $"{i + 1}";
                }
            }

            return "No Result";
        }
    }
}
