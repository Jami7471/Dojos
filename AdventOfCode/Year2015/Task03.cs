using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2015
{
    public sealed class Task03 : BaseTask
    {
        /*         
        --- Day 3: Perfectly Spherical Houses in a Vacuum ---
        Santa is delivering presents to an infinite two-dimensional grid of houses.

        He begins by delivering a present to the house at his starting location, and then an elf at the North Pole calls him via radio and tells him where to move next. 
        Moves are always exactly one house to the north (^), south (v), east (>), or west (<). After each move, he delivers another present to the house at his new location.

        However, the elf back at the north pole has had a little too much eggnog, and so his directions are a little off, and Santa ends up visiting some houses more than once. 
        
        Task: How many houses receive at least one present?
        Solution: 2572

        For example:
            - > delivers presents to 2 houses: one at the starting location, and one to the east.
            - ^>v< delivers presents to 4 houses in a square, including twice to the house at his starting/ending location
            - ^v^v^v^v^v delivers a bunch of presents to some very lucky children at only 2 houses

        --- Part Two ---
        The next year, to speed up the process, Santa creates a robot version of himself, Robo-Santa, to deliver presents with him.

        Santa and Robo-Santa start at the same location (delivering two presents to the same starting house), then take turns moving based on instructions from the elf, 
        who is eggnoggedly reading from the same script as the previous year.

        Task: This year, how many houses receive at least one present?
        Solution: 2631

        For example:
            - ^v delivers presents to 3 houses, because Santa goes north, and then Robo-Santa goes south.
            - ^>v< now delivers presents to 3 houses, and Santa and Robo-Santa end up back where they started
            - ^v^v^v^v^v now delivers presents to 11 houses, with Santa going one direction and Robo-Santa going the other

         */

        public override string Part1()
        {
            string input = ReadInputToEnd(@"Year2015\Input\Input03.txt");

            int xPosition = 0;
            int yPosition = 0;
            int maxCountOfHousesInColumn = 0;

            List<List<int>> gridOfHouses = new List<List<int>>();

            // First element 
            gridOfHouses.Add(new List<int>());
            gridOfHouses[xPosition].Add(1);
            maxCountOfHousesInColumn++;

            foreach (char direction in input)
            {
                switch (direction)
                {
                    case '^':
                        // If y is on the last element, increase the maximum number by one and adjust all columns 
                        if (yPosition == maxCountOfHousesInColumn - 1)
                        {
                            maxCountOfHousesInColumn++;

                            for (int i = 0; i < gridOfHouses.Count; i++)
                            {
                                for (int j = gridOfHouses[i].Count; j <= maxCountOfHousesInColumn; j++)
                                {
                                    gridOfHouses[i].Add(0);
                                }
                            }
                        }

                        yPosition++;

                        break;
                    case '>':
                        // If x is at the outer right border, add a new column at the back
                        if (xPosition == gridOfHouses.Count - 1)
                        {
                            gridOfHouses.Add(new List<int>(maxCountOfHousesInColumn));

                            for (int i = 0; i < maxCountOfHousesInColumn; i++)
                            {
                                gridOfHouses[^1].Add(0);
                            }
                        }

                        xPosition++;

                        break;
                    case 'v':
                        // If y = 0, all columns must be extended by a 1st row because of the coordinate system
                        if (yPosition == 0)
                        {
                            for (int i = 0; i < gridOfHouses.Count; i++)
                            {
                                gridOfHouses[i].Insert(0, 0);
                            }

                            maxCountOfHousesInColumn++;
                        }
                        else
                        {
                            yPosition--;
                        }

                        break;
                    case '<':
                        // If x is at the outer left border, add a new column at the front
                        if (xPosition == 0)
                        {
                            gridOfHouses.Insert(0, new List<int>(maxCountOfHousesInColumn));

                            for (int i = 0; i < maxCountOfHousesInColumn; i++)
                            {
                                gridOfHouses[0].Add(0);
                            }
                        }
                        else
                        {
                            xPosition--;
                        }

                        break;
                    default:
                        throw new Exception("Divided by 0");
                }

                gridOfHouses[xPosition][yPosition]++;
            }

            return GetQuantityOfHousesPresentedWithGifts(gridOfHouses).ToString();
        }

        public override string Part2()
        {
            string input = ReadInputToEnd(@"Year2015\Input\Input03.txt");

            int xSantaPosition = 0;
            int ySantaPosition = 0;
            int xRoboPosition = 0;
            int yRoboPosition = 0;
            int maxCountOfHousesInColumn = 0;
            List<List<int>> gridOfHouses = new List<List<int>>();

            // First element 
            gridOfHouses.Add(new List<int>());
            gridOfHouses[0].Add(2);
            maxCountOfHousesInColumn++;

            for (int direction = 0; direction < input.Length; direction++)
            {
                bool isSantaMove = (direction % 2 == 0 || direction == 0);

                switch (input[direction])
                {
                    case '^':
                        if (isSantaMove)
                        {
                            GoNorth(gridOfHouses, ref maxCountOfHousesInColumn, ref ySantaPosition);
                        }
                        else
                        {
                            GoNorth(gridOfHouses, ref maxCountOfHousesInColumn, ref yRoboPosition);
                        }
                        break;
                    case '>':
                        if (isSantaMove)
                        {
                            GoWest(gridOfHouses, ref maxCountOfHousesInColumn, ref xSantaPosition);
                        }
                        else
                        {
                            GoWest(gridOfHouses, ref maxCountOfHousesInColumn, ref xRoboPosition);
                        }
                        break;
                    case 'v':
                        if (isSantaMove)
                        {
                            GoSouth(gridOfHouses, ref maxCountOfHousesInColumn, ref ySantaPosition, ref yRoboPosition);
                        }
                        else
                        {
                            GoSouth(gridOfHouses, ref maxCountOfHousesInColumn, ref yRoboPosition, ref ySantaPosition);
                        }
                        break;
                    case '<':
                        if (isSantaMove)
                        {
                            GoEast(gridOfHouses, ref maxCountOfHousesInColumn, ref xSantaPosition, ref xRoboPosition);
                        }
                        else
                        {
                            GoEast(gridOfHouses, ref maxCountOfHousesInColumn, ref xRoboPosition, ref xSantaPosition);
                        }
                        break;
                    default:
                        throw new Exception("Divided by 0");
                }

                if (isSantaMove)
                {
                    gridOfHouses[xSantaPosition][ySantaPosition]++;
                }
                else
                {
                    gridOfHouses[xRoboPosition][yRoboPosition]++;
                }
            }

            return GetQuantityOfHousesPresentedWithGifts(gridOfHouses).ToString();
        }

        private int GetQuantityOfHousesPresentedWithGifts(List<List<int>> gridOfHouses)
        {
            int quantityOfHousesPresentedWithGifts = 0;

            for (int i = 0; i < gridOfHouses.Count; i++)
            {
                quantityOfHousesPresentedWithGifts += gridOfHouses[i].Count(house => house > 0);
            }

            return quantityOfHousesPresentedWithGifts;
        }
        private void GoNorth(List<List<int>> gridOfHouses, ref int maxCountOfHousesInColumn, ref int yPosition)
        {
            // If y is on the last element, increase the maximum number by one and adjust all columns 
            if (yPosition == maxCountOfHousesInColumn - 1)
            {
                maxCountOfHousesInColumn++;

                for (int i = 0; i < gridOfHouses.Count; i++)
                {
                    for (int j = gridOfHouses[i].Count; j <= maxCountOfHousesInColumn; j++)
                    {
                        gridOfHouses[i].Add(0);
                    }
                }
            }

            yPosition++;
        }

        private void GoWest(List<List<int>> gridOfHouses, ref int maxCountOfHousesInColumn, ref int xPosition)
        {
            // If x is at the outer right border, add a new column at the back
            if (xPosition == gridOfHouses.Count - 1)
            {
                gridOfHouses.Add(new List<int>(maxCountOfHousesInColumn));

                for (int i = 0; i < maxCountOfHousesInColumn; i++)
                {
                    gridOfHouses[^1].Add(0);
                }
            }

            xPosition++;
        }

        private void GoSouth(List<List<int>> gridOfHouses, ref int maxCountOfHousesInColumn, ref int yPosition, ref int yOtherPosition)
        {
            // If y = 0, all columns must be extended by a 1st row because of the coordinate system
            if (yPosition == 0)
            {
                for (int i = 0; i < gridOfHouses.Count; i++)
                {
                    gridOfHouses[i].Insert(0, 0);
                }

                maxCountOfHousesInColumn++;
                yOtherPosition++;
            }
            else
            {
                yPosition--;
            }
        }

        private void GoEast(List<List<int>> gridOfHouses, ref int maxCountOfHousesInColumn, ref int xPosition, ref int xOtherPosition)
        {
            // If x is at the outer left border, add a new column at the front
            if (xPosition == 0)
            {
                gridOfHouses.Insert(0, new List<int>(maxCountOfHousesInColumn));

                for (int i = 0; i < maxCountOfHousesInColumn; i++)
                {
                    gridOfHouses[0].Add(0);
                }

                xOtherPosition++;
            }
            else
            {
                xPosition--;
            }
        }
    }
}
