using System;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public class Timer
    {
        public Action onCompleted;

        public bool run;
        public bool permenant;
        public bool loop;
        public float endTime;
        private float _duration;

        public Timer(float duration, Action p_onCompleted, bool p_run = true, bool p_permenant = false, bool p_loop = false)
        {
            run = p_run;
            permenant = p_permenant;
            loop = p_loop;
            _duration = duration;
            onCompleted = p_onCompleted;
        }

        public virtual float GetDuration()
        {
            return _duration;
        }

        public virtual void Run()
        {
            endTime = PlayTime + _duration;
            run = true;
        }

        public virtual void Run(float p_duration)
        {
            endTime = PlayTime + p_duration;
            run = true;
        }

        public void Stop()
        {
            run = false;
        }
    }

    public class RandomTimer : Timer
    {
        public Range range;

        public RandomTimer(Range range, bool run, bool permenant, bool loop, float duration, Action onCompleted) : base(duration, onCompleted, run, permenant, loop)
        {
            this.range = range;
        }

        public override float GetDuration()
        {
            return range.Value;
        }

        public override void Run()
        {
            endTime = PlayTime + range.Value;
            run = true;
        }
    }

    public static float PlayTime;

    private static List<Timer> PlayTimers = new List<Timer>();

    private void Awake()
    {
        PlayTime = Time.time;
    }

    private void Update()
    {
        if (GameCEO.State != GameState.PLAY)
            return;

        PlayTime += Time.deltaTime;

        for (int __i = 0; __i < PlayTimers.Count; __i++)
        {
            if (PlayTimers[__i].run && PlayTime >= PlayTimers[__i].endTime)
            {
                PlayTimers[__i].onCompleted?.Invoke();

                if (PlayTimers[__i].loop)
                {
                    PlayTimers[__i].endTime = PlayTime + PlayTimers[__i].GetDuration();
                }
                else
                {
                    if (PlayTimers[__i].permenant)
                    {
                        PlayTimers[__i].run = false;
                    }
                    else
                    {
                        PlayTimers.Remove(PlayTimers[__i]);
                        __i--;
                    }
                }
            }
        }
    }

    public static Timer AddTimer(Timer p_timer)
    {
        PlayTimers.Add(p_timer);

        return p_timer;
    }

    public static void ResetTimers()
    {
        PlayTimers.Clear();
        PlayTime = Time.time;
    }
}