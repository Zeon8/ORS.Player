using System.Diagnostics;

TimeSpan current = new TimeSpan();
long start = Stopwatch.GetTimestamp();
var stopwatch = Stopwatch.StartNew();
while (true)
{
    current += Stopwatch.GetElapsedTime(start);
    start = Stopwatch.GetTimestamp();

    if (current == TimeSpan.FromSeconds(1))
    {
        Task.Run(() =>
        {
            while (true)
            {
                for (int i = 0; i < 1000000000; i++) { }
            }
        }).Start();
    }

    Console.WriteLine(stopwatch.Elapsed + " " + current);


}

