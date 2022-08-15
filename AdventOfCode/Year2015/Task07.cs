using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Year2015
{
    public sealed class Task07 : BaseTask
    {
        /*
        --- Day 7: Some Assembly Required ---
        This year, Santa brought little Bobby Tables a set of wires and bitwise logic gates! Unfortunately, little Bobby is a little under the recommended age range, and he needs help assembling the circuit.

        Each wire has an identifier (some lowercase letters) and can carry a 16-bit signal (a number from 0 to 65535). 
        A signal is provided to each wire by a gate, another wire, or some specific value. Each wire can only get a signal from one source, but can provide its signal to multiple destinations. 
        A gate provides no signal until all of its inputs have a signal.

        The included instructions booklet describes how to connect the parts together: 
            x AND y -> z means to connect wires x and y to an AND gate, and then connect its output to wire z.

        For example:
            - 123 -> x means that the signal 123 is provided to wire x.
            - x AND y -> z means that the bitwise AND of wire x and wire y is provided to wire z.
            - p LSHIFT 2 -> q means that the value from wire p is left-shifted by 2 and then provided to wire q.
            - NOT e -> f means that the bitwise complement of the value from wire e is provided to wire f.
        
        Other possible gates include OR (bitwise OR) and RSHIFT (right-shift). 
        If, for some reason, you'd like to emulate the circuit instead, almost all programming languages (for example, C, JavaScript, or Python) provide operators for these gates.

        For example, here is a simple circuit:
            123 -> x
            456 -> y
            x AND y -> d
            x OR y -> e
            x LSHIFT 2 -> f
            y RSHIFT 2 -> g
            NOT x -> h
            NOT y -> i
        
        After it is run, these are the signals on the wires:

        d: 72
        e: 507
        f: 492
        g: 114
        h: 65412
        i: 65079
        x: 123
        y: 456

        Task: In little Bobby's kit's instructions booklet (provided as your puzzle input), what signal is ultimately provided to wire a?
        Solution:  46065

        --- Part Two ---
        Now, take the signal you got on wire a, override wire b to that signal, and reset the other wires (including wire a). 
        
        Task: What new signal is ultimately provided to wire a?
        Solution: 14134

        */

        public override string Part1()
        {
            List<string> input = ReadInputLines(@"Year2015\Input\Input07.txt");

            Dictionary<string, ushort> wires = new Dictionary<string, ushort>();
            Dictionary<string, Instruction> instructions = new Dictionary<string, Instruction>();

            foreach (string instruction in input)
            {
                InstructionOption instructionOption = AnalyseInstruction(instruction, out string[]? values);

                if (instructionOption == InstructionOption.Unknown || values == null)
                {
                    Console.WriteLine($"{System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType?.FullName}.{System.Reflection.MethodBase.GetCurrentMethod()?.Name} - Instruction/Values-Error {instruction}");
                    return "NO RESULT";
                }

                instructions.Add(values[^1], new Instruction(instructionOption, values));
            }

            return GetWireSignal("a", wires, instructions).ToString();
        }

        public override string Part2()
        {
            List<string> input = ReadInputLines(@"Year2015\Input\Input07.txt");

            Dictionary<string, ushort> wires = new Dictionary<string, ushort>();
            Dictionary<string, Instruction> instructions = new Dictionary<string, Instruction>();

            foreach (string instruction in input)
            {
                InstructionOption instructionOption = AnalyseInstruction(instruction, out string[]? values);

                if (instructionOption == InstructionOption.Unknown || values == null)
                {
                    Console.WriteLine($"{System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType?.FullName}.{System.Reflection.MethodBase.GetCurrentMethod()?.Name} - Instruction/Values-Error {instruction}");
                    return "NO RESULT";
                }

                instructions.Add(values[^1], new Instruction(instructionOption, values));
            }

            ushort a1 = GetWireSignal("a", wires, instructions);
            wires.Clear();
            wires.Add("b", a1);

            return GetWireSignal("a", wires, instructions).ToString();
        }

        private enum InstructionOption
        {
            Unknown = -1,
            MOVE = 0,
            AND = 1,
            OR = 2,
            Complement = 3,
            LSHIFT = 4,
            RSHIFT = 5
        }

        private sealed class Instruction
        {
            public Instruction(InstructionOption instructionOption, string[] values)
            {
                InstructionOption = instructionOption;
                Values = values;
            }

            public InstructionOption InstructionOption { get; }

            public string[] Values { get; }
        }

        private InstructionOption AnalyseInstruction(string instruction, out string[]? values)
        {
            InstructionOption instructionOption = InstructionOption.Unknown;

            if (instruction.Contains("AND"))
            {
                instructionOption = InstructionOption.AND;
                instruction = instruction.Replace(" AND ", " ");
            }
            else if (instruction.Contains("OR"))
            {
                instructionOption = InstructionOption.OR;
                instruction = instruction.Replace(" OR ", " ");
            }
            else if (instruction.Contains("NOT"))
            {
                instructionOption = InstructionOption.Complement;
                instruction = instruction.Replace("NOT ", "");
            }
            else if (instruction.Contains("LSHIFT"))
            {
                instructionOption = InstructionOption.LSHIFT;
                instruction = instruction.Replace(" LSHIFT ", " ");
            }
            else if (instruction.Contains("RSHIFT"))
            {
                instructionOption = InstructionOption.RSHIFT;
                instruction = instruction.Replace(" RSHIFT ", " ");
            }
            else if (instruction.Contains("->"))
            {
                instructionOption = InstructionOption.MOVE;
            }
            else
            {
                values = null;
                return instructionOption;
            }

            instruction = instruction.Replace(" -> ", " ");
            values = instruction.Split(' ');

            return instructionOption;
        }

        private ushort GetWireSignal(string key, Dictionary<string, ushort> wires, Dictionary<string, Instruction> instructions)
        {
            if (char.IsDigit(key[0]))
            {
                return ushort.Parse(key);
            }

            if (wires.ContainsKey(key))
            {
                return wires[key];
            }

            if (instructions[key].InstructionOption == InstructionOption.MOVE)
            {
                if (char.IsDigit(instructions[key].Values[0][0]))
                {
                    wires.Add(key, ushort.Parse(instructions[key].Values[0]));
                    return ushort.Parse(instructions[key].Values[0]);
                }
                else
                {
                    ushort result = GetWireSignal(instructions[key].Values[0], wires, instructions);
                    wires.Add(key, result);
                    return result;
                }
            }
            else
            {
                ushort[] values = new ushort[1];

                switch (instructions[key].InstructionOption)
                {
                    case InstructionOption.AND:
                    case InstructionOption.OR:
                    case InstructionOption.LSHIFT:
                    case InstructionOption.RSHIFT:
                        values = new ushort[2] { GetWireSignal(instructions[key].Values[0], wires, instructions),
                            GetWireSignal(instructions[key].Values[1], wires, instructions)};
                        break;
                    case InstructionOption.Complement:
                        values = new ushort[1] { GetWireSignal(instructions[key].Values[0], wires, instructions) };
                        break;
                }

                ushort result = DoInstruction(instructions[key].InstructionOption, values);
                wires.Add(key, result);
                return result;
            }
        }

        private ushort DoInstruction(InstructionOption instructionOption, ushort[] values)
        {
            switch (instructionOption)
            {
                case InstructionOption.AND:
                    return (ushort)(values[0] & values[1]);
                case InstructionOption.OR:
                    return (ushort)(values[0] | values[1]);
                case InstructionOption.Complement:
                    return (ushort)(~values[0]);
                case InstructionOption.RSHIFT:
                    return (ushort)(values[0] >> values[1]);
                case InstructionOption.LSHIFT:
                    return (ushort)(values[0] << values[1]);
                default:
                    return 0;
            }
        }
    }
}
