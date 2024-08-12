using UnityEngine;

public class InteractController : MonoBehaviour
{
    private bool ShouldInteract => Input.GetKeyDown(_interactKey);

    [Header("Bingding KeyCode")] [SerializeField]
    private KeyCode _interactKey = KeyCode.E;
    
    [SerializeField] private Vector3 _viewport;
    [SerializeField] private float _range;
    [SerializeField] private LayerMask _layer;

    private Camera _camera;
    private Interactable _currentInteractable;

    private void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        HandleInteractionCheck();
    }

    private void HandleInteractionCheck()
    {
        if (ShouldInteract)
        {
            if (Physics.Raycast(_camera.ViewportPointToRay(_viewport), out RaycastHit hit, _range, _layer, QueryTriggerInteraction.Ignore))
            {
                if (_currentInteractable == null || hit.collider.gameObject.GetInstanceID() != _currentInteractable.GetInstanceID())
                {
                    if (hit.collider.TryGetComponent(out _currentInteractable))
                    {
                        _currentInteractable.OnInteract();
                    }
                }
            }
            else
            {
                _currentInteractable = null;
            }
        }
    }
}
