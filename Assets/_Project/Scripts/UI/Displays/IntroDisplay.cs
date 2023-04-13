using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class IntroDisplay : Display
{
    [SerializeField] private Transform _body;

    private List<ButtonControl> _buttons = new();

    public override void Initialize()
    {
        base.Initialize();
        _buttons.AddRange(_body.GetComponentsInChildren<ButtonControl>());
        _buttons.ForEach(b => b.transform.localScale = Vector3.zero);
    }

    protected async override void TweenInAnimation(Action p_onCompleted)
    {
        _canvas.enabled = true;

        for (int __i = 0; __i < _buttons.Count; __i++)
        {
            _buttons[__i].transform.DOScale(1f, 0.5f);
            await Task.Delay(100);
        }

        _graphicRaycaster.enabled = true;

        p_onCompleted?.Invoke();
    }

    protected async override void TweenOutAnimation(Action p_onCompleted)
    {
        _graphicRaycaster.enabled = false;

        for (int __i = 0; __i < _buttons.Count; __i++)
        {
            _buttons[__i].transform.DOScale(0f, 0.5f);
            await Task.Delay(100);
        }

        _canvas.enabled = false;

        p_onCompleted?.Invoke();
    }
}