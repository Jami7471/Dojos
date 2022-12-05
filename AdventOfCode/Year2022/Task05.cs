using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static AdventOfCode.Year2022.Task04;

namespace AdventOfCode.Year2022
{
    public sealed class Task05 : BaseTask
    {
        /*
         
        --- Day 5: Supply Stacks ---
        The expedition can depart as soon as the final supplies have been unloaded from the ships. 
        Supplies are stored in stacks of marked crates, but because the needed supplies are buried under many other crates, the crates need to be rearranged.

        The ship has a giant cargo crane capable of moving crates between stacks. 
        To ensure none of the crates get crushed or fall over, the crane operator will rearrange them in a series of carefully-planned steps. 
        After the crates are rearranged, the desired crates will be at the top of each stack.

        The Elves don't want to interrupt the crane operator during this delicate procedure, but they forgot to ask her which crate will end up where,
        and they want to be ready to unload them as soon as possible so they can embark.

        They do, however, have a drawing of the starting stacks of crates and the rearrangement procedure (your puzzle input). For example:

                [D]    
            [N] [C]    
            [Z] [M] [P]
             1   2   3 

            move 1 from 2 to 1
            move 3 from 1 to 3
            move 2 from 2 to 1
            move 1 from 1 to 2
        
        In this example, there are three stacks of crates.
        Stack 1 contains two crates: crate Z is on the bottom, and crate N is on top. 
        Stack 2 contains three crates; from bottom to top, they are crates M, C, and D. 
        Finally, stack 3 contains a single crate, P.

        Then, the rearrangement procedure is given. 
        In each step of the procedure, a quantity of crates is moved from one stack to a different stack. 
        In the first step of the above rearrangement procedure, one crate is moved from stack 2 to stack 1, resulting in this configuration:

            [D]        
            [N] [C]    
            [Z] [M] [P]
             1   2   3 

        In the second step, three crates are moved from stack 1 to stack 3. 
        Crates are moved one at a time, so the first crate to be moved (D) ends up below the second and third crates:

                    [Z]
                    [N]
                [C] [D]
                [M] [P]
             1   2   3

        Then, both crates are moved from stack 2 to stack 1. 
        Again, because crates are moved one at a time, crate C ends up below crate M:

                    [Z]
                    [N]
            [M]     [D]
            [C]     [P]
             1   2   3

        Finally, one crate is moved from stack 1 to stack 2:

                    [Z]
                    [N]
                    [D]
            [C] [M] [P]
             1   2   3

        The Elves just need to know which crate will end up on top of each stack; 
        in this example, the top crates are C in stack 1, M in stack 2, and Z in stack 3, so you should combine these together and give the Elves the message CMZ.

        Task: After the rearrangement procedure completes, what crate ends up on top of each stack?
        Solution: TDCHVHJTG
         
        --- Part Two ---
        As you watch the crane operator expertly rearrange the crates, you notice the process isn't following your prediction.

        Some mud was covering the writing on the side of the crane, and you quickly wipe it away. The crane isn't a CrateMover 9000 - it's a CrateMover 9001.

        The CrateMover 9001 is notable for many new and exciting features: air conditioning, leather seats, an extra cup holder, 
        and the ability to pick up and move multiple crates at once.

        Again considering the example above, the crates begin in the same configuration:

                [D]    
            [N] [C]    
            [Z] [M] [P]
             1   2   3 

        Moving a single crate from stack 2 to stack 1 behaves the same as before:

        [D]        
        [N] [C]    
        [Z] [M] [P]
         1   2   3 

        However, the action of moving three crates from stack 1 to stack 3 means that those three moved crates stay in the same order, resulting in this new configuration:

                    [D]
                    [N]
                [C] [Z]
                [M] [P]
             1   2   3

        Next, as both crates are moved from stack 2 to stack 1, they retain their order as well:

                    [D]
                    [N]
            [C]     [Z]
            [M]     [P]
             1   2   3

        Finally, a single crate is still moved from stack 1 to stack 2, but now it's crate C that gets moved:

                    [D]
                    [N]
                    [Z]
            [M] [C] [P]
             1   2   3

        In this example, the CrateMover 9001 has put the crates in a totally different order: MCD.

        Before the rearrangement process finishes, update your simulation so that the Elves know where they should stand to be ready to unload the final supplies. 
        
        Task: After the rearrangement procedure completes, what crate ends up on top of each stack?
        Solution: NGCMPJLHV

         */

        public override string Part1()
        {
            List<string> input = ReadInputLines(@"Year2022\Input\Input05.txt");

            List<int> indexOfStackNumbers = GetIndexOfStackNumbers(input, out int stackNumberLineIndex);
            Dictionary<int, List<char>> stacks = CreateStacks(input.GetRange(0, stackNumberLineIndex), indexOfStackNumbers);
            List<RearrangementProcedure> rearrangementProcedures= GetRearrangementProcedures(input.GetRange(stackNumberLineIndex + 2, input.Count - stackNumberLineIndex - 2));

            SortCrates(stacks, rearrangementProcedures);

            string lastCrateChars = GetLastCrateChars(stacks);

            return lastCrateChars;
        }


        public override string Part2()
        {
            List<string> input = ReadInputLines(@"Year2022\Input\Input05.txt");

            List<int> indexOfStackNumbers = GetIndexOfStackNumbers(input, out int stackNumberLineIndex);
            Dictionary<int, List<char>> stacks = CreateStacks(input.GetRange(0, stackNumberLineIndex), indexOfStackNumbers);
            List<RearrangementProcedure> rearrangementProcedures = GetRearrangementProcedures(input.GetRange(stackNumberLineIndex + 2, input.Count - stackNumberLineIndex - 2));

            SortCrates(stacks, rearrangementProcedures, true);

            string lastCrateChars = GetLastCrateChars(stacks);

            return lastCrateChars;
        }

        private string GetLastCrateChars(Dictionary<int, List<char>> stacks)
        {
            StringBuilder stringBuilder = new StringBuilder();

            for(int i = 0; i < stacks.Count; i++)
            {
                stringBuilder.Append(stacks[i+1].Last());
            }

            return stringBuilder.ToString();
        }


        private void SortCrates(Dictionary<int, List<char>> stacks, List<RearrangementProcedure> rearrangementProcedures, bool isCrateMover9001 = false)
        {
            foreach(RearrangementProcedure rearrangementProcedure in rearrangementProcedures) 
            {
                if (isCrateMover9001)
                {
                    List<char> crates = stacks[rearrangementProcedure.FromStack]
                        .GetRange(stacks[rearrangementProcedure.FromStack].Count - rearrangementProcedure.Count, 
                            rearrangementProcedure.Count);
                    stacks[rearrangementProcedure.ToStack].AddRange(crates);
                    stacks[rearrangementProcedure.FromStack]
                        .RemoveRange(stacks[rearrangementProcedure.FromStack].Count - rearrangementProcedure.Count,
                            rearrangementProcedure.Count);
                }
                else
                {
                    for (int i = 0; i < rearrangementProcedure.Count; i++)
                    {
                        char crate = stacks[rearrangementProcedure.FromStack].Last();
                        stacks[rearrangementProcedure.ToStack].Add(crate);
                        stacks[rearrangementProcedure.FromStack].RemoveAt(stacks[rearrangementProcedure.FromStack].Count - 1);
                    }
                }
            }
        }

        private List<int> GetIndexOfStackNumbers(List<string> content, out int stackNumberLineIndex)
        {
            List<int> result = new();
            stackNumberLineIndex = -1;

            for (int i = 0; i < content.Count && result.Count == 0; i++)
            {
                if (char.IsDigit(content[i].Trim()[0]))
                {
                    for (int j = 0; j < content[i].Length; j++)
                    {
                        if (char.IsDigit(content[i][j]))
                        {
                            result.Add(j);
                        }
                    }

                    stackNumberLineIndex = i;
                }
            }

            return result;
        }

        private Dictionary<int, List<char>> CreateStacks(List<string> content, List<int> indexOfStackNumbers)
        {
            Dictionary<int, List<char>> stacks = new ();

            for(int i = 1; i <= indexOfStackNumbers.Count; i++)
            {
                stacks.Add(i, new List<char>());
            }

            for (int i = content.Count - 1; i > -1; i--)
            {
                for (int j = 0; j < content[i].Length; j++)
                {
                    if (char.IsLetter(content[i][j]))
                    {
                        int stackNumber = indexOfStackNumbers.IndexOf(j) + 1;
                        stacks[stackNumber].Add(content[i][j]);
                    }
                }
            }

            return stacks;
        } 

        private List<RearrangementProcedure> GetRearrangementProcedures(List<string> content)
        {
            List<RearrangementProcedure> result = new();

            foreach(string rearrangementProcedure in content)
            {
                string[] parts = rearrangementProcedure
                    .Replace("move ", "")
                    .Replace(" from ", ";")
                    .Replace(" to ", ";")
                    .Split(";");

                result.Add(new RearrangementProcedure(Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]), Convert.ToInt32(parts[0])));
            }

            return result;
        }

        public sealed class RearrangementProcedure
        {
            public RearrangementProcedure(int fromStack, int toStack, int count)
            {
                FromStack = fromStack;
                ToStack = toStack;
                Count = count;
            }

            public int FromStack { get; set; }

            public int ToStack { get; set; }

            public int Count { get; set; }
        }
    }
}
