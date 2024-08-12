using UnityEngine;

public class GrabingController : MonoBehaviour
{
    private bool ShouldGrabingItem => Input.GetMouseButtonDown(0);
    
    [Header("PickUp Parameters")] 
    [SerializeField] private Vector3 _viewport;
    [SerializeField] private Transform _holdArea;
    [SerializeField] private float _range;
    [SerializeField] private float _force;
    [SerializeField] private LayerMask _layer;

    public GameObject _heldGameObject;
    
    private Camera _camera;
    private Rigidbody _heldGameObjectRigidbody;
    private PickUpController _pickUpController;

    private void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
        _pickUpController = GetComponent<PickUpController>();
    }

    private void Update()
    {
        HandleGrabingInput();
    }

    private void HandleGrabingInput()
    {
        if (_pickUpController._currentPickUpItem == null)
        {
            if (ShouldGrabingItem)
            {
                if (_heldGameObject == null)
                {
                    if (Physics.Raycast(_camera.ViewportPointToRay(_viewport), out RaycastHit hit, _range, _layer, QueryTriggerInteraction.Ignore))
                    {
                        GrabingObject(hit.transform.gameObject);
                    }
                }
                else
                {
                    DropGrabingObject();
                }
            }

            if (_heldGameObject != null)
            {
                MoveGrabingObject();
            }
        }
    }

    private void GrabingObject(GameObject gameObject)
    {
        if (gameObject.GetComponent<Rigidbody>())
        {
            _heldGameObjectRigidbody = gameObject.GetComponent<Rigidbody>();
            _heldGameObjectRigidbody.useGravity = false;
            _heldGameObjectRigidbody.drag = 10;
            _heldGameObjectRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            _heldGameObjectRigidbody.transform.parent = _holdArea;
            _heldGameObject = gameObject;
        }
    }

    private void DropGrabingObject()
    {
        _heldGameObjectRigidbody.useGravity = true;
        _heldGameObjectRigidbody.drag = 1;
        _heldGameObjectRigidbody.constraints = RigidbodyConstraints.None;

        _heldGameObjectRigidbody.transform.parent = null;
        _heldGameObject = null;
    }

    private void MoveGrabingObject()
    {
        if (Vector3.Distance(_heldGameObject.transform.position, _holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = (_holdArea.position - _heldGameObject.transform.position);
            _heldGameObjectRigidbody.AddForce(moveDirection * _force);
        }
    }
}