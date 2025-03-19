using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace PixelCrew.UI.Widgets
{
    public class FilledBarWidget : MonoBehaviour
    {
        [SerializeField] private Image _bar;
        [SerializeField, Range(0,1)] private float _fillingDuration;
        
        private Tween _animation;
        
        public void SetAmount(float amount)
        {
            if (_animation != null && _animation.IsPlaying())
                _animation.Complete();
            
            _animation = _bar.DOFillAmount(amount, _fillingDuration);
            _animation.Play();
        }
        
        private void OnDestroy()
        {
            _animation?.Kill();
        }
    }
}
