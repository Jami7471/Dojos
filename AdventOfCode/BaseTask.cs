using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class BaseTask
    {
        internal List<T> GetListOf<T>(string inputFilePart)
        {
            List<T> result = new();
            List<string> lines = ReadInputLines(inputFilePart);

            foreach (string line in lines)
            {
                T? item = (T?)Activator.CreateInstance(typeof(T), line);

                if (item != null)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        internal string ReadInputToEnd(string inputFilePart)
        {
            string txtFile = Path.Combine(AppContext.BaseDirectory, inputFilePart);
            using StreamReader reader = new(txtFile);
            return reader.ReadToEnd();
        }

        internal List<string> ReadInputLines(string inputFilePart)
        {
            List<string> inputTxt = new();

            string txtFile = Path.Combine(AppContext.BaseDirectory, inputFilePart);

            using (StreamReader reader = new(txtFile))
            {
                string? line = string.Empty;

                while ((line = reader.ReadLine()) != null)
                {
                    inputTxt.Add(line);
                }
            }

            return inputTxt;
        }

        public virtual string Part1()
        {
            return "NO RESULT";
        }

        public virtual string Part2()
        {
            return "NO RESULT";
        }
    }
}
