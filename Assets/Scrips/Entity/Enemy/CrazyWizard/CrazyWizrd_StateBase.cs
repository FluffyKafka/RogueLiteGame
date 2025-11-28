using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyWizrd_StateBase : EnemyState
{
    protected CrazyWizrd enemy;
    public CrazyWizrd_StateBase(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, CrazyWizrd _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
