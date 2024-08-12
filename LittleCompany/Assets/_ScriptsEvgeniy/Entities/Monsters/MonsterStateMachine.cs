using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterStateMachine<T> where T : Monster
{
    protected MonsterState _currentState;

    protected Dictionary<string, MonsterState> _states = new Dictionary<string, MonsterState>();

    protected T _monster;

    public T Monster {  get { return _monster; } }
    public MonsterStateMachine(T monster)
    {
        _monster = monster;
    }
    public void UpdateState()
    {
        _currentState.StateUpdate();
    }
    public void ChangeState(string stateName)
    {
        _currentState?.Exit();
        _currentState = _states[stateName];
        _currentState?.Enter();
    }
}
