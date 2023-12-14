using Avalonia.Threading;
using DynamicData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ORS.Interpreter
{
    public record struct TimedTask(TimeSpan Time, Action Task);

    internal class TasksTimer
    {
        public TimeSpan CurrentTime { get; private set; }

        private List<TimedTask> _tasks = new();
        private readonly TimeSpan _updateTime = TimeSpan.FromMilliseconds(10);
        private readonly Stopwatch _stopwatch = new();
        private bool _enabled;
        private TimeSpan _previous;

        public int Speed { get; set; } = 1;

        public void Restart()
        {
            Reset();
            Start();
        }

        public void Start()
        {
            _enabled = true;
            Task.Run(Run);
        }

        public void Stop()
        {
            _enabled = false;
        }

        public void Reset()
        {
            CurrentTime = new TimeSpan();
            _tasks.Clear();
        }

        public void SetEnabled(bool enabled)
        {
            if (enabled)
                Start();
            else
                Stop();
        }

        public void AddTask(TimeSpan startTime, Action task)
        {
            _tasks.Add(new TimedTask(startTime, task));
        }

        private void Run()
        {
            long start = Stopwatch.GetTimestamp();
            int count = _tasks.Count;
            Span<TimedTask> tasks = CollectionsMarshal.AsSpan(_tasks);
            _stopwatch.Start();
            while (_enabled)
            {
                CurrentTime += Stopwatch.GetElapsedTime(start) * Speed;
                start = Stopwatch.GetTimestamp();

                for (int i = 0; i < count; i++)
                {
                    TimedTask item = tasks[i];
                    if (item.Task is not null && CurrentTime >= item.Time)
                    {
                        item.Task.Invoke();
                        tasks[i].Task = null!;
                    }
                }
            }
        }
    }
}
