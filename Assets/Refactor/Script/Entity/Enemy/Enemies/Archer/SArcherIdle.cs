using EntitySystem.EntityActor;
using EntitySystem.EntityComponent.StateMachineComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySystem
{
    namespace EntityState
    {
        namespace ArcherState
        {
            internal class SArcherIdle : SArcherPeace
            {
                protected Coroutine idleToMoveNoticeCoRoutine;

                public SArcherIdle(CEntityStateMachine _stateMachine, AEntity _entity) : base(_stateMachine, _entity)
                {
                }

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
                        enemy.InvokeAction(enemy.Flip);
                    }
                    enemyStateMachine.ChangeState(enemyStateMachine.move);
                }
            }
        }
    }
}
