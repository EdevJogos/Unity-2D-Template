using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : Manager
{
    public class Timer
    {
        public float Duration { get; private set; }
        public Action OnCompleted { get; private set; }
        public Action<float> OnUpdated { get; private set; }

        public Timer(float p_duration, Action p_onCompleted, Action<float> p_onUpdated)
        {
            Duration = p_duration;
            OnCompleted = p_onCompleted;
            OnUpdated = p_onUpdated;
        }

        public void Tick()
        {
            Duration = HelpExtensions.ClampMin0(Duration - Time.deltaTime);
        }
    }

    public static TimerManager Instance;

    private static List<Timer> s_Timers = new List<Timer>();

    public override void Initiate()
    {
        Instance = this;
    }

    public override void Initialize()
    {
        
    }

    private void Update()
    {
        for (int __i = 0; __i < s_Timers.Count; __i++)
        {
            s_Timers[__i].Tick();
        }
    }

    public override void Restart()
    {
        
    }

    public static void PlayCoroutine(bool p_play, IEnumerator p_routine)
    {
        if(p_play)
        {
            Instance.StartCoroutine(p_routine);
        }
        else
        {
            Instance.StopCoroutine(p_routine);
        }
    }
}