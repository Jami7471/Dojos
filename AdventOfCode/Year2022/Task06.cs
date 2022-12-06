using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2022
{
    public sealed class Task06 : BaseTask
    {
        /*
         
        --- Day 6: Tuning Trouble ---
        The preparations are finally complete; you and the Elves leave camp on foot and begin to make your way toward the star fruit grove.

        As you move through the dense undergrowth, one of the Elves gives you a handheld device. 
        He says that it has many fancy features, but the most important one to set up right now is the communication system.

        However, because he's heard you have significant experience dealing with signal-based systems,
        he convinced the other Elves that it would be okay to give you their one malfunctioning device - surely you'll have no problem fixing it.

        As if inspired by comedic timing, the device emits a few colorful sparks.

        To be able to communicate with the Elves, the device needs to lock on to their signal.
        The signal is a series of seemingly-random characters that the device receives one at a time.

        To fix the communication system, you need to add a subroutine to the device that detects a start-of-packet marker in the datastream.
        In the protocol being used by the Elves, the start of a packet is indicated by a sequence of four characters that are all different.

        The device will send your subroutine a datastream buffer (your puzzle input);
        your subroutine needs to identify the first position where the four most recently received characters were all different.
        Specifically, it needs to report the number of characters from the beginning of the buffer to the end of the first such four-character marker.

        For example, suppose you receive the following datastream buffer:

            mjqjpqmgbljsphdztnvjfqwrcgsmlb

        After the first three characters (mjq) have been received, there haven't been enough characters received yet to find the marker.
        The first time a marker could occur is after the fourth character is received, making the most recent four characters mjqj.
        Because j is repeated, this isn't a marker.

        The first time a marker appears is after the seventh character arrives.
        Once it does, the last four characters received are jpqm, which are all different.
        In this case, your subroutine should report the value 7, because the first start-of-packet marker is complete after 7 characters have been processed.

        Here are a few more examples:

            - bvwbjplbgvbhsrlpgdmjqwftvncz: first marker after character 5
            - nppdvjthqldpwncqszvftbrmjlhg: first marker after character 6
            - nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg: first marker after character 10
            - zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw: first marker after character 11

        Task: How many characters need to be processed before the first start-of-packet marker is detected?
        Solution: 1804

        --- Part Two ---
        Your device's communication system is correctly detecting packets, but still isn't working. 
        It looks like it also needs to look for messages.

        A start-of-message marker is just like a start-of-packet marker, except it consists of 14 distinct characters rather than 4.

        Here are the first positions of start-of-message markers for all of the above examples:

            - mjqjpqmgbljsphdztnvjfqwrcgsmlb: first marker after character 19
            - bvwbjplbgvbhsrlpgdmjqwftvncz: first marker after character 23
            - nppdvjthqldpwncqszvftbrmjlhg: first marker after character 23
            - nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg: first marker after character 29
            - zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw: first marker after character 26
       
        Task: How many characters need to be processed before the first start-of-message marker is detected?
        Solution: 2508

         */

        public override string Part1()
        {
            string signal = ReadInputToEnd(@"Year2022\Input\Input06.txt");
            int indexOfAfterStartOfPacket = IndexOfAfterStartOfPacket(signal);
            return indexOfAfterStartOfPacket.ToString();
        }

        public override string Part2()
        {
            string signal = ReadInputToEnd(@"Year2022\Input\Input06.txt");
            int indexOfAfterStartOfMessage = IndexOfAfterStartOfMessage(signal);
            return indexOfAfterStartOfMessage.ToString();
        }

        private int IndexOfAfterStartOfPacket(string signal)
        {
            return IndexOfAfterCountOfUniqueCharcters(signal, 4);
        }

        private int IndexOfAfterStartOfMessage(string signal)
        {
            return IndexOfAfterCountOfUniqueCharcters(signal, 14);
        }

        private int IndexOfAfterCountOfUniqueCharcters(string signal, int countOfUniqueCharacters)
        {
            char[] startPacket = new char[countOfUniqueCharacters];

            for (int i = 0; i < signal.Length; i++)
            {
                if (i < countOfUniqueCharacters - 1)
                {
                    startPacket[i] = signal[i];
                }
                else
                {
                    if (i == countOfUniqueCharacters - 1)
                    {
                        startPacket[i] = signal[i];
                    }
                    else
                    {
                        startPacket = AddCharacterToCharacters(signal[i], startPacket);
                    }

                    if (AllCharactersAreUnique(startPacket))
                    {
                        return i + 1;
                    }
                }
            }

            return -1;
        }

        private bool AllCharactersAreUnique(char[] chars)
        {
            IEnumerable<char> distinctChars = chars.Distinct();
            return (chars.Length == distinctChars.Count());
        }

        private char[] AddCharacterToCharacters(char character, char[] characters)
        {
            for(int i = 0; i < characters.Length; i++)
            {
                if(i < characters.Length - 1)
                {
                    characters[i] = characters[i + 1];
                }
                else
                {
                    characters[i] = character;
                }
            }

            return characters;
        }
    }
}
