using UnityEngine;

public class Target : MonoBehaviour, ICanPushAway
{
    private Rigidbody _rigidbody;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    public void PushAway(Vector3 forcePointPosition, float pushForce)
    {
        var forceDir = transform.position - forcePointPosition;
        forceDir /= forceDir.magnitude;
        forceDir *= pushForce;
        _rigidbody.AddForce(forceDir, ForceMode.Impulse);
    }
}
