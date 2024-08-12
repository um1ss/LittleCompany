using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public abstract class Monster : Entity
{
    [Header("MonsterFields")]
    [SerializeField] protected float _rageDistance = 1f;
    [SerializeField] protected float _offsetToDestination = 1;
    [SerializeField] protected float _offsetTargetVisionX = 1;
    [SerializeField] protected float _offsetTargetVisionY = 1;
    [SerializeField][Range(-1, 1)] protected float _offsetTargetVectorComparisonZ = 0.5f;

    protected NavMeshAgent _agent;
    protected Animator _animator;
    protected Rigidbody _rb;

    public float RageDistance { get { return _rageDistance; } }
    public float OffsetToDestination { get { return _offsetToDestination; } }
    public float OffsetTargetVisionX { get { return _offsetTargetVisionX; } }
    public float OffsetTargetVisionY { get { return _offsetTargetVisionY; } }
    public float OffsetTargetVectorDotZ { get { return _offsetTargetVectorComparisonZ; } }
    public NavMeshAgent Agent { 
        get 
        { 
            if (_agent == null)
            {
                _agent = GetComponent<NavMeshAgent>();
            }
            return _agent; 
        } 
    }
    public Rigidbody Rigidbody
    {
        get
        {
            if (_rb == null)
            {
                _rb = GetComponent<Rigidbody>();
            }
            return _rb;
        }
    }
    public Animator Animator
    {
        get
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }
            return _animator;
        }
    }
}
