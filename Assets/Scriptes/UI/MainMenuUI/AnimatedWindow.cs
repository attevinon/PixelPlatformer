using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class AnimatedWindow : MonoBehaviour
{
    [SerializeField, Range(0,2)] private float _moveDuration;
    private Canvas Canvas;
    private RectTransform _rectTransform;
    private Sequence _moveAnimation;
    private float _beyondTheScreenXPosition;
    private bool _isInitialized = false;

    private void Initialize()
    {
        if (_isInitialized) return;
        Canvas = FindObjectOfType<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
        _beyondTheScreenXPosition = Canvas.pixelRect.width * 1.6f;
        _rectTransform.position =
            new Vector3(_beyondTheScreenXPosition, Canvas.pixelRect.center.y, 0);
        _isInitialized = true;
    }

    public void Show()
    {
        Initialize();
        gameObject.SetActive(true);
        MoveX(true);
    }

    public void Hide()
    {
        MoveX(false);
    }

    private void MoveX(bool show)
    {
        if (_moveAnimation == null)
        {
            _moveAnimation = DOTween.Sequence(_moveDuration / 1.6f);
            _moveAnimation
                .PrependInterval(_moveDuration / 1.6f)
                .Append(_rectTransform
                    .DOMoveX(Canvas.pixelRect.center.x, _moveDuration)
                    .SetEase(Ease.OutBack))
                .SetLoops(2, LoopType.Yoyo)
                .OnStepComplete( () => _moveAnimation.Pause())
                .SetAutoKill(false);
            _moveAnimation.Play();
            return;
        }

        if (show)
        {
            _moveAnimation.Restart();
        }
        else
        {
            _moveAnimation.Play();
        }
    }

    protected void KillAnimations()
    {
        _moveAnimation?.Kill();
    }

    private void OnDestroy()
    {
        KillAnimations();
    }
}
