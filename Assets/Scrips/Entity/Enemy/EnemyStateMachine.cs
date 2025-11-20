using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState currentState { get; private set; }
    public Enemy enemy { get; private set; }

    public EnemyStateMachine(Enemy _enemy)
    {
        enemy = _enemy;
    }

    public void Initialize(EnemyState _state)
    {
        currentState = _state;
        currentState.Enter();
    }

    public void ChangeState(EnemyState _state)
    {
        if(enemy.isDead)
        {
            return;
        }

        currentState?.Exit();
        currentState = _state;
        currentState.Enter();
    }
}
