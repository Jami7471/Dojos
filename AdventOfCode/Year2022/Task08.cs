using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    public sealed class Task08 : BaseTask
    {
        /*
        
        --- Day 8: Treetop Tree House ---
        The expedition comes across a peculiar patch of tall trees all planted carefully in a grid.
        The Elves explain that a previous expedition planted these trees as a reforestation effort.
        Now, they're curious if this would be a good location for a tree house.

        First, determine whether there is enough tree cover here to keep a tree house hidden.
        To do this, you need to count the number of trees that are visible from outside the grid when looking directly along a row or column.

        The Elves have already launched a quadcopter to generate a map with the height of each tree (your puzzle input). For example:

            30373
            25512
            65332
            33549
            35390

        Each tree is represented as a single digit whose value is its height, where 0 is the shortest and 9 is the tallest.

        A tree is visible if all of the other trees between it and an edge of the grid are shorter than it. Only consider trees in the same row or column; that is, only look up, down, left, or right from any given tree.

        All of the trees around the edge of the grid are visible - since they are already on the edge, there are no trees to block the view. In this example, that only leaves the interior nine trees to consider:

            - The top-left 5 is visible from the left and top. (It isn't visible from the right or bottom since other trees of height 5 are in the way.)
            - The top-middle 5 is visible from the top and right.
            - The top-right 1 is not visible from any direction; for it to be visible, there would need to only be trees of height 0 between it and an edge.
            - The left-middle 5 is visible, but only from the right.
            - The center 3 is not visible from any direction; for it to be visible, there would need to be only trees of at most height 2 between it and an edge.
            - The right-middle 3 is visible from the right.
            - In the bottom row, the middle 5 is visible, but the 3 and 4 are not.

        With 16 trees visible on the edge and another 5 visible in the interior, a total of 21 trees are visible in this arrangement.

        Task: Consider your map; how many trees are visible from outside the grid?
        Solution: 1818
         
        --- Part Two ---
        Content with the amount of tree cover available, the Elves just need to know the best spot to build their tree house:
        they would like to be able to see a lot of trees.

        To measure the viewing distance from a given tree, look up, down, left, and right from that tree;
        stop if you reach an edge or at the first tree that is the same height or taller than the tree under consideration.
        (If a tree is right on the edge, at least one of its viewing distances will be zero.)

        The Elves don't care about distant trees taller than those found by the rules above;
        the proposed tree house has large eaves to keep it dry, so they wouldn't be able to see higher than the tree house anyway.

        In the example above, consider the middle 5 in the second row:

            30373
            25512
            65332
            33549
            35390

            - Looking up, its view is not blocked; it can see 1 tree (of height 3).
            - Looking left, its view is blocked immediately; it can see only 1 tree (of height 5, right next to it).
            - Looking right, its view is not blocked; it can see 2 trees.
            - Looking down, its view is blocked eventually; it can see 2 trees
              (one of height 3, then the tree of height 5 that blocks its view).
            - A tree's scenic score is found by multiplying together its viewing distance in each of the four directions.
              For this tree, this is 4 (found by multiplying 1 * 1 * 2 * 2).

        However, you can do even better: consider the tree of height 5 in the middle of the fourth row:

            30373
            25512
            65332
            33549
            35390

            - Looking up, its view is blocked at 2 trees (by another tree with a height of 5).
            - Looking left, its view is not blocked; it can see 2 trees.
            - Looking down, its view is also not blocked; it can see 1 tree.
            - Looking right, its view is blocked at 2 trees (by a massive tree of height 9).
            - This tree's scenic score is 8 (2 * 2 * 1 * 2); this is the ideal spot for the tree house.

        Task: Consider each tree on your map. What is the highest scenic score possible for any tree?
        Solution: 368368

         */

        public override string Part1()
        {
            List<string> input = ReadInputLines(@"Year2022\Input\Input08.txt");

            List<List<int>> treeMap = new();
            FillTreeMap(input, treeMap);

            int visibleTrees = CountVisibleTrees(treeMap);

            return visibleTrees.ToString();
        }

        public override string Part2()
        {
            List<string> input = ReadInputLines(@"Year2022\Input\Input08.txt");

            List<List<int>> treeMap = new();
            FillTreeMap(input, treeMap);

            int highestScenicScore = DetermineHighestScenicScore(treeMap);

            return highestScenicScore.ToString();
        }

        private int DetermineHighestScenicScore(List<List<int>> treeMap)
        {
            int highestScenicScore = 0;

            for (int x = 1; x < treeMap.Count - 1; x++)
            {
                for (int y = 1; y < treeMap[x].Count - 1; y++)
                {
                    int currentScenicScore = GetCurrentScenicScore(treeMap, x, y);

                    if (highestScenicScore < currentScenicScore)
                    {
                        highestScenicScore = currentScenicScore;
                    }
                }
            }

            return highestScenicScore;
        }

        private int GetCurrentScenicScore(List<List<int>> treeMap, int x, int y)
        {
            int leftDistance = GetDistanceToLeft(treeMap, x, y);
            int rightDistance = GetDistanceToRight(treeMap, x, y);
            int topDistance = GetDistanceToTop(treeMap, x, y);
            int bottomDistance = GetDistanceToBottom(treeMap, x, y);
            int currentScenicScore = leftDistance * rightDistance * topDistance * bottomDistance;
            return currentScenicScore;
        }

        private int GetDistanceToLeft(List<List<int>> treeMap, int x, int y)
        {
            int currentTreeSize = treeMap[x][y];

            for (int lx = x - 1; lx > -1; lx--)
            {
                int compareTreeSize = treeMap[lx][y];

                if (currentTreeSize <= compareTreeSize)
                {
                    return x - lx;
                }
            }

            return x;
        }

        private int GetDistanceToRight(List<List<int>> treeMap, int x, int y)
        {
            int currentTreeSize = treeMap[x][y];

            for (int rx = x + 1; rx < treeMap.Count; rx++)
            {
                int compareTreeSize = treeMap[rx][y];

                if (currentTreeSize <= compareTreeSize)
                {
                    return rx - x;
                }
            }

            return treeMap.Count - x - 1;
        }

        private int GetDistanceToTop(List<List<int>> treeMap, int x, int y)
        {
            int currentTreeSize = treeMap[x][y];

            for (int ty = y - 1; ty > -1; ty--)
            {
                int compareTreeSize = treeMap[x][ty];

                if (currentTreeSize <= compareTreeSize)
                {
                    return y - ty;
                }
            }

            return y;
        }

        private int GetDistanceToBottom(List<List<int>> treeMap, int x, int y)
        {
            int currentTreeSize = treeMap[x][y];

            for (int by = y + 1; by < treeMap[x].Count; by++)
            {
                int compareTreeSize = treeMap[x][by];

                if (currentTreeSize <= compareTreeSize)
                {
                    return by - y;
                }
            }

            return treeMap[x].Count - y - 1;
        }

        private int CountVisibleTrees(List<List<int>> treeMap)
        {
            // They are all visible in the outer border. Edges not counted twice.
            int visibleTrees = 2 * treeMap.Count + 2 * treeMap[0].Count - 4;

            for (int x = 1; x < treeMap.Count - 1; x++)
            {
                for (int y = 1; y < treeMap[x].Count - 1; y++)
                {
                    if (IsNotVisibleFromLeft(treeMap, x, y, treeMap[x][y]) == false
                        || IsNotVisibleFromRight(treeMap, x, y, treeMap[x][y]) == false
                        || IsNotVisibleFromTop(treeMap, x, y, treeMap[x][y]) == false
                        || IsNotVisibleFromBottom(treeMap, x, y, treeMap[x][y]) == false)
                    {
                        visibleTrees++;
                    }
                }
            }

            return visibleTrees;
        }

        private bool IsNotVisibleFromLeft(List<List<int>> treeMap, int x, int y, int currentTreeSize)
        {
            for (int lx = 0; lx < x; lx++)
            {
                int compareTreeSize = treeMap[lx][y];

                if (currentTreeSize <= compareTreeSize)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsNotVisibleFromRight(List<List<int>> treeMap, int x, int y, int currentTreeSize)
        {
            for (int rx = treeMap.Count - 1; rx > x; rx--)
            {
                int compareTreeSize = treeMap[rx][y];

                if (currentTreeSize <= compareTreeSize)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsNotVisibleFromTop(List<List<int>> treeMap, int x, int y, int currentTreeSize)
        {
            for (int ty = 0; ty < y; ty++)
            {
                int compareTreeSize = treeMap[x][ty];

                if (currentTreeSize <= compareTreeSize)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsNotVisibleFromBottom(List<List<int>> treeMap, int x, int y, int currentTreeSize)
        {
            for (int by = treeMap.Count - 1; by > y; by--)
            {
                int compareTreeSize = treeMap[x][by];

                if (currentTreeSize <= compareTreeSize)
                {
                    return true;
                }
            }

            return false;
        }

        private void FillTreeMap(List<string> input, List<List<int>> treeMap)
        {
            foreach (string line in input)
            {
                if (treeMap.Count == 0)
                {
                    AddLineToTreeMap(treeMap, line, true);
                }
                else
                {
                    AddLineToTreeMap(treeMap, line);
                }
            }
        }

        private void AddLineToTreeMap(List<List<int>> treeMap, string line, bool isNewMap = false)
        {
            for (int i = 0; i < line.Length; i++)
            {
                int treeSize = Convert.ToInt32(line[i].ToString());

                if (isNewMap)
                {
                    treeMap.Add(new List<int> { treeSize });
                }
                else
                {
                    treeMap[i].Add(treeSize);
                }
            }
        }
    }
}
