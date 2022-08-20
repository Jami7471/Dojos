using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2015
{
    public sealed class Task13 : BaseTask
    {
        /*

        --- Day 13: Knights of the Dinner Table ---
        In years past, the holiday feast with your family hasn't gone so well. 
        Not everyone gets along! This year, you resolve, will be different. 
        You're going to find the optimal seating arrangement and avoid all those awkward conversations.

        You start by writing up a list of everyone invited and the amount their happiness would increase or decrease if they were to find themselves sitting next to each other person.
        You have a circular table that will be just big enough to fit everyone comfortably, and so each person will have exactly two neighbors.

        For example, suppose you have only four attendees planned, and you calculate their potential happiness as follows:

            Alice would gain 54 happiness units by sitting next to Bob.
            Alice would lose 79 happiness units by sitting next to Carol.
            Alice would lose 2 happiness units by sitting next to David.
            Bob would gain 83 happiness units by sitting next to Alice.
            Bob would lose 7 happiness units by sitting next to Carol.
            Bob would lose 63 happiness units by sitting next to David.
            Carol would lose 62 happiness units by sitting next to Alice.
            Carol would gain 60 happiness units by sitting next to Bob.
            Carol would gain 55 happiness units by sitting next to David.
            David would gain 46 happiness units by sitting next to Alice.
            David would lose 7 happiness units by sitting next to Bob.
            David would gain 41 happiness units by sitting next to Carol.

        Then, if you seat Alice next to David, Alice would lose 2 happiness units (because David talks so much), but David would gain 46 happiness units (because Alice is such a good listener), for a total change of 44.

        If you continue around the table, you could then seat Bob next to Alice (Bob gains 83, Alice gains 54). Finally, seat Carol, who sits next to Bob (Carol gains 60, Bob loses 7) and David (Carol gains 55, David gains 41).
        The arrangement looks like this:

                 +41 +46
            +55   David    -2
            Carol       Alice
            +60    Bob    +54
                 -7  +83

        After trying every other seating arrangement in this hypothetical scenario, you find that this one is the most optimal, with a total change in happiness of 330.

        Task: What is the total change in happiness for the optimal seating arrangement of the actual guest list?
        Solution: 709

        --- Part Two ---
        In all the commotion, you realize that you forgot to seat yourself.
        At this point, you're pretty apathetic toward the whole thing, and your happiness wouldn't really go up or down regardless of who you sit next to.
        You assume everyone else would be just as ambivalent about sitting next to you, too.

        So, add yourself to the list, and give all happiness relationships that involve you a score of 0.

        Task: What is the total change in happiness for the optimal seating arrangement that actually includes yourself?
        Solution: 668

        */

        public override string Part1()
        {
            List<string> input = ReadInputLines(@"Year2015\Input\Input13.txt");

            List<PersonNode> convertedPersonList = ConvertToPersonList(input, out List<string> personNames);
            List<PersonNode> allSeatingArrangements = CreateAllSeatingArrangements(personNames, convertedPersonList);
            List<PersonNode> lastPersonNodeList = new List<PersonNode>();

            foreach (PersonNode personNode in allSeatingArrangements)
            {
                FillLastPersonNodeList(lastPersonNodeList, personNode);
            }
            
            return lastPersonNodeList.Where(pl => pl.ItemOrder >= personNames.Count).Max(pl => pl.HappyinessUnitsToNeighbourFromBeginningToEnd).ToString();
        }

        public override string Part2()
        {
            List<string> input = ReadInputLines(@"Year2015\Input\Input13.txt");

            List<PersonNode> convertedPersonList = ConvertToPersonList(input, out List<string> personNames, true);
            List<PersonNode> allSeatingArrangements = CreateAllSeatingArrangements(personNames, convertedPersonList);
            List<PersonNode> lastPersonNodeList = new List<PersonNode>();

            foreach (PersonNode personNode in allSeatingArrangements)
            {
                FillLastPersonNodeList(lastPersonNodeList, personNode);
            }

            return lastPersonNodeList.Where(pl => pl.ItemOrder >= personNames.Count).Max(pl => pl.HappyinessUnitsToNeighbourFromBeginningToEnd).ToString();
        }

        private List<PersonNode> ConvertToPersonList(List<string> persons, out List<string> personNames, bool addMyself = false)
        {
            List<PersonNode> convertedPersonList = new List<PersonNode>();
            Dictionary<string, string[]> personIndices = new Dictionary<string, string[]>();

            personNames = new List<string>();

            CreatePersonIndices(persons, personIndices);
            FillConvertesPersonList(personNames, convertedPersonList, personIndices);

            if (addMyself)
            {
                foreach (string personName in personNames)
                {
                    convertedPersonList.Add(new PersonNode("I", personName, 0));
                    convertedPersonList.Add(new PersonNode(personName, "I", 0));
                }

                personNames.Add("I");
            }

            return convertedPersonList;
        }

        private void FillConvertesPersonList(List<string> personNames, List<PersonNode> convertedPersonList, Dictionary<string, string[]> personIndices)
        {
            foreach (KeyValuePair<string, string[]> personIndex in personIndices)
            {
                int happyUnit = Convert.ToInt32(personIndex.Value[1]);

                if (personIndices.ContainsKey($"{personIndex.Value[2]}~{personIndex.Value[0]}"))
                {
                    happyUnit += Convert.ToInt32(personIndices[$"{personIndex.Value[2]}~{personIndex.Value[0]}"][1]);
                }
                else
                {
                    throw new KeyNotFoundException($"Key '{personIndex.Value[2]}~{personIndex.Value[0]}' not found");
                }

                convertedPersonList.Add(new PersonNode(personIndex.Value[0], personIndex.Value[2], happyUnit));

                if (personNames.Contains(personIndex.Value[0]) == false)
                {
                    personNames.Add(personIndex.Value[0]);
                }
            }
        }

        private void CreatePersonIndices(List<string> persons, Dictionary<string, string[]> personIndices)
        {
            for (int i = 0; i < persons.Count; i++)
            {
                string[] personValues = GetPersonValues(persons[i]);
                personIndices.Add($"{personValues[0]}~{personValues[2]}", personValues);
            }
        }

        private string[] GetPersonValues(string person)
        {
            bool isLose = person.Contains("lose");

            person = person.Replace(" would gain ", " ");
            person = person.Replace(" would lose ", " ");
            person = person.Replace(" happiness units by sitting next to ", " ");
            person = person.Replace(".", "");

            string[] personValues = person.Split(' ');         

            if(isLose)
            {
                personValues[1] = $"-{personValues[1]}";
            }

            return personValues;
        }

        private List<PersonNode> CreateAllSeatingArrangements(List<string> personNames, List<PersonNode> convertedPersonList)
        {
            List<PersonNode> allSeatingArrangements = new List<PersonNode>();

            foreach (string personName in personNames)
            {
                PersonNode[] startNames = convertedPersonList.Where(r => r.Name == personName).ToArray();

                foreach (PersonNode startName in startNames)
                {
                    PersonNode? oppositeDirectionNode = convertedPersonList.Where(p => p.Name == startName.Neighbour && p.Neighbour == startName.Name).FirstOrDefault();

                    if (oppositeDirectionNode == null)
                    {
                        throw new Exception("Person not found");
                    }

                    AddNextSeat(convertedPersonList, startName, new List<string>() { personName }, personNames.Count);
                }

                allSeatingArrangements.AddRange(startNames);
            }

            return allSeatingArrangements;
        }

        private void AddNextSeat(List<PersonNode> convertedPersonList, PersonNode parent, List<string> reservedNames, int countOfPersons)
        {
            if (reservedNames?.Count > countOfPersons - 1)
            {
                return;
            }

            PersonNode[] nextSeats = convertedPersonList.Where(r => r.Name == parent.Neighbour).ToArray();

            foreach (PersonNode nextSeat in nextSeats)
            {
                if(reservedNames?.Count < countOfPersons - 1 && reservedNames?.Contains(nextSeat.Neighbour) == true)
                {
                    continue;
                }
                else if(reservedNames?.Count == countOfPersons - 1 && nextSeat.Neighbour != parent.FirstPerson.Name)
                {
                    continue;
                } 

                PersonNode next = GetNextPersonNode(parent, nextSeat, convertedPersonList);

                List<string> newreservedNames = new List<string>();

                if (reservedNames != null)
                {
                    newreservedNames.AddRange(reservedNames);
                }

                newreservedNames.Add(nextSeat.Name);
                AddNextSeat(convertedPersonList, next, newreservedNames, countOfPersons);
            }
        }

        private PersonNode GetNextPersonNode(PersonNode parent, PersonNode nextRoute, List<PersonNode> convertedPersonList)
        {
            PersonNode next = new PersonNode(nextRoute.Name, nextRoute.Neighbour, nextRoute.HappyinessUnitsToNeighbour);
            next.PreviousRoute = parent;
            next.FirstPerson = parent.FirstPerson;
            next.HappyinessUnitsToNeighbourFromBeginningToEnd += parent.HappyinessUnitsToNeighbourFromBeginningToEnd;
            next.ItemOrder = parent.ItemOrder + 1;
            parent.NextSeats.Add(next);
            return next;
        }

        private void FillLastPersonNodeList(List<PersonNode> lastPersonNodeList, PersonNode person)
        {
            if (person.NextSeats.Count > 0)
            {
                foreach (PersonNode nextPerson in person.NextSeats)
                {
                    FillLastPersonNodeList(lastPersonNodeList, nextPerson);
                }
            }
            else
            {
                if (person.Neighbour == person.FirstPerson.Name)
                {
                    lastPersonNodeList.Add(person);
                }
            }
        }


        private sealed class PersonNode
        {
            public PersonNode(string name, string neighbour, int happinessUnits)
            {
                Name = name;
                FirstPerson = this;
                Neighbour = neighbour;
                HappyinessUnitsToNeighbour = happinessUnits;
                HappyinessUnitsToNeighbourFromBeginningToEnd = happinessUnits;
            }

            public string Name { get; }

            public string Neighbour { get; }

            public int HappyinessUnitsToNeighbour { get; }

            public int HappyinessUnitsToNeighbourFromBeginningToEnd { get; set; }

            public PersonNode FirstPerson { get; set; }

            public PersonNode? PreviousRoute { get; set; }

            public List<PersonNode> NextSeats { get; set; } = new List<PersonNode>();

            public int ItemOrder { get; set; } = 1;
        }
    }
}
