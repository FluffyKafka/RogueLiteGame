using EntitySystem.EntityActor;
using EntitySystem.EntityActor.EnemyActor;
using EntitySystem.EntityComponent.StateMachineComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EntitySystem
{
    namespace EntityState
    {
        namespace ArcherState
        {
            internal class SArcherState : SEntityState
            {
                protected AArcher enemy;
                protected CArcherStateMachine enemyStateMachine;
                public SArcherState(CEntityStateMachine _stateMachine, AEntity _entity) : base(_stateMachine, _entity)
                {
                    Assert.IsTrue(_entity is AArcher, "此状态属于Archer");
                    enemy = _entity as AArcher;

                    Assert.IsTrue(_stateMachine is CArcherStateMachine, "此状态属于Archer");
                    enemyStateMachine = _stateMachine as CArcherStateMachine;
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
        }
    }
}
