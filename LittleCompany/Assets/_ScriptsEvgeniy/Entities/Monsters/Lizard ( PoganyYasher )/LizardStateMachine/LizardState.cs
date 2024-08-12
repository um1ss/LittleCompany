using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LizardState : MonsterState
{
    protected LizardStateMachine _stateMachine;

    public LizardState(LizardStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }
    protected void StopMonsterVelocity()
    {
        _stateMachine.Monster.Agent.velocity = Vector3.zero;
        _stateMachine.Monster.Rigidbody.velocity = Vector3.zero;
        _stateMachine.Monster.Rigidbody.angularVelocity = Vector3.zero;
    }
}
