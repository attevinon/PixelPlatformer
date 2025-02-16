using System;
using DG.Tweening;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class MainMenuWindow : MonoBehaviour
{
    [SerializeField, Range(0,2)] private float _showDelay;
    [SerializeField, Range(0,2)] private float _scaleDuration; 
    [SerializeField, Range(0,2)] private float _fadeDuration;
    [SerializeField, Range(0,2)] private float _moveDuration;
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private float _beyondTheScreenYPosition;
    private float _beyondTheScreenXPosition;
    private bool _isInitialized = false;
    private SettingsWindow _settings;
    
    private Tween _showAnimation;
    private Sequence _hideAnimation;
    private Sequence _moveAnimation;
    
    private void Initialize()
    {
        if (_isInitialized) return;
        _canvas = FindObjectOfType<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
        _beyondTheScreenYPosition = _canvas.pixelRect.height * -1;
        _beyondTheScreenXPosition = _canvas.pixelRect.width * -1;
        _isInitialized = true;
    }
    
    private void Start()
    {
        Show();
    }
    
    private void Show()
    {
        _hideAnimation?.Pause();
        PrepareUIElementsBeforeShow();
        gameObject.SetActive(true);
        if (_showAnimation == null)
        {
            _showAnimation = _rectTransform
                .DOScale(1f, _scaleDuration)
                .From(0f)
                .SetEase(Ease.OutBack)
                .SetDelay(_showDelay);
            _showAnimation.SetAutoKill(false);
            _showAnimation.Play();
            Debug.Log("Shown from " + gameObject.name);
            return;
        }
        _showAnimation.Restart();
    }
    
    private void PrepareUIElementsBeforeShow()
    {
        Initialize();
        _rectTransform.localScale = Vector3.zero;
        _rectTransform.position = _canvas.pixelRect.center;
        _canvasGroup.alpha = 1f;
    }

    private void MoveX(bool show)
    {
        if (_moveAnimation == null)
        {
            _moveAnimation = DOTween.Sequence();
            _moveAnimation
                .Append(_rectTransform
                    .DOMoveX(_beyondTheScreenXPosition, _moveDuration)
                    .SetEase(Ease.InBack))
                .SetLoops(2, LoopType.Yoyo)
                .OnStepComplete(() => _moveAnimation.Pause())
                .AppendInterval(_moveDuration / 1.6f)
                .SetAutoKill(false);
            _moveAnimation.Play();
            return;
        }

        if (show)
        {
            _moveAnimation.Play();
        }
        else
        {
            _moveAnimation.Restart();
        }
    }
    
    public void OnPlayClicked()
    {
        SceneManager.LoadScene(ScenesNames.Level_1.ToString());
    }

    public void OnOptionsClicked()
    {
        MoveX(false);
        if (_settings == null)
        {
            var settingsPrefab = Resources.Load<GameObject>("UI/SettingsWindow");
            GameObject settingsWindow = Instantiate(settingsPrefab, _canvas.transform);
            _settings = settingsWindow.GetComponent<SettingsWindow>();
            _settings.OnHide += OnWindowHide;
        }
        _settings.Show();
    }

    private void OnWindowHide() => MoveX(true);
    
    public void OnExitClicked()
    {
        Hide(OnHideAnimationComplete);
    }
    
    private void Hide(Action callback = null)
    {
        _showAnimation?.Pause();
        _hideAnimation = DOTween.Sequence();
        _hideAnimation
            .Append(_canvasGroup
                .DOFade(0f, _fadeDuration)
                .From(1f)
                .SetEase(Ease.InOutSine))
            .Join(_rectTransform
                .DOMoveY(_beyondTheScreenYPosition, _moveDuration)
                .SetEase(Ease.InBack))
            .OnComplete(() => callback?.Invoke());
        _hideAnimation.SetAutoKill(true);
        _hideAnimation.Play();
    }

    private void OnDisable()
    {
        _settings.OnHide -= OnWindowHide;
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
    
    private void OnHideAnimationComplete()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
