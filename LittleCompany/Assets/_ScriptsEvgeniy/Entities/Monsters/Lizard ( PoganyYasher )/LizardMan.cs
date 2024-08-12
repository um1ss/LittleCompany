using System;
using System.Collections.Generic;
using UnityEngine;

public class LizardMan : Monster
{
    public Action<float, string> OnCollisionWithObstakle;

    [Header("LizardManFields")]
    [SerializeField] private Transform[] _walkPositions;
    [SerializeField] private float _runSpeed = 2;
    [SerializeField] private float _stunDuration = 2;
    [SerializeField] private float _bigRoomFearDuration = 2;
    [SerializeField] private float _playerKillDuration = 1;
    [SerializeField] private float _pushForce = 2;
    [SerializeField] private float _runDelay = 2;
    [SerializeField] private LizardsCheckPlayerCollider[] _checkPlayerColliders;

    private LizardStateMachine _stateMachine;
    private bool _isAttacking;

    private int _currentPosIndex;
    private int _indexChanged = 1;

    private Transform _attackPlayerPos;

    private Dictionary<string, float> _afterRunDelays = new Dictionary<string, float>();

    public float RunDelay { get { return _runDelay; } }
    public float RunSpeed { get { return _runSpeed; } }
    public LizardsCheckPlayerCollider[] Colliders { get { return _checkPlayerColliders; } }
    private void Start()
    {
        _stateMachine = new LizardStateMachine(this);
        _currentPosIndex = 0;

        _afterRunDelays.Add("Wall", _stunDuration);
        _afterRunDelays.Add("Player", _playerKillDuration);
    }
    private void Update()
    {
        _stateMachine.UpdateState();
    }
    public void UpIndexPos()
    {
        _currentPosIndex += _indexChanged;
        if (_currentPosIndex >= _walkPositions.Length || _currentPosIndex < 0)
        {
            _currentPosIndex -= _indexChanged * 2;
            _indexChanged *= -1;
        }
    }
    public Vector3 GetNearestPosition()
    {
        Vector3 pos = _walkPositions[0].position;
        _currentPosIndex = 0;
        for (int i = 0; i < _walkPositions.Length; i++)
        {
            if (Vector3.Distance(transform.position, pos) > (Vector3.Distance(transform.position, _walkPositions[i].position)))
            {
                pos = _walkPositions[i].position;
                _currentPosIndex = i;
            }
        }
        return pos;
    }
    public Vector3 GetWalkPosition()
    {
        var pos = _walkPositions[_currentPosIndex].position;
        return pos;
    }
    public void SetPlayerTransform(Transform pos)
    {
        _attackPlayerPos = pos;
    }
    public Transform GetRunPoint()
    {
        return _attackPlayerPos;
    }
    public void SetColliderAttackState()
    {
        _isAttacking = true;
    }
    public void SetColliderSaveState()
    {
        _isAttacking = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!_isAttacking) return;

        var tag = collision.gameObject.tag;

        if (_afterRunDelays.ContainsKey(tag))
        {
            OnCollisionWithObstakle?.Invoke(_afterRunDelays[tag], tag);
        }

        if (collision.gameObject.TryGetComponent(out ICanPushAway pusher))
        {
            pusher.PushAway(transform.position, _pushForce);
        }

        if (collision.gameObject.TryGetComponent(out IDamageTaker taker))
        {
            taker.TakeDamage();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("BigRoom")) return;
        OnCollisionWithObstakle?.Invoke(_bigRoomFearDuration, "BigRoom");
    }
}
