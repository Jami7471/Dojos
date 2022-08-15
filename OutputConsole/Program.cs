// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");




using Analyse;


TimeAnalyse timeAnalyse = new TimeAnalyse();
//timeAnalyse.DoAll("2015.10", typeof(AdventOfCode.Year2015.Task10), 100);


bool end = false;

while (end == false)
{
    Console.WriteLine("Give me a year (2015):");
    string? line = Console.ReadLine();

    if (line?.ToLower() == "cu")
    {
        end = true;
    }

    if (int.TryParse(line, out int year) == false)
    {
        if (line?.ToLower() == "cu")
        {
            end = true;
        }
        else
        {
            Console.WriteLine("No year.");
        }
    }
    else
    {
        if (year == 2015)
        {
            Console.WriteLine("Give me a day  (1-10):");
            line = Console.ReadLine();

            if (int.TryParse(line, out int day))
            {
                switch (day)
                {
                    case 1:
                        timeAnalyse.DoAll("2015.01", typeof(AdventOfCode.Year2015.Task01));
                        break;
                    case 2:
                        timeAnalyse.DoAll("2015.02", typeof(AdventOfCode.Year2015.Task02));
                        break;
                    case 3:
                        timeAnalyse.DoAll("2015.03", typeof(AdventOfCode.Year2015.Task03));
                        break;
                    case 4:
                        timeAnalyse.DoAll("2015.04", typeof(AdventOfCode.Year2015.Task04), 10);
                        break;
                    case 5:
                        timeAnalyse.DoAll("2015.05", typeof(AdventOfCode.Year2015.Task05));
                        break;
                    case 6:
                        timeAnalyse.DoAll("2015.06", typeof(AdventOfCode.Year2015.Task06), 100);
                        break;
                    case 7:
                        timeAnalyse.DoAll("2015.07", typeof(AdventOfCode.Year2015.Task07));
                        break;
                    case 8:
                        timeAnalyse.DoAll("2015.08", typeof(AdventOfCode.Year2015.Task08));
                        break;
                    case 9:
                        timeAnalyse.DoAll("2015.09", typeof(AdventOfCode.Year2015.Task09), 10);
                        break;
                    case 10:
                        timeAnalyse.DoAll("2015.10", typeof(AdventOfCode.Year2015.Task10), 100);
                        break;
                    default:
                        if (line?.ToLower() == "cu")
                        {
                            end = true;
                        }
                        else
                        {
                            Console.WriteLine("Day does not yet exist");
                        }
                        break;
                }
            }
        }
        else
        {
            if (line?.ToLower() == "cu")
            {
                end = true;
            }
            else
            {
                Console.WriteLine("Year does not yet exist");
            }
        }
    }
}

