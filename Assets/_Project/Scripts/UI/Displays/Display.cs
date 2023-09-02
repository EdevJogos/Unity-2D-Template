using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace ETemplate.UI
{
    public class Display : MonoBehaviour
    {
        public UnityEvent OnShowStarted { private set; get; } = new UnityEvent();
        public UnityEvent OnHideStarted { private set; get; } = new UnityEvent();

        public enum AnimationStyles
        {
            DIRECT,
            ANIMATOR,
            TWEEN,
        }

        public Displays ID => id;
        public Navigation Navigation => _navigation;

        [Tooltip("This define which display will be hidden when show is called, ex: if you call show on a type menu all other displays of type menu will be hidden, but the type main will remain." +
        "Menus are always shown over Main and Pop-Ups are shown over menus")]
        [SerializeField] protected DisplayTypes type;
        [SerializeField] protected Displays id;
        [Tooltip("What type of animation it will use to show/hide this display")]
        [SerializeField] protected AnimationStyles animationStyle;

        protected Canvas _canvas;
        protected Navigation _navigation;
        protected GraphicRaycaster _graphicRaycaster;

        #region Behaviour
        public virtual void Initiate()
        {
            _canvas = GetComponent<Canvas>();
            _graphicRaycaster = GetComponent<GraphicRaycaster>();
            _navigation = GetComponent<Navigation>();
            _navigation?.Setup(this);

            HandleEvents(true);
        }
        public virtual void Initialize()
        {

        }

        protected virtual void OnDestroy()
        {
            _navigation?.Dismantle();
            HandleEvents(false);
        }
        #endregion

        public virtual void Show(bool p_show, System.Action p_onCompleted, float p_ratio)
        {
            if (p_show)
            {
                OnShowStarted?.Invoke();
            }
            else
            {
                OnHideStarted?.Invoke();
            }

            switch (animationStyle)
            {
                case AnimationStyles.DIRECT:
                    GetComponent<Canvas>().enabled = p_show;
                    GetComponent<GraphicRaycaster>().enabled = p_show;
                    p_onCompleted?.Invoke();
                    break;
                case AnimationStyles.ANIMATOR:
                    HandleAnimator(p_show, p_onCompleted, p_ratio);
                    break;
                case AnimationStyles.TWEEN:
                    HandleTween(p_show, p_onCompleted);
                    break;
            }
        }

        protected virtual void HandleEvents(bool subscribe) { }

        #region Animator
        private void HandleAnimator(bool p_show, System.Action p_callback, float p_ratio)
        {
            string __animation = p_show ? "In" : "Out";

            if (TryGetComponent(out Animator __animator))
            {
                __animator.SetTrigger(__animation);

                if (p_callback != null) StartCoroutine(RoutineShow(__animation, p_callback, p_ratio));
            }
            else
            {
                Debug.LogError("Requested animator animation style but animator could not be found.");
            }
        }

        private IEnumerator RoutineShow(string p_animation, System.Action p_callback, float p_ratio)
        {
            float __delay = GetClipLength(p_animation) * GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).speed * p_ratio;

            yield return new WaitForSecondsRealtime(__delay);

            p_callback?.Invoke();
        }

        private float GetClipLength(string name)
        {
            AnimationClip[] clips = GetComponent<Animator>().runtimeAnimatorController.animationClips;
            foreach (var item in clips)
            {
                if (item.name == name)
                {
                    return item.length;
                }
            }

            return 0;
        }
        #endregion

        #region Tween
        protected virtual void HandleTween(bool p_show, System.Action p_onCompleted)
        {
            if (p_show) TweenInAnimation(p_onCompleted); else TweenOutAnimation(p_onCompleted);
        }
        /// <summary>
        /// Override this to write the animation that will be executed when ShowDisplay is called.
        /// base will call the onCompleted callback, be careful to not end up calling it twiece.
        /// </summary>
        protected virtual void TweenInAnimation(System.Action p_onCompleted) { p_onCompleted?.Invoke(); }
        /// <summary>
        /// Override this to write the animation that will be executed when HideDisplay is called.
        /// base will call the onCompleted callback, be careful to not end up calling it twiece.
        /// </summary>
        protected virtual void TweenOutAnimation(System.Action p_onCompleted) { p_onCompleted?.Invoke(); }
        #endregion
    }
}