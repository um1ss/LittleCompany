using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadScreen : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Image _loadPanel;
    [SerializeField] private float _fadeDuration = 0.3f;

    private Tween _fadeTween;
    private bool _animIsPlayind;

    private Color _fadeColor;
    private Color _normalColor;

    private void Start()
    {
        DontDestroyOnLoad(this);
        _normalColor = _loadPanel.color;
        _fadeColor = _normalColor;
        _fadeColor.a = 0;
    }
    public async UniTask Load(Queue<ILoadingOperation> loadingOperations)
    {
        _canvas.enabled = true;
        _loadPanel.color = _normalColor;

        foreach (var operation in loadingOperations)
        {
            await operation.Load();
        }

        EndLoadAnim();
        while (_animIsPlayind)
        {
            await UniTask.Yield();
        }
        _loadPanel.color = _fadeColor;
        _canvas.enabled = false;
    }
    private void EndLoadAnim()
    {
        _fadeTween.Kill();
        _animIsPlayind = true;
        _fadeTween = _loadPanel.DOFade(0, _fadeDuration).SetEase(Ease.Linear).OnComplete(() => _animIsPlayind = false);
    }
}
