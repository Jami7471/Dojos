using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyse
{
    public delegate void TimeAnalyseEventHandler(object sender, TimeAnalyseEventArgs e);

    public class TimeAnalyseEventArgs : EventArgs
    {
        public TimeAnalyseEventArgs(string title, Type type, string methodName, int countOfLoops, string executionTime, string resultText)
        {
            Title = title;
            Type = type;
            MethodName = methodName;
            CountOfLoops = countOfLoops;
            ExecutionTime = executionTime;
            ResultText = resultText;
        }

        public string Title { get; }

        public Type Type { get; }

        public string MethodName { get; }

        public int CountOfLoops { get; }

        public string ExecutionTime { get; }

        public string ResultText { get; }
    }
}
