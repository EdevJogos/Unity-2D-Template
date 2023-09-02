using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ETemplate.UI
{
    public class IntroDisplay : Display
    {
        [SerializeField] private Transform _body;
        [SerializeField] private UIButton _creditsButton;
        [SerializeField] private UIButton _settingsButton;

        private List<UIButton> _buttons = new();

        public override void Initialize()
        {
            base.Initialize();
            _buttons.AddRange(_body.GetComponentsInChildren<UIButton>());
            _buttons.ForEach(b => b.transform.localScale = Vector3.zero);
        }

        protected override void TweenInAnimation(Action p_onCompleted)
        {
            _canvas.enabled = true;
            _graphicRaycaster.enabled = false;

            Sequence seq = DOTween.Sequence().SetEase(Ease.Linear);

            for (int __i = 0; __i < _buttons.Count; __i++)
            {
                seq.Join(_buttons[__i].transform.DOScale(1f, 0.25f + (__i * 0.25f)));
            }

            seq.AppendCallback(() =>
            {
                _graphicRaycaster.enabled = true;
                p_onCompleted?.Invoke();
            });
        }
        protected override void TweenOutAnimation(Action p_onCompleted)
        {
            _graphicRaycaster.enabled = false;

            Sequence seq = DOTween.Sequence().SetEase(Ease.Linear);

            for (int __i = 0; __i < _buttons.Count; __i++)
            {
                seq.Join(_buttons[__i].transform.DOScale(0f, 0.25f + (__i * 0.25f)));
            }

            seq.AppendCallback(() =>
            {
                _canvas.enabled = false;
                p_onCompleted?.Invoke();
            });
        }
        protected override void HandleEvents(bool subscribe)
        {
            _settingsButton.onPointerClick.HandleSubscribe(() => Navigation.OnDisplayRequested.Invoke(Displays.SETTINGS), subscribe);
            _creditsButton.onPointerClick.HandleSubscribe(() => Navigation.OnDisplayRequested.Invoke(Displays.CREDITS), subscribe);
        }
    }
}