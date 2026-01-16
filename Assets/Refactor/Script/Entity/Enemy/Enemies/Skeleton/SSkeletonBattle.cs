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
            internal class SSkeletonBattle : SSkeletonState
            {
                public SSkeletonBattle(CEntityStateMachine _stateMachine, AEntity _entity, string _animName) : base(_stateMachine, _entity, _animName)
                {
                }

                public override void Enter()
                {
                    base.Enter();
                    enemy.StopBattle += StopBattle;
                    enemy.Attack += Attack;
                }

                public override void Exit()
                {
                    base.Exit();
                    enemy.StopBattle -= StopBattle;
                    enemy.Attack -= Attack;
                }

                public override void Update()
                {
                    base.Update();
                    enemy.InvokeAction(enemy.UpdateBattle);
                    enemy.InvokeAction(enemy.AttackCheck);
                }

                protected void StopBattle()
                {
                    enemyStateMachine.ChangeState(enemyStateMachine.idle);
                }

                protected bool IdleCheckWithFlip()
                {
                    int moveDir = enemy.InvokeFunc(enemy.CheckBattleMoveDir);
                    if (moveDir == 0)
                    {
                        return true;
                    }
                    else
                    {
                        if(!enemy.InvokeFunc(enemy.IsGroundedOrPlatForm) || enemy.InvokeFunc(enemy.IsTouchWall))
                        {
                            if(moveDir == enemy.InvokeFunc(enemy.CheckFacingDir))
                            {
                                return true;
                            }
                            else
                            {
                                enemy.InvokeAction(enemy.Flip);
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                protected void Attack()
                {
                    enemyStateMachine.ChangeState(enemyStateMachine.attack);
                }
            }
        }
    }
}