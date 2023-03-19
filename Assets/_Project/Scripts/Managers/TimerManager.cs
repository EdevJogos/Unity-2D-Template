using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : Manager
{
    public static TimerManager Instance;

    public override void Initiate()
    {
        Instance = this;
    }

    public override void Initialize()
    {
        
    }

    public override void Renew()
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