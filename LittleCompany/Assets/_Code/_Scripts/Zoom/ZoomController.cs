using System.Collections;
using UnityEngine;

public class ZoomController : MonoBehaviour
{
    private bool ShouldZoom => Input.GetKeyDown(_zoomKey);
    private bool CancelZoom => Input.GetKeyUp(_zoomKey);
    
    [Header("Bingding KeyCode")] [SerializeField]
    private KeyCode _zoomKey = KeyCode.Z;
    
    [Header("Zoom Parameters")] [SerializeField]
    private float _timeToZoom = 0.3f;
    [SerializeField] private float _zoomFOV = 30f;
    private float _defaultFOV;
    private Coroutine _zoomRoutine;

    private Camera _playerCamera;

    private void Awake()
    {
        _playerCamera = GetComponentInChildren<Camera>();
        _defaultFOV = _playerCamera.fieldOfView;
    }

    private void Update()
    {
        HandleZoom();
    }

    private void HandleZoom()
    {
        if (ShouldZoom)
        {
            if (_zoomRoutine != null)
            {
                StopCoroutine(_zoomRoutine);
                _zoomRoutine = null;
            }

            _zoomRoutine = StartCoroutine(ToogleZoom(true));
        }

        if (CancelZoom)
        {
            if (_zoomRoutine != null)
            {
                StopCoroutine(_zoomRoutine);
                _zoomRoutine = null;
            }

            _zoomRoutine = StartCoroutine(ToogleZoom(false));
        }
    }

    private IEnumerator ToogleZoom(bool isEnter)
    {
        float targetFOV = isEnter ? _zoomFOV : _defaultFOV;
        float startingFOV = _playerCamera.fieldOfView;
        float timeElapsed = 0;

        while (timeElapsed < _timeToZoom)
        {
            _playerCamera.fieldOfView = Mathf.Lerp(startingFOV, targetFOV, timeElapsed / _timeToZoom);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        _playerCamera.fieldOfView = targetFOV;
        _zoomRoutine = null;
    }

}
