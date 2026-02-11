using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyBehaviour
{
    internal class SNecromancerControll : SNecromancerState
    {
        public SNecromancerControll(CNecromancerStateMachine _stateMachine, MEnemyBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            enemy.InvokeAction(enemy.ToControll);           
            enemy.ObjectFinishNotice += ControllFinish;
        }

        public override void Exit()
        {
            base.Exit();
            enemy.ObjectFinishNotice -= ControllFinish;
        }

        public override void Update()
        {
            base.Update();
            enemy.InvokeAction(enemy.StandStill);
        }

        protected void ControllFinish()
        {
            enemyStateMachine.ChangeState(enemyStateMachine.battleIdle);
        }
    }
}

