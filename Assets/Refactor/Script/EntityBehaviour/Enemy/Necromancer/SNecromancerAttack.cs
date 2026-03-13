using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyBehaviour
{
    internal class SNecromancerAttack : SNecromancerState
    {
        public SNecromancerAttack(CNecromancerStateMachine _stateMachine, MEnemyBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        public override void Enter()
        {
            base.Enter();
            enemy.AttackFinish += OnAttackFinish;
            enemy.InvokeAction(enemy.ToBattle, true);
        }

        public override void Exit()
        {
            base.Exit();
            enemy.AttackFinish -= OnAttackFinish;
            enemy.InvokeAction(enemy.ToBattle, false);
        }

        public override void Update()
        {
            base.Update();
            enemy.InvokeAction(enemy.StandStill);
            enemy.InvokeAction(enemy.FacingToPlayer);
        }

        protected void OnAttackFinish()
        {
            enemyStateMachine.ChangeState(enemyStateMachine.controll);
        }
    }
}

