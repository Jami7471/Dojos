using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2015
{
    public class Task02 : BaseTask
    {
        /*
         --- Day 2: I Was Told There Would Be No Math ---
        The elves are running low on wrapping paper, and so they need to submit an order for more. 
        They have a list of the dimensions (length l, width w, and height h) of each present, and only want to order exactly as much as they need.

        Fortunately, every present is a box (a perfect right rectangular prism (https://en.wikipedia.org/wiki/Cuboid#Rectangular_cuboid)), which makes calculating the required wrapping paper for each gift a little easier: 
        find the surface area of the box, which is 2*l*w + 2*w*h + 2*h*l. The elves also need a little extra paper for each present: the area of the smallest side.

        For example:
            - A present with dimensions 2x3x4 requires 2*6 + 2*12 + 2*8 = 52 square feet of wrapping paper plus 6 square feet of slack, for a total of 58 square feet.
            - A present with dimensions 1x1x10 requires 2*1 + 2*10 + 2*10 = 42 square feet of wrapping paper plus 1 square foot of slack, for a total of 43 square feet.
            
        All numbers in the elves' list are in feet. 
        
        Task: How many total square feet of wrapping paper should they order?
        Solution: 1586300       

        --- Part Two ---
        The elves are also running low on ribbon. Ribbon is all the same width, so they only have to worry about the length they need to order, which they would again like to be exact.

        The ribbon required to wrap a present is the shortest distance around its sides, or the smallest perimeter of any one face. 
        Each present also requires a bow made out of ribbon as well; the feet of ribbon required for the perfect bow is equal to the cubic feet of volume of the present. 
        Don't ask how they tie the bow, though; they'll never tell.

        For example:
            - A present with dimensions 2x3x4 requires 2+2+3+3 = 10 feet of ribbon to wrap the present plus 2*3*4 = 24 feet of ribbon for the bow, for a total of 34 feet
            - A present with dimensions 1x1x10 requires 1+1+1+1 = 4 feet of ribbon to wrap the present plus 1*1*10 = 10 feet of ribbon for the bow, for a total of 14 feet.

        Task: How many total feet of ribbon should they order?
        Solution:  3737498
        */

        public override string Part1()
        {
            List<string> input = ReadInputLines(@"Year2015\Input\Input02.txt");

            int totalSquareFeetOfWrappingPaper = 0;

            for (int i = 0; i < input.Count; i++)
            {
                GetDimensions(input[i], out int length, out int width, out int height);
                totalSquareFeetOfWrappingPaper += GetSquareFeetOfWrappingPaper(length, width, height);
            }

            return totalSquareFeetOfWrappingPaper.ToString();
        }

        public override string Part2()
        {
            List<string> input = ReadInputLines(@"Year2015\Input\Input02.txt");

            int totalFeetOfRibbon = 0;

            for (int i = 0; i < input.Count; i++)
            {
                GetDimensions(input[i], out int length, out int width, out int height);
                totalFeetOfRibbon += GetFeetOfRibbon(length, width, height);
            }

            return totalFeetOfRibbon.ToString();
        }

        private void GetDimensions(string dimensionsString, out int length, out int width, out int height)
        {
            string[] numbers = dimensionsString.Split('x');

            length = Convert.ToInt32(numbers[0]);
            width = Convert.ToInt32(numbers[1]);
            height = Convert.ToInt32(numbers[2]);
        }

        private int GetSquareFeetOfWrappingPaper(int length, int width, int height)
        {
            int areaSideLW = length * width;
            int areaSideWH = width * height;
            int areaSideHL = height * length;
            int smallestArea = (new int[] { areaSideHL, areaSideLW, areaSideWH }).OrderBy(area => area).ToArray()[0];

            return areaSideLW * 2 + areaSideWH * 2 + areaSideHL * 2 + smallestArea;
        }

        private int GetFeetOfRibbon(int length, int width, int height)
        {
            int[] sortedDimensions = (new int[] { length, width, height }).OrderBy(dim => dim).ToArray();
            return sortedDimensions[0] * 2 + sortedDimensions[1] * 2 + length * width * height;
        }
    }
}
