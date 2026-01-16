using EntitySystem.EntityActor;
using EntitySystem.EntityComponent.StateMachineComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySystem
{
    namespace EntityState
    {
        namespace SkeletonState
        {
            internal class SSkeletonBattleIdle : SSkeletonBattle
            {
                public SSkeletonBattleIdle(CEntityStateMachine _stateMachine, AEntity _entity, string _animName) : base(_stateMachine, _entity, _animName)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                    enemy.InvokeAction(enemy.StandStill);
                }

                public override void Exit()
                {
                    base.Exit();
                }

                public override void Update()
                {
                    base.Update();
                    if(!IdleCheckWithFlip())
                    {
                        enemyStateMachine.ChangeState(enemyStateMachine.battleMove);
                    }
                }
            }
        }
    }
}
