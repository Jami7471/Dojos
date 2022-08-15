using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2015
{
    public sealed class Task06 : BaseTask
    {
        /*
        --- Day 6: Probably a Fire Hazard ---
        Because your neighbors keep defeating you in the holiday house decorating contest year after year, you've decided to deploy one million lights in a 1000x1000 grid.

        Furthermore, because you've been especially nice this year, Santa has mailed you instructions on how to display the ideal lighting configuration.

        Lights in your grid are numbered from 0 to 999 in each direction; the lights at each corner are at 0,0, 0,999, 999,999, and 999,0. 
        The instructions include whether to turn on, turn off, or toggle various inclusive ranges given as coordinate pairs. 
        Each coordinate pair represents opposite corners of a rectangle, inclusive; a coordinate pair like 0,0 through 2,2 therefore refers to 9 lights in a 3x3 square. The lights all start turned off.

        To defeat your neighbors this year, all you have to do is set up your lights by doing the instructions Santa sent you in order.

        For example: 
            - turn on 0,0 through 999,999 would turn on (or leave on) every light.
            - toggle 0,0 through 999,0 would toggle the first line of 1000 lights, turning off the ones that were on, and turning on the ones that were off.
            - turn off 499,499 through 500,500 would turn off (or leave off) the middle four lights.

        Task: After following the instructions, how many lights are lit?
        Solution: 377891

        --- Part Two ---
        You just finish implementing your winning light pattern when you realize you mistranslated Santa's message from Ancient Nordic Elvish.

        The light grid you bought actually has individual brightness controls; each light can have a brightness of zero or more. 
        The lights all start at zero.

        The phrase turn on actually means that you should increase the brightness of those lights by 1.

        The phrase turn off actually means that you should decrease the brightness of those lights by 1, to a minimum of zero.

        The phrase toggle actually means that you should increase the brightness of those lights by 2.

        Task: What is the total brightness of all lights combined after following Santa's instructions?
        Solution: 14110788

        For example:
            - turn on 0,0 through 0,0 would increase the total brightness by 1.
            - toggle 0,0 through 999,999 would increase the total brightness by 2000000.
        */

        public override string Part1()
        {
            List<string> input = ReadInputLines(@"Year2015\Input\Input06.txt");

            int[,] lights = new int[1000, 1000];

            foreach (string instruction in input)
            {
                InstructionOption instructionOption = DeterminePoints(instruction, out Point startPoint, out Point endPoint);

                switch (instructionOption)
                {
                    case InstructionOption.TurnOFF:
                        TurnOffLights(lights, startPoint, endPoint);
                        break;
                    case InstructionOption.TurnOn:
                        TurnOnLights(lights, startPoint, endPoint);
                        break;
                    case InstructionOption.Toggle:
                        ToogleLights(lights, startPoint, endPoint);
                        break;
                }
            }

            return CountLightsWhichAreOn(lights).ToString();
        }

        public override string Part2()
        {
            List<string> input = ReadInputLines(@"Year2015\Input\Input06.txt");

            int[,] lights = new int[1000, 1000];

            foreach (string instruction in input)
            {
                InstructionOption instructionOption = DeterminePoints(instruction, out Point startPoint, out Point endPoint);

                switch (instructionOption)
                {
                    case InstructionOption.TurnOFF:
                        TurnOffLights(lights, startPoint, endPoint, true);
                        break;
                    case InstructionOption.TurnOn:
                        TurnOnLights(lights, startPoint, endPoint, true);
                        break;
                    case InstructionOption.Toggle:
                        ToogleLights(lights, startPoint, endPoint, true);
                        break;
                }
            }

            return CountLightsWhichAreOn(lights).ToString();
        }

        private enum InstructionOption
        {
            TurnOFF = 0,
            TurnOn = 1,
            Toggle = 2
        }
        private InstructionOption DeterminePoints(string instruction, out Point startPoint, out Point endPoint)
        {
            InstructionOption instructionOption;

            if (instruction.Contains("turn on"))
            {
                instructionOption = InstructionOption.TurnOn;
                instruction = instruction.Replace("turn on ", "");
            }
            else if (instruction.Contains("turn off"))
            {
                instructionOption = InstructionOption.TurnOFF;
                instruction = instruction.Replace("turn off ", "");
            }
            else if (instruction.Contains("toggle"))
            {
                instructionOption = InstructionOption.Toggle;
                instruction = instruction.Replace("toggle ", "");
            }
            else
            {
                throw new InvalidOperationException("No matching instruction found");
            }

            instruction = instruction.Replace(" through ", ";");
            string[] points = instruction.Split(';');

            startPoint = GetPoint(points[0]);
            endPoint = GetPoint(points[1]);

            return instructionOption;
        }

        private Point GetPoint(string pointValue)
        {
            string[] pointValues = pointValue.Split(',');

            if (int.TryParse(pointValues[0], out int xPos) && int.TryParse(pointValues[1], out int yPos))
            {
                return new Point(xPos, yPos);
            }
            else
            {
                throw new ArgumentException($"'{pointValue}' is not a valid point");
            }
        }

        private void ToogleLights(int[,] lights, Point startPoint, Point endPoint, bool useBrightnessSwitch = false)
        {
            for (int xPos = startPoint.X; xPos <= endPoint.X; xPos++)
            {
                for (int yPos = startPoint.Y; yPos <= endPoint.Y; yPos++)
                {
                    if (useBrightnessSwitch)
                    {
                        lights[xPos, yPos] += 2;
                    }
                    else
                    {
                        if (lights[xPos, yPos] == 1)
                        {
                            lights[xPos, yPos] = 0;
                        }
                        else
                        {
                            lights[xPos, yPos] = 1;
                        }
                    }
                }
            }
        }

        private void TurnOnLights(int[,] lights, Point startPoint, Point endPoint, bool useBrightnessSwitch = false)
        {
            for (int xPos = startPoint.X; xPos <= endPoint.X; xPos++)
            {
                for (int yPos = startPoint.Y; yPos <= endPoint.Y; yPos++)
                {
                    lights[xPos, yPos] = (useBrightnessSwitch) ? lights[xPos, yPos] + 1 : 1;
                }
            }
        }

        private void TurnOffLights(int[,] lights, Point startPoint, Point endPoint, bool useBrightnessSwitch = false)
        {
            for (int xPos = startPoint.X; xPos <= endPoint.X; xPos++)
            {
                for (int yPos = startPoint.Y; yPos <= endPoint.Y; yPos++)
                {
                    if (useBrightnessSwitch)
                    {
                        if (lights[xPos, yPos] > 0)
                        {
                            lights[xPos, yPos]--;
                        }
                    }
                    else
                    {
                        lights[xPos, yPos] = 0;
                    }
                }
            }
        }

        private int CountLightsWhichAreOn(int[,] lights)
        {
            int number0fLightsTurnedOn = 0;
            int xMax = lights.GetLength(0);
            int yMax = lights.GetLength(1);

            for (int xPos = 0; xPos < xMax; xPos++)
            {
                for (int yPos = 0; yPos < yMax; yPos++)
                {
                    number0fLightsTurnedOn += lights[xPos, yPos];
                }
            }

            return number0fLightsTurnedOn;
        }
    }
}
