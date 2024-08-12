using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MopWashCollider : MonoBehaviour
{
    [SerializeField] private Spot _spotPrefab;

    private Mop _mop;
    private BoxCollider _boxCollider;
    private Rigidbody _rigidbody;
    private void Awake()
    {
        _mop = GetComponentInParent<Mop>();
        _boxCollider = GetComponent<BoxCollider>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        _mop.OnColliderSwitch += SwitchCollider;
    }
    private void OnDisable()
    {
        _mop.OnColliderSwitch -= SwitchCollider;
    }
    private void SwitchCollider(bool isEnable)
    {
        _boxCollider.enabled = isEnable;
        _rigidbody.isKinematic = !isEnable;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (_mop.Health <= 0 && collision.gameObject.CompareTag("Wall"))
        {
            var point = collision.GetContact(0);
            var spot = Instantiate(_spotPrefab, point.point, Quaternion.LookRotation(point.normal));
            spot.SetColor(_mop.DirtColor);
        }
    }
}
