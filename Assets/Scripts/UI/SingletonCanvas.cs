using System;
using DG.Tweening;
using UnityEngine;

namespace UI.Canvas
{
    public abstract class SingletonCanvas<T> : MonoBehaviour where T : SingletonCanvas<T>
    {
        private static T _instance;

        public float FadeInTime = 0.5f;
        public float FadeOutTime = 0.5f;
        public Vector3 FadeInStartScale = Vector3.one;
        public Vector3 FadeOutEndScale = Vector3.one;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                }
                return _instance;
            }
        }

        public RectTransform Rect;

        //public UnityEngine.Canvas Canvas;
        public CanvasGroup CanvasGroup;

        public virtual void Show()
        {
            Rect ??= GetComponent<RectTransform>();
            Rect.DOScale(Vector3.one, FadeInTime).From(FadeInStartScale);
            CanvasGroup.DOFade(1f,FadeInTime);
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
        }

        public virtual void Hide(bool isInstant = false)
        {
            Rect ??= GetComponent<RectTransform>();
            if (isInstant)
            {
                CanvasGroup.alpha = 0;
            }
            else
            {
                Rect.DOScale(FadeOutEndScale, FadeOutTime);
                CanvasGroup.DOFade(0f, FadeOutTime);
            }
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
        }

        private void OnDestroy()
        {
            _instance = null;
        }
    }
}