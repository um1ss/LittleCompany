using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class LizardStateMachine : MonsterStateMachine<LizardMan>
{
    public readonly string CalmStateName = "CalmState";
    public readonly string AttackStateName = "AttackState";

    public LizardStateMachine(LizardMan monster) : base(monster)
    {
        _states.Add(CalmStateName, new CalmLizardState(this));
        _states.Add(AttackStateName, new AttackLizardState(this));
        ChangeState(CalmStateName);
    }
}
