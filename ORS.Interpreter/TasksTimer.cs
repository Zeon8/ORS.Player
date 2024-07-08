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
    public record struct TimedTask(TimeSpan Time, Action Task, bool Finished = false);

    internal class TasksTimer
    {
        public TimeSpan CurrentTime { get; private set; }
        public int Speed { get; set; } = 1;

        private List<TimedTask> _tasks = new();
        private bool _enabled;

        public void Restart()
        {
            Reset();
            Start();
        }

        public void Start()
        {
            _enabled = true;
            var thread = new Thread(Run);
            thread.IsBackground = true;
            thread.Start();
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
            int count = _tasks.Count;
            Span<TimedTask> tasks = CollectionsMarshal.AsSpan(_tasks);
            long time = Stopwatch.GetTimestamp();
            while (_enabled)
            {
                CurrentTime += Stopwatch.GetElapsedTime(time) * Speed;
                time = Stopwatch.GetTimestamp();

                for (int i = 0; i < count; i++)
                {
                    ref TimedTask item = ref tasks[i];
                    if (_enabled && !item.Finished && CurrentTime >= item.Time)
                    {
                        Task.Run(item.Task);
                        item.Finished = true;
                    }
                }
            }
        }
    }
}
