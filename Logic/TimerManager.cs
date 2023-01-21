using System;
using System.Collections.Generic;
using System.Linq;

namespace RogueDefense
{
    public static class TimerManager
    {
        public static void AddTimer(Action onTimeout, float time)
        {
            timers.Add(new Timer(onTimeout, time));
        }
        public static List<Timer> timers = new List<Timer>();
        public static void Process(float delta)
        {
            timers.ForEach(x =>
            {
                x.timeLeft -= delta;
                if (x.timeLeft <= 0)
                {
                    x.onTimeout();
                }
            });
            timers = timers.Where(x => x.timeLeft > 0f).ToList();
        }
    }
    public struct Timer
    {
        public Timer(Action onTimeout, float timeLeft)
        {
            this.onTimeout = onTimeout;
            this.timeLeft = timeLeft;
        }
        public Action onTimeout;
        public float timeLeft;
    }
}