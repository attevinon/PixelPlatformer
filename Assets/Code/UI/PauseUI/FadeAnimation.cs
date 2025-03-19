using System;
using DG.Tweening;
using UnityEngine;

namespace PixelCrew.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeAnimation : MonoBehaviour
    {
        [SerializeField, Range(0,2)] private float _fadeDuration; 
        [SerializeField] private bool _hideOnAwake;
        
        private CanvasGroup _canvasGroup;
        private Sequence _fadeAnimation;
        private bool _isInitialized = false;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if(_isInitialized) return;
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = _hideOnAwake ? 0f : 1f;
            _isInitialized = true;
            _canvasGroup.interactable = !_hideOnAwake;
        }
    
        public void Show()
        {
            Initialize();
            gameObject.SetActive(true);
            Fade(true);
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }
        
    
        public void Hide()
        {
            Fade(false);
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    
        private void Fade(bool show)
        {
            if (_fadeAnimation == null)
            {
                float targetAlpha = _hideOnAwake ? 1f : 0f;
                _fadeAnimation = DOTween.Sequence();
                _fadeAnimation
                    .Append(_canvasGroup
                        .DOFade(targetAlpha, _fadeDuration)
                        .SetEase(Ease.InSine))
                    .SetLoops(2, LoopType.Yoyo)
                    .OnStepComplete(() => _fadeAnimation.Pause())
                    .SetAutoKill(false);
                
                if (_hideOnAwake)
                {
                    _fadeAnimation.PrependInterval(_fadeDuration / 2f);
                }
                else
                {
                    _fadeAnimation.AppendInterval(_fadeDuration / 2f);
                }
                
                _fadeAnimation.Play();
                return;
            }
            
            if (show == _hideOnAwake)
            {
                _fadeAnimation.Restart();
            }
            else
            {
                _fadeAnimation.Play();
            }
        }

        private void OnDestroy()
        {
            _fadeAnimation?.Kill();
        }
    }
}

