using System;
using DG.Tweening;
using UnityEngine;

namespace PixelCrew.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(RectTransform))]
    public class InGameMenu : MonoBehaviour
    {
        [SerializeField, Range(0, 2)] private float _animationDuration;
        [SerializeField] private CanvasGroup _backgroundCanvasGroup;
        [SerializeField] private Canvas _canvas;
        
        private RectTransform _rectTransform;
        private CanvasGroup _menuCanvasGroup;
        private bool _isInitialized = false;
        private Sequence _showAnimation;
        private Sequence _hideAnimation;

        private void Awake()
        {
            _canvas.enabled = false;
        }

        private void Initialize()
        {
            if (_isInitialized) return;
            _menuCanvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
            _isInitialized = true;
        }

        public void Show()
        {
            _hideAnimation?.Pause();
            PrepareUIElementsBeforeShow();
            gameObject.SetActive(true);
            _canvas.enabled = true;
            if (_showAnimation == null)
            {
                _showAnimation = DOTween.Sequence();
                _showAnimation
                    .Append(_backgroundCanvasGroup
                        .DOFade(1f, _animationDuration)
                        .SetEase(Ease.InSine))
                    .Join(_rectTransform
                        .DOScale(1f, _animationDuration)
                        .From(0f)
                        .SetEase(Ease.OutBack))
                    .Join(_menuCanvasGroup
                        .DOFade(1f, _animationDuration)
                        .From(0f)
                        .SetEase(Ease.InCubic));

                _showAnimation.SetAutoKill(false);
                _showAnimation.Play();
                return;
            }

            _showAnimation.Restart();
        }

        private void PrepareUIElementsBeforeShow()
        {
            Initialize();
            _rectTransform.localScale = Vector3.zero;
            _menuCanvasGroup.alpha = 1f;
        }

        public void Hide()
        {
            _showAnimation?.Pause();
            if (_hideAnimation == null)
            {
                _hideAnimation = DOTween.Sequence();
                _hideAnimation
                    .Append(_rectTransform
                        .DOScale(0f, _animationDuration)
                        .SetEase(Ease.InBack))
                    .Join(_menuCanvasGroup
                        .DOFade(0f, _animationDuration)
                        .From(1f)
                        .SetEase(Ease.InExpo))
                    .Join(_backgroundCanvasGroup
                        .DOFade(0f, _animationDuration)
                        .SetEase(Ease.InCubic))
                    .OnComplete(()=> _canvas.enabled = false);
                _hideAnimation.SetAutoKill(false);
                _hideAnimation.Play();
                return;
            }
            _hideAnimation.Restart();
        }

        private void OnDestroy()
        {
            KillAnimations();
        }

        private void KillAnimations()
        {
            _showAnimation?.Kill();
            _hideAnimation?.Kill();
        }
    }
}
