using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Analyse
{
    public class TimeAnalyse
    {
        private void GiveTimeAndOutput(string resulText, List<TimeSpan> times, string? result, int countOfLoops)
        {
            TimeSpan maxTime = times.Max();
            TimeSpan minTime = times.Min();

            double averageMilliseconds = 0;

            foreach (TimeSpan time in times)
            {
                averageMilliseconds += time.TotalMilliseconds;
            }

            averageMilliseconds /= countOfLoops;

            LogTime(resulText, result, maxTime, minTime, averageMilliseconds, countOfLoops);
        }

        private void LogTime(string resulText, string? result, TimeSpan maxTime, TimeSpan minTime, double averageMilliseconds, int countOfLoops)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{resulText}\t=>\t{result}");
            sb.AppendLine($"Loops:\t\t\t{countOfLoops}");
            sb.AppendLine($"Max-Time:\t\t{maxTime}");
            sb.AppendLine($"Min-Time:\t\t{minTime}");

            string avgTime = DetermineTime(averageMilliseconds);
            sb.AppendLine($"Avg-Time:\t\t{avgTime}");

            Console.WriteLine(sb.ToString());

            using (StreamWriter sw = new StreamWriter(Path.Combine(AppContext.BaseDirectory, $"{nameof(TimeAnalyse)}_Output.txt"), true))
            {
                sw.WriteLine($"{resulText} -> {result}");
                sw.WriteLine($"Loops\tMax-Time\tMin-Time\tAvg-Time");
                sw.WriteLine($"{countOfLoops}\t{maxTime}\t{minTime}\t{avgTime}");
                sw.Flush();
            }
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

        public void Do(string resulText, Type type, string methodName, int countOfLoops = 1000)
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
                    times.Add(new TimeSpan(timer.ElapsedTicks));
                }
            }

            GiveTimeAndOutput(resulText, times, result, countOfLoops);
        }

        public void Do(string resulText, Type type, string[] methodNames, int countOfLoops = 1000)
        {
            for (int i = 0; i < methodNames.Length; i++)
            {
                Do($"{resulText}.{methodNames[i]}", type, methodNames[i], countOfLoops);
            }
        }

        public void DoAll(string resulText, Type type, int countOfLoops = 1000)
        {
            string[] methodNamesOfObject = typeof(object).GetMethods(BindingFlags.Instance | BindingFlags.Public).Select(m => m.Name).ToArray();

            MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(m => methodNamesOfObject.Contains(m.Name) == false)
                .ToArray();

            for (int i = 0; i < methods.Length; i++)
            {
                Do($"{resulText}.{methods[i].Name}", type, methods[i].Name, countOfLoops);
            }
        }
    }
}