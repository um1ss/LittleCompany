using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    private bool ShouldPickUp => Input.GetKeyDown(_pickUpKey);
    private bool ShouldDrop => Input.GetKeyDown(_drop);
    private bool ShouldUse => Input.GetKeyDown(_use);

    [Header("Bingding KeyCode")] [SerializeField]
    private KeyCode _pickUpKey = KeyCode.E;
    [SerializeField] private KeyCode _drop = KeyCode.G;
    [SerializeField] private KeyCode _use = KeyCode.Mouse0;
    
    [Header("PickUp Parameters")] 
    [SerializeField] private Vector3 _viewport;
    [SerializeField] private Transform _pickUpArea;
    [SerializeField] private float _range;
    [SerializeField] private LayerMask _layer;
    
    public PickUpItem _currentPickUpItem;
    
    private IStrangeObjects _currentStrangeObjects;

    private Camera _camera;
    private GrabingController _grabingController;

    private void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
        _grabingController = GetComponent<GrabingController>();
    }

    private void Update()
    {
        HandlePickUpCheck();
        HandleDropItem();
        HandlePickUpUse();
    }

    private void HandlePickUpCheck()
    {
        if (_grabingController._heldGameObject == null)
        {
            if (ShouldPickUp)
            {
                if (Physics.Raycast(_camera.ViewportPointToRay(_viewport), out RaycastHit hit, _range, _layer, QueryTriggerInteraction.Ignore))
                {
                    if (_currentPickUpItem == null)
                    {
                        if (hit.collider.TryGetComponent(out _currentPickUpItem))
                        {
                            _currentPickUpItem.OnPickUp(_pickUpArea);
                    
                            if (hit.collider.TryGetComponent(out _currentStrangeObjects))
                            {
                                _currentStrangeObjects.OnAffect();
                            }
                        }
                    }
                }
            }
        }
    }
    
    private void HandleDropItem()
    {
        if (ShouldDrop && _currentPickUpItem != null)
        {
            if (_currentStrangeObjects != null)
            {
                _currentStrangeObjects.OnUnAffect();
            }
            
            _currentPickUpItem.OnDrop(_pickUpArea);
            _currentStrangeObjects = null;
            _currentPickUpItem = null;
        }
    }

    private void HandlePickUpUse()
    {
        if (ShouldUse && _currentPickUpItem != null)
        {
            _currentPickUpItem.OnUse();
        }
    }
}
