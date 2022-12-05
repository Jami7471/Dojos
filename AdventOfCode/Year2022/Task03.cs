using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    public sealed class Task03 : BaseTask
    {
        /*
        
        --- Day 3: Rucksack Reorganization ---
        One Elf has the important job of loading all of the rucksacks with supplies for the jungle journey. 
        Unfortunately, that Elf didn't quite follow the packing instructions, and so a few items now need to be rearranged.

        Each rucksack has two large compartments. All items of a given type are meant to go into exactly one of the two compartments. 
        The Elf that did the packing failed to follow this rule for exactly one item type per rucksack.

        The Elves have made a list of all of the items currently in each rucksack (your puzzle input), but they need your help finding the errors. 
        Every item type is identified by a single lowercase or uppercase letter (that is, a and A refer to different types of items).

        The list of items for each rucksack is given as characters all on a single line. 
        A given rucksack always has the same number of items in each of its two compartments, 
        so the first half of the characters represent items in the first compartment, while the second half of the characters represent items in the second compartment.

        For example, suppose you have the following list of contents from six rucksacks:        
        
            vJrwpWtwJgWrhcsFMMfFFhFp
            jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
            PmmdzqPrVvPwwTWBwg
            wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
            ttgJtRGJQctTZtZT
            CrZsJsPPZsGzwwsLwLmpwMDw

            - The first rucksack contains the items vJrwpWtwJgWrhcsFMMfFFhFp, which means its first compartment contains the items vJrwpWtwJgWr, 
              while the second compartment contains the items hcsFMMfFFhFp. 
              The only item type that appears in both compartments is lowercase p.
            - The second rucksack's compartments contain jqHRNqRjqzjGDLGL and rsFMfFZSrLrFZsSL.
              The only item type that appears in both compartments is uppercase L.
            - The third rucksack's compartments contain PmmdzqPrV and vPwwTWBwg; the only common item type is uppercase P.
            - The fourth rucksack's compartments only share item type v.
            - The fifth rucksack's compartments only share item type t.
            - The sixth rucksack's compartments only share item type s.

        To help prioritize item rearrangement, every item type can be converted to a priority:
            - Lowercase item types a through z have priorities 1 through 26.
            - Uppercase item types A through Z have priorities 27 through 52.
         
        In the above example, the priority of the item type that appears in both compartments of each rucksack is 16 (p), 38 (L), 42 (P), 22 (v), 20 (t), and 19 (s); 
        the sum of these is 157.
        
        Task: Find the item type that appears in both compartments of each rucksack. What is the sum of the priorities of those item types?
        Solution: 7917

        --- Part Two ---
        As you finish identifying the misplaced items, the Elves come to you with another issue.

        For safety, the Elves are divided into groups of three. Every Elf carries a badge that identifies their group. 
        For efficiency, within each group of three Elves, the badge is the only item type carried by all three Elves. 
        That is, if a group's badge is item type B, then all three Elves will have item type B somewhere in their rucksack, 
        and at most two of the Elves will be carrying any other item type.

        The problem is that someone forgot to put this year's updated authenticity sticker on the badges. 
        All of the badges need to be pulled out of the rucksacks so the new authenticity stickers can be attached.

        Additionally, nobody wrote down which item type corresponds to each group's badges. 
        The only way to tell which item type is the right one is by finding the one item type that is common between all three Elves in each group.

        Every set of three lines in your list corresponds to a single group, but each group can have a different badge item type. 
        So, in the above example, the first group's rucksacks are the first three lines:

            vJrwpWtwJgWrhcsFMMfFFhFp
            jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
            PmmdzqPrVvPwwTWBwg

        And the second group's rucksacks are the next three lines:

            wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
            ttgJtRGJQctTZtZT
            CrZsJsPPZsGzwwsLwLmpwMDw

        In the first group, the only item type that appears in all three rucksacks is lowercase r; this must be their badges. 
        In the second group, their badge item type must be Z.

        Priorities for these items must still be found to organize the sticker attachment efforts: 
        here, they are 18 (r) for the first group and 52 (Z) for the second group. The sum of these is 70.

        Task: Find the item type that corresponds to the badges of each three-Elf group. What is the sum of the priorities of those item types?
        Solution: 2585
         
        */

        public override string Part1()
        {
            List<string> input = ReadInputLines(@"Year2022\Input\Input03.txt");
            List<Backpack> backpacks = ConvertInputToBackpacks(input);
            int sumOfPriorities = 0;

            foreach (Backpack backpack in backpacks)
            {
                sumOfPriorities = AddPriorityValue(sumOfPriorities, backpack.ItemTypes);
            }

            return sumOfPriorities.ToString();
        }

        public override string Part2()
        {
            List<string> input = ReadInputLines(@"Year2022\Input\Input03.txt");
            List<Backpack> backpacks = ConvertInputToBackpacks(input);
            List<BackpackGroup> backpackGroups = GroupBackpacksIntoGroups(backpacks);
            int sumOfPriorities = 0;

            foreach (BackpackGroup backpackGroup in backpackGroups)
            {
                char bagde = ' ';

                if (backpackGroup.Badge.HasValue)
                {
                    bagde = backpackGroup.Badge.Value;
                }
                else
                {
                    continue;
                }

                sumOfPriorities = AddPriorityValue(sumOfPriorities, bagde);
            }

            return sumOfPriorities.ToString();
        }

        private List<BackpackGroup> GroupBackpacksIntoGroups(List<Backpack> backpacks)
        {
            List<BackpackGroup> result = new();

            foreach (Backpack backpack in backpacks)
            {
                if (result.Count == 0 || result[^1].Backpacks.Count % 3 == 0)
                {
                    result.Add(new BackpackGroup());
                }

                result[^1].Backpacks.Add(backpack);
            }

            return result;
        }

        private List<Backpack> ConvertInputToBackpacks(List<string> input)
        {
            List<Backpack> result = new();

            foreach (string backpackContent in input)
            {
                result.Add(new Backpack(backpackContent));
            }

            return result;
        }

        private int AddPriorityValue(int sumOfPriorities, char itemType)
        {
            if (char.IsUpper(itemType))
            {
                sumOfPriorities += GetUppercasePriorityValue(itemType);
            }
            else
            {
                sumOfPriorities += GetLowercasePriorityValue(itemType);
            }

            return sumOfPriorities;
        }

        private int AddPriorityValue(int sumOfPriorities, List<char> itemTypes)
        {
            foreach (char item in itemTypes)
            {
                if (char.IsUpper(item))
                {
                    sumOfPriorities += GetUppercasePriorityValue(item);
                }
                else
                {
                    sumOfPriorities += GetLowercasePriorityValue(item);
                }
            }

            return sumOfPriorities;
        }

        private int GetLowercasePriorityValue(char c)
        {
            // Lowercase item types a through z have priorities 1 through 26.
            return (int)c + 1 - (int)'a';
        }

        private int GetUppercasePriorityValue(char c)
        {
            // Uppercase item types A through Z have priorities 27 through 52.
            return (int)c + 27 - (int)'A';
        }
    }

    public class BackpackGroup
    {
        private List<Backpack> _backpacks = new();

        public List<Backpack> Backpacks
        {
            get { return _backpacks; }
            set { _backpacks = value; }
        }

        private char? _badge;

        public char? Badge
        {
            get
            {
                if (_badge == null)
                {
                    _badge = GetBadge();
                }

                return _badge;
            }
        }

        private char GetBadge()
        {
            foreach (char c in _backpacks[0].CompartmentAll)
            {
                if (_backpacks[1].CompartmentAll.Contains(c) && _backpacks[2].CompartmentAll.Contains(c))
                {
                    return c;
                }
            }

            return ' ';
        }
    }

    public class Backpack
    {
        public Backpack(string content)
        {
            CompartmentAll = content;
            Compartment1 = content.Substring(0, content.Length / 2);
            Compartment2 = content.Substring(content.Length / 2);
        }

        public string CompartmentAll { get; set; }

        public string Compartment1 { get; set; }

        public string Compartment2 { get; set; }

        private List<char>? _itemTypes;

        public List<char> ItemTypes
        {
            get
            {
                if (_itemTypes == null)
                {
                    _itemTypes = new();
                    CompareItemTypes();
                }

                return _itemTypes;
            }
        }


        private void CompareItemTypes()
        {
            foreach (char c in Compartment1)
            {
                if (Compartment2.Contains(c) && _itemTypes?.Contains(c) == false)
                {
                    _itemTypes.Add(c);
                }
            }
        }
    }
}
