using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2015
{
    public sealed class Task14 : BaseTask
    {
        /*

        --- Day 14: Reindeer Olympics ---
        This year is the Reindeer Olympics!
        Reindeer can fly at high speeds, but must rest occasionally to recover their energy.
        Santa would like to know which of his reindeer is fastest, and so he has them race.

        Reindeer can only either be flying (always at their top speed) or resting (not moving at all), and always spend whole seconds in either state.

        For example, suppose you have the following Reindeer:
            - Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds.
            - Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds.

        After one second, Comet has gone 14 km, while Dancer has gone 16 km.
        After ten seconds, Comet has gone 140 km, while Dancer has gone 160 km.
        On the eleventh second, Comet begins resting (staying at 140 km), and Dancer continues on for a total distance of 176 km.
        On the 12th second, both reindeer are resting. They continue to rest until the 138th second, when Comet flies for another ten seconds.
        On the 174th second, Dancer flies for another 11 seconds.

        In this example, after the 1000th second, both reindeer are resting, and Comet is in the lead at 1120 km (poor Dancer has only gotten 1056 km by that point).
        So, in this situation, Comet would win (if the race ended at 1000 seconds).

        Task: Given the descriptions of each reindeer (in your puzzle input), after exactly 2503 seconds, what distance has the winning reindeer traveled?
        Solution: 2655

        --- Part Two ---
        Seeing how reindeer move in bursts, Santa decides he's not pleased with the old scoring system.

        Instead, at the end of each second, he awards one point to the reindeer currently in the lead. 
        (If there are multiple reindeer tied for the lead, they each get one point.) 
        He keeps the traditional 2503 second time limit, of course, as doing otherwise would be entirely ridiculous.

        Given the example reindeer from above, after the first second, Dancer is in the lead and gets one point. 
        He stays in the lead until several seconds into Comet's second burst: after the 140th second, Comet pulls into the lead and gets his first point. 
        Of course, since Dancer had been in the lead for the 139 seconds before that, he has accumulated 139 points by the 140th second.

        After the 1000th second, Dancer has accumulated 689 points, while poor Comet, our old champion, only has 312. 
        So, with the new scoring system, Dancer would win (if the race ended at 1000 seconds).

        Task: Again given the descriptions of each reindeer (in your puzzle input), after exactly 2503 seconds, how many points does the winning reindeer have?
        Solution: 1059

        */

        public override string Part1()
        {
            List<string> input = ReadInputLines(@"Year2015\Input\Input14.txt");
            List<Reindeer> reindeers = GetReindeers(input);

            for (int i = 0; i < reindeers.Count; i++)
            {
                reindeers[i].FlySeconds(2503);
            }

            return reindeers.Max(r => r.FlyDistance).ToString();
        }

        public override string Part2()
        {
            List<string> input = ReadInputLines(@"Year2015\Input\Input14.txt");
            List<Reindeer> reindeers = GetReindeers(input);

            for (int i = 0; i < 2503; i++)
            {
                for (int j = 0; j < reindeers.Count; j++)
                {
                    reindeers[j].FlySeconds(1, false);
                }

                SetDistancePoint(reindeers);
            }

            return reindeers.Max(r => r.Points).ToString();
        }

        private void SetDistancePoint(List<Reindeer> reindeers)
        {
            int maxDistance = reindeers.Max(r => r.FlyDistance);

            List<Reindeer> reindeersWithMaxDistance = reindeers.Where(r => r.FlyDistance == maxDistance).ToList();

            for (int j = 0; j < reindeersWithMaxDistance.Count; j++)
            {
                reindeersWithMaxDistance[j].Points++;
            }
        }

        private List<Reindeer> GetReindeers(List<string> input)
        {
            List<Reindeer> reindeers = new List<Reindeer>();

            for (int i = 0; i < input.Count; i++)
            {
                string reindeerLine = input[i];
                reindeerLine = reindeerLine.Replace(" can fly ", " ");
                reindeerLine = reindeerLine.Replace(" km/s for ", " ");
                reindeerLine = reindeerLine.Replace(" seconds, but then must rest for ", " ");
                reindeerLine = reindeerLine.Replace(" seconds.", " ");

                string[] reindeerValues = reindeerLine.Split(' ');

                reindeers.Add(new Reindeer(reindeerValues[0], Convert.ToInt32(reindeerValues[1]), Convert.ToInt32(reindeerValues[2]), Convert.ToInt32(reindeerValues[3])));
            }

            return reindeers;
        }

        private sealed class Reindeer
        {
            public Reindeer(string name, int speed, int flightTime, int restTime)
            {
                Name = name;
                Speed = speed;
                FlightTime = flightTime;
                RestTime = restTime;
            }

            private int _stateCount = 0;
            private bool _isResting = false;

            public string Name { get; }
            public int Speed { get; }
            public int FlightTime { get; }
            public int RestTime { get; }

            public bool IsResting
            {
                get
                {
                    return _isResting;
                }
                private set
                {
                    if (value != IsResting)
                    {
                        _isResting = value;
                        _stateCount = 0;
                    }
                }
            }

            public int FlyDistance { get; private set; } = 0;

            public int Points { get; set; } = 0;

            public void FlySeconds(int seconds, bool resetDistance = true)
            {
                if (resetDistance)
                {
                    FlyDistance = 0;
                    _stateCount = 0;
                }

                for (int second = 0; second < seconds; second++)
                {
                    _stateCount++;

                    if (IsResting)
                    {
                        IsResting = (_stateCount < RestTime);
                    }
                    else
                    {
                        FlyDistance += Speed;
                        IsResting = (_stateCount == FlightTime);
                    }
                }
            }


            public override string ToString()
            {
                return Name;
            }
        }
    }
}
