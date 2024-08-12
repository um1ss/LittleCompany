using UnityEngine;

public class Door : Interactable
{
    private bool _isOpen = false;
    private bool _canInteractedWith = true;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override void OnInteract()
    {
        if (_canInteractedWith)
        {
            _isOpen = !_isOpen;
            _animator.SetBool("isOpen", _isOpen);
        }
    }

    public override void OnFocus()
    {
    }

    public override void OnLoseFocus()
    {
    }
}
