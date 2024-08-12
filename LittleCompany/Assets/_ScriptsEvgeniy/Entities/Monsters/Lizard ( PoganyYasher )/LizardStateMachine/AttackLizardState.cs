using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class AttackLizardState : LizardState
{
    private LayerMask _wallMask;
    private CancellationTokenSource _cancellationTokenSource;
    private string _resetAnimTriggerName;
    public AttackLizardState(LizardStateMachine stateMachine) : base(stateMachine)
    {
        _wallMask = LayerMask.GetMask("Wall");
    }
    public async override void Enter()
    {
        _stateMachine.Monster.Agent.speed = _stateMachine.Monster.RunSpeed;
        _cancellationTokenSource = new CancellationTokenSource();
        foreach (var collider in _stateMachine.Monster.Colliders)
        {
            collider.OnAllPlayerExit += CanselToken;
        }
        try
        {
            // Проверяем состояние токена отмены
            _cancellationTokenSource.Token.ThrowIfCancellationRequested();

            await PrepareToRun(_cancellationTokenSource.Token);

            _cancellationTokenSource.Token.ThrowIfCancellationRequested();
        }
        catch (OperationCanceledException)
        {
            _stateMachine.Monster.Agent.isStopped = false;
            _stateMachine.ChangeState(_stateMachine.CalmStateName);
            return;
        }
        _stateMachine.Monster.Animator.ResetTrigger("Spotted");
        _stateMachine.Monster.Animator.SetTrigger("Run");
        SetEndRunPoint();
        _stateMachine.Monster.SetColliderAttackState();
        _stateMachine.Monster.OnCollisionWithObstakle += StopRuning;
    }
    private void SetEndRunPoint()
    {
        var agentTrans = _stateMachine.Monster.Agent.transform;
        var runDir = _stateMachine.Monster.GetRunPoint().position - _stateMachine.Monster.Agent.transform.position;
        runDir /= runDir.magnitude;
        runDir.y = 0;
        float maxRayDistance = 100f;
        RaycastHit hitInfo;

        Debug.DrawRay(agentTrans.position, runDir * 100, Color.green, 5, true);

        if (Physics.Raycast(agentTrans.position, runDir, out hitInfo, maxRayDistance, _wallMask, QueryTriggerInteraction.Ignore))
        {
            Vector3 hitPoint = hitInfo.point;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(hitPoint, out hit, 40, NavMesh.AllAreas)) 
            {
                _stateMachine.Monster.Agent.SetDestination(hit.position); // Устанавливаем путь до ближайшей точки на NavMesh
                return;
            }
        }
        _stateMachine.ChangeState(_stateMachine.CalmStateName);
    }
    private void CanselToken()
    {
        _cancellationTokenSource?.Cancel();
    }
    private async UniTask PrepareToRun(CancellationToken token)
    {
        _stateMachine.Monster.Animator.SetTrigger("Spotted");
        StopMonsterVelocity();
        _stateMachine.Monster.Agent.isStopped = true;
        await UniTask.Delay(TimeSpan.FromSeconds(_stateMachine.Monster.RunDelay), cancellationToken: token);
        _stateMachine.Monster.Agent.isStopped = false;
    }
    private async void StopRuning(float delay, string obstacleName)
    {
        StopMonsterVelocity();
        if (obstacleName == "Player")
        {
            _stateMachine.Monster.Animator.SetTrigger("Win");
            _resetAnimTriggerName = "Win";
        }
        else
        {
            _stateMachine.Monster.Animator.SetTrigger("Stun");
            _resetAnimTriggerName = "Stun";
        }
        await AfterRunDelay(delay);
        _stateMachine.ChangeState(_stateMachine.CalmStateName);
    }
    private async UniTask AfterRunDelay(float delay)
    {
        _stateMachine.Monster.Agent.isStopped = true;
        await UniTask.Delay(TimeSpan.FromSeconds(delay));
        _stateMachine.Monster.Agent.isStopped = false;
    }
    public override void Exit()
    {
        _stateMachine.Monster.Animator.ResetTrigger("Run");
        _stateMachine.Monster.Animator.ResetTrigger(_resetAnimTriggerName);
        foreach (var collider in _stateMachine.Monster.Colliders)
        {
            collider.OnAllPlayerExit -= CanselToken;
        }
        _stateMachine.Monster.OnCollisionWithObstakle -= StopRuning;
        _stateMachine.Monster.SetColliderSaveState();
    }
}
