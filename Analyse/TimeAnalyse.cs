using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Analyse
{
    public sealed class TimeAnalyse
    {
        private string GiveTimeAndOutput(string title, List<TimeSpan> times, string? result, int countOfLoops)
        {
            if (times == null || times.Count == 0)
            {
                string output = "No time available for logging";
                Console.WriteLine(output);

                using (StreamWriter sw = new StreamWriter(Path.Combine(AppContext.BaseDirectory, $"{nameof(TimeAnalyse)}_Output.txt"), true))
                {
                    sw.WriteLine(output);
                    sw.Flush();
                }

                return output;
            }
            else
            {
                TimeSpan maxTime = times.Max();
                TimeSpan minTime = times.Min();

                double averageMilliseconds = 0;

                foreach (TimeSpan time in times)
                {
                    averageMilliseconds += time.TotalMilliseconds;
                }

                averageMilliseconds /= countOfLoops;

                return LogTime(title, result, maxTime, minTime, averageMilliseconds, countOfLoops);
            }
        }

        private string LogTime(string title, string? result, TimeSpan maxTime, TimeSpan minTime, double averageMilliseconds, int countOfLoops)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{title}\t=>\t{result}");
            sb.AppendLine($"Loops:\t\t\t{countOfLoops}");
            sb.AppendLine($"Max-Time:\t\t{maxTime}");
            sb.AppendLine($"Min-Time:\t\t{minTime}");

            string avgTime = DetermineTime(averageMilliseconds);
            sb.AppendLine($"Avg-Time:\t\t{avgTime}");

            //Console.WriteLine(sb.ToString());

            using (StreamWriter sw = new StreamWriter(Path.Combine(AppContext.BaseDirectory, $"{nameof(TimeAnalyse)}_Output.txt"), true))
            {
                sw.WriteLine($"{title} -> {result}");
                sw.WriteLine($"Loops\tMax-Time\tMin-Time\tAvg-Time");
                sw.WriteLine($"{countOfLoops}\t{maxTime}\t{minTime}\t{avgTime}");
                sw.Flush();
            }

            return sb.ToString();
        }

        private string DetermineTime(double averageMilliseconds)
        {
            if (averageMilliseconds > 1000)
            {
                double averageSeconds = averageMilliseconds / 1000;

                if (averageSeconds > 60)
                {
                    double averageMinutes = averageSeconds / 60;

                    if (averageMinutes > 60)
                    {
                        double averageHours = averageMinutes / 60;

                        if (averageHours > 24)
                        {
                            double averageDays = averageHours / 24;
                            return $"{averageDays} d";
                        }
                        else
                        {
                            return $"{averageHours} h";
                        }
                    }
                    else
                    {
                        return $"{averageMinutes} m";
                    }
                }
                else
                {
                    return $"{averageSeconds} s";
                }
            }
            else
            {
                return $"{averageMilliseconds} ms";
            }
        }

        public void DoMethod(string title, Type type, string methodName, int countOfLoops = 1000)
        {
            Stopwatch timer = new Stopwatch();
            List<TimeSpan> times = new List<TimeSpan>();
            string? result = string.Empty;

            for (int i = 0; i <= countOfLoops; i++)
            {
                timer.Restart();

                object? instance = Activator.CreateInstance(type);
                MethodInfo? method = instance?.GetType()?.GetMethod(methodName);
                result = method?.Invoke(instance, null)?.ToString();

                timer.Stop();

                /* First Stopwatch: Higher > Rest
                 * The answer is that Stopwatch does not compensate for "background noise" activity in .NET, such as JITing. 
                 * Therefore the first time you run your method, .NET JIT's it first. The time it takes to do this is added to the time of the execution. 
                 * Equally, other factors will also cause the execution time to vary.
                 */
                if (i > 0)
                {
                    TimeSpan time = new TimeSpan(timer.ElapsedTicks);
                    times.Add(time);
                    OnLoopOfMethodFinished(title, type, methodName, i, time.ToString(), result);
                }
            }

            string resultText = GiveTimeAndOutput(title, times, result, countOfLoops);
            OnAllLoopOfMethodFinished(title, type, methodName, countOfLoops, string.Empty, resultText);
        }

        public void DoMethods(string title, Type type, string[] methodNames, int countOfLoops = 1000)
        {
            for (int i = 0; i < methodNames.Length; i++)
            {
                DoMethod($"{title}.{methodNames[i]}", type, methodNames[i], countOfLoops);
            }
        }

        public void DoAllPublicMethods(string title, Type type, int countOfLoops = 1000)
        {
            string[] methodNamesOfObject = typeof(object).GetMethods(BindingFlags.Instance | BindingFlags.Public).Select(m => m.Name).ToArray();

            MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(m => methodNamesOfObject.Contains(m.Name) == false)
                .ToArray();

            for (int i = 0; i < methods.Length; i++)
            {
                DoMethod($"{title}.{methods[i].Name}", type, methods[i].Name, countOfLoops);
            }
        }

        public event TimeAnalyseEventHandler? AllLoopOfMethodFinished;
        public event TimeAnalyseEventHandler? LoopOfMethodFinished;

        private void OnAllLoopOfMethodFinished(string title, Type type, string methodName, int countOfLoops, string executionTime, string resultText)
        {
            AllLoopOfMethodFinished?.Invoke(this, new TimeAnalyseEventArgs(title, type, methodName, countOfLoops, executionTime, resultText));
        }

        private void OnLoopOfMethodFinished(string title, Type type, string methodName, int countOfLoops, string executionTime, string? resultText)
        {
            if(string.IsNullOrWhiteSpace(resultText))
            {
                resultText = string.Empty;
            }

            LoopOfMethodFinished?.Invoke(this, new TimeAnalyseEventArgs(title, type, methodName, countOfLoops, executionTime, resultText));
        }
    }
}