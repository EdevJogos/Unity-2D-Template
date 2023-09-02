using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace ETemplate.Manager
{
    public class StageManager : Manager
    {
        public event System.Action<float> onStartCountdownUpdated;
        public event System.Action onStartCountdownFinished;

        public float StartCountdown { get; set; } = 3;

        public override void Initialize()
        {

        }

        public override void Initiate()
        {

        }

        private void Update()
        {

        }

        public void RequestMatchStart()
        {
            StartCountdown = 3;
            DOTween.To(() => StartCountdown, x => UpdateStartCountdown(x), 0f, 3f).SetEase(Ease.Linear).OnComplete(() => onStartCountdownFinished.Invoke());
        }

        private void UpdateStartCountdown(float p_time)
        {
            StartCountdown = p_time;
            onStartCountdownUpdated?.Invoke(Mathf.CeilToInt(StartCountdown));
        }
    }
}