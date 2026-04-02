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

        protected bool isAmmoFinish = false; 

        public override void Enter()
        {
            base.Enter();
            enemy.AttackFinish += OnAttackFinish;
            enemy.ObjectFinishNotice += OnAmmoFinish;
            enemy.InvokeAction(enemy.ToBattle, true);
            isAmmoFinish = false;
        }

        public override void Exit()
        {
            base.Exit();
            enemy.AttackFinish -= OnAttackFinish;
            enemy.ObjectFinishNotice -= OnAmmoFinish;
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
            if(!isAmmoFinish)
            {
                enemyStateMachine.ChangeState(enemyStateMachine.controll);
            }     
            else
            {
                enemyStateMachine.ChangeState(enemyStateMachine.battleIdle);
            }
        }
        protected void OnAmmoFinish()
        {
            isAmmoFinish = true;
        }
    }
}

