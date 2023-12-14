//using OverflowScript.Parser;

//var file = File.ReadAllText("01/1.ORS");
//var reader = new StringReader(file);
//var parser = new Parser(reader);
//var list = parser.ParseCommands().ToList();
//Console.ReadKey();

using System;
using System.Diagnostics;

var stopWatch = new Stopwatch();
var timer = new Timer(TimerCallback, null, 0, 10);
void TimerCallback(object state)
{
    stopWatch.Stop();
    Console.WriteLine(stopWatch.Elapsed);
    stopWatch.Restart();
}

while (true) { }
