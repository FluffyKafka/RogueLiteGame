using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyBehaviour
{
    internal class SNecromancerIdle : SNecromancerPeace
    {
        public SNecromancerIdle(CNecromancerStateMachine _stateMachine, MEnemyBehaviour _entity) : base(_stateMachine, _entity)
        {
        }

        protected Coroutine idleToMoveNoticeCoRoutine;

        public override void Enter()
        {
            base.Enter();
            idleToMoveNoticeCoRoutine = enemyStateMachine.StartCoroutine(IdleToMoveNotice(enemy.InvokeFunc(enemy.CheckIdleDuration)));
            enemy.InvokeAction(enemy.StandStill);
            enemy.InvokeAction(enemy.ToIdle);
        }

        public override void Exit()
        {
            base.Exit();
            enemyStateMachine.StopCoroutine(idleToMoveNoticeCoRoutine);
        }

        public override void Update()
        {
            base.Update();
        }

        protected IEnumerator IdleToMoveNotice(float _after)
        {
            yield return new WaitForSeconds(_after);
            if (enemy.InvokeFunc(enemy.IsTouchWall) || !enemy.InvokeFunc(enemy.IsGroundedOrPlatForm))
            {
                enemy.InvokeAction(enemy.MoveForward, -1);
            }
            enemyStateMachine.ChangeState(enemyStateMachine.move);
        }
    }
}

