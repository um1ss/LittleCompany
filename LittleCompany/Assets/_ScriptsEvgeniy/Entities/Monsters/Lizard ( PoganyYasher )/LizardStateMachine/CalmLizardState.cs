using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CalmLizardState : LizardState
{
    private Transform _agentTransform;
    private Vector3 _currentTargetPos;
    private bool _isAttacking;
    public CalmLizardState(LizardStateMachine stateMachine) : base(stateMachine)
    {
        _agentTransform = _stateMachine.Monster.gameObject.transform;
    }
    public override void Enter()
    {
        _stateMachine.Monster.Agent.speed = _stateMachine.Monster.MoveSpeed;
        _stateMachine.Monster.Agent.angularSpeed = _stateMachine.Monster.RotationSpeed;
        foreach (var collider in _stateMachine.Monster.Colliders)
        {
            collider.OnPlayerEnter += ChangeStateToAttack;
        }
        _isAttacking = false;
        SetNearestPoint();
        _stateMachine.Monster.Animator.SetTrigger("Walk");
    }
    public override void StateUpdate()
    {
        if (_isAttacking) return;
        MoveToTarget();
    }
    private bool CheckPlayerInCollider()
    {
        List<Transform> targets = new List<Transform>();
        foreach (var collider in _stateMachine.Monster.Colliders)
        {
            if (collider.IsEnterTargets && collider.EnterTarget.Count > 0)
            {
                targets = targets.Concat(collider.EnterTarget).ToList();
            }
        }
        if (targets.Count > 0)
        {
            Transform nearTargetPos = targets[0];
            foreach (var target in targets)
            {
                if (Vector3.Distance(_agentTransform.position, nearTargetPos.position) > Vector3.Distance(_agentTransform.position, target.position))
                {
                    nearTargetPos = target;
                }
            }
            ChangeStateToAttack(nearTargetPos);
            return true;
        }
        return false;
    }
    private void MoveToTarget()
    {
        float dist = Vector3.Distance(_currentTargetPos, _agentTransform.position);
        if (dist < _stateMachine.Monster.OffsetToDestination)
        {
            _stateMachine.Monster.UpIndexPos();
            if (CheckPlayerInCollider())
            {
                return;
            }
            ChangeTarget();
        }
    }
    private void SetNearestPoint()
    {
        StopMonsterVelocity();
        _currentTargetPos = _stateMachine.Monster.GetNearestPosition();
        _stateMachine.Monster.Agent.SetDestination(_currentTargetPos);
    }
    private void ChangeTarget()
    {
        StopMonsterVelocity();
        _currentTargetPos = _stateMachine.Monster.GetWalkPosition();
        _stateMachine.Monster.Agent.SetDestination(_currentTargetPos);
    }
    private void ChangeStateToAttack(Transform playerTrans)
    {
        _stateMachine.Monster.SetPlayerTransform(playerTrans);
        _isAttacking = true;
        _agentTransform.DOLookAt(playerTrans.position, 0.5f).
            OnComplete(() => _stateMachine.ChangeState(_stateMachine.AttackStateName));
    }
    public override void Exit()
    {
        _stateMachine.Monster.Animator.ResetTrigger("Walk");
        foreach (var collider in _stateMachine.Monster.Colliders)
        {
            collider.OnPlayerEnter -= ChangeStateToAttack;
        }
    }
}
