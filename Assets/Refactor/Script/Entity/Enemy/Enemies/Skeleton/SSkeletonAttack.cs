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
            internal class SSkeletonAttack : SSkeletonState
            {
                public SSkeletonAttack(CEntityStateMachine _stateMachine, AEntity _entity) : base(_stateMachine, _entity)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                    enemy.AttackFinish += OnAttackFinish;
                    enemy.BeStunned += OnStun;
                }

                public override void Exit()
                {
                    base.Exit();
                    enemy.AttackFinish -= OnAttackFinish;
                    enemy.BeStunned -= OnStun;
                }

                public override void Update()
                {
                    base.Update();
                }

                protected void OnAttackFinish()
                {
                    enemyStateMachine.ChangeState(enemyStateMachine.battleIdle);
                }

                protected void OnStun()
                {
                    enemyStateMachine.ChangeState(enemyStateMachine.stunned);
                }
            }
        }
    }
}
