using UnityEngine;

public abstract class PickUpItem : MonoBehaviour
{
    protected FirstPlayerController _player;
    protected Collider _collider;
    protected Rigidbody _rigidbody;

    private void Awake()
    {
        gameObject.layer = 11;
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public virtual void OnPickUp(Transform container)
    {
        _rigidbody.isKinematic = true;

        gameObject.transform.position = container.transform.position;
        gameObject.transform.rotation = container.transform.rotation;
        
        _collider.enabled = false;
        
        gameObject.transform.SetParent(container);
        
        _player = GetComponentInParent<FirstPlayerController>();
    }

    public virtual void OnDrop(Transform container)
    {
        container.DetachChildren();
        
        _player = null;
        
        gameObject.transform.eulerAngles = new Vector3(transform.position.x, transform.position.z, transform.position.y);
        _rigidbody.isKinematic = false;
        _collider.enabled = true;
    }

    public abstract void OnUse();

    public virtual void OnFocus()
    {
        print("Нажмите 'Е' чтобы поднять предмет " + gameObject.name);
    }

    public virtual void OnLoseFocus()
    {
        
    }
}
