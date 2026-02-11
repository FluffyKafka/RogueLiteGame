using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyBehaviour
{
    internal class SNecromancerBattleMove : SNecromancerBattle
    {
        public SNecromancerBattleMove(CNecromancerStateMachine _stateMachine, MEnemyBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            enemy.InvokeAction(enemy.ToMove);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            if (IdleCheckWithFlip())
            {
                enemyStateMachine.ChangeState(enemyStateMachine.battleIdle);
            }
            else
            {
                enemy.InvokeAction(enemy.MoveToward_Battle, enemy.InvokeFunc(enemy.CheckBattleMoveDir));
            }
        }
    }
}

