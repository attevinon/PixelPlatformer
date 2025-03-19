using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PixelCrew.UI.MainMenu
{
    [RequireComponent(typeof(RectTransform))]
    public class AdditionalWindow : MonoBehaviour
    {
        public event Action OnHide;
        
        [SerializeField, Range(0, 2)] private float _moveDuration; 
        [SerializeField] private Button _backButton;
        
        private Canvas Canvas;
        private RectTransform _rectTransform;
        private Sequence _showAnimation;
        private Sequence _hideAnimation;
        private float _beyondTheScreenXPosition;
        private bool _isInitialized = false;

        private void Initialize()
        {
            if (_isInitialized) return;
            Canvas = FindObjectOfType<Canvas>();
            _rectTransform = GetComponent<RectTransform>();
            _beyondTheScreenXPosition = Canvas.pixelRect.width * 1.6f;
            _rectTransform.position = new Vector3(
                _beyondTheScreenXPosition,
                Canvas.pixelRect.center.y, 0);
            _backButton.onClick.AddListener(Hide);
            _isInitialized = true;
        }

        public void Show()
        {
            Initialize();
            gameObject.SetActive(true); //todo canvas.enable
            if (_showAnimation == null && !_hideAnimation.IsActive())
            {
                _showAnimation = GetMoveAnimation(true);
                _showAnimation.OnPause(() => _backButton.interactable = true);
                _showAnimation.Play();
                return;
            }
            if(_showAnimation.IsPlaying()) return;
            _showAnimation.Restart();
        }

        public void Hide()
        {
            if (_hideAnimation == null && !_hideAnimation.IsActive())
            {
                _hideAnimation = GetMoveAnimation(false);
                _hideAnimation.OnPlay(() => OnHide?.Invoke());
                _hideAnimation.OnPause(() => _backButton.interactable = false);
                _hideAnimation.Play();
                return;
            }
            if(_hideAnimation.IsPlaying()) return;
            _hideAnimation.Restart();
        }

        private Sequence GetMoveAnimation(bool show)
        {
            float delay = show ? _moveDuration / 1.6f : 0f;
            float _targetXPosition = show ? Canvas.pixelRect.center.x : _beyondTheScreenXPosition;
            Ease ease = show ? Ease.OutBack : Ease.InBack;

            Sequence moveAnimation = DOTween.Sequence();
            moveAnimation
                .PrependInterval(delay)
                .Append(_rectTransform
                    .DOMoveX(_targetXPosition, _moveDuration)
                    .SetEase(ease))
                .SetAutoKill(false);
            return moveAnimation;
        }

        protected void KillAnimations()
        {
            _hideAnimation?.Kill();
            _showAnimation?.Kill();
        }

        private void OnDestroy()
        {
            _backButton.onClick.RemoveListener(Hide);
            KillAnimations();
        }
    }
}
