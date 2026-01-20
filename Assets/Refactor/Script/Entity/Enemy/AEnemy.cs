using EntitySystem.EntityActor.PlayerActor;
using EntitySystem.EntityComponent.BattleComponent;
using EntitySystem.EntityComponent.MovementComponent;
using EntitySystem.EntityComponent.StateMachineComponent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EntitySystem
{
    namespace EntityActor
    {
        namespace EnemyActor
        {
            public interface IAnimEnemy : IAnimEntity
            {
                public abstract void OpenStun(bool _isOpen);
            }

            internal class AEnemy : AEntity, IAnimEnemy, IPlayerEnemy
            {
                public IEnemyPlayer player;

                #region Action
                public Action StandStill;
                public Action<int> MoveForward;
                public Action<int> MoveToward;
                public Action<int> MoveToward_Battle;
                public Action UpdateBattle;
                public Action StopBattle;
                public Action FacingToPlayer;
                public Action AttackCheck;
                public Action Attack;
                public Action StunCheck;
                public Action BeStunned;
                public Action<bool> OpenStun;
                public Action StunFinish;

                public Action ToIdle;
                public Action ToMove;
                #endregion

                #region Func
                public Func<float> CheckIdleDuration;
                public Func<bool> IsDetectPlayer;
                public Func<int> CheckBattleMoveDir;
                #endregion

                protected override void Awake()
                {
                    base.Awake();

                    Assert.IsTrue(anim is IEnemyAnimation);
                    IEnemyAnimation enemyAnim = anim as IEnemyAnimation;
                    ToIdle += enemyAnim.Idle;
                    ToMove += enemyAnim.Move;
                    Attack += enemyAnim.Attack;
                    BeStunned += enemyAnim.BeStunned;
                    StunFinish += enemyAnim.StunFinish;
                }
                protected override void ComponentValidCheck()
                {
                    Assert.IsNotNull(GetComponent<CEnemyMovement>(), "缺少敌人运动组件");
                    Assert.IsNotNull(GetComponent<CEnemyBattle>(), "缺少敌人战斗组件");
                }
                protected virtual void Start()
                {
                    if(player == null)
                    {
                        MEnemyFactory.GetInstance_TestMode().InitEnemyNotGenerateByFactory_TestMode(this);
                    }

                }

                #region Init
                public void Init(IEnemyPlayer _player)
                {
                    player = _player;
                }
                #endregion

                #region Animation
                void IAnimEntity.AttackDamageTrigger()
                {
                    InvokeAction(AttackDamageTrigger);
                }              

                void IAnimEnemy.OpenStun(bool _isOpen)
                {
                    InvokeAction(OpenStun, true);
                }

                void IAnimEntity.AttackFinish()
                {
                    InvokeAction(AttackFinish);
                }
                #endregion

                #region Player
                bool IPlayerEnemy.IsDead()
                {
                    return isDead;
                }
                Vector3 IPlayerEnemy.CheckPosition()
                {
                    return transform.position;
                }
                WReadOnlyDamageData IPlayerEnemy.TakeDamage(WReadOnlyDamageData _damageData)
                {
                    WReadOnlyDamageData damage = InvokeFunc(CalculateDamageTaken, _damageData);
                    InvokeAction(TakeDamage, damage);
                    return damage;
                }
                void IPlayerEnemy.StunCheck()
                {
                    InvokeAction(StunCheck);
                }
                #endregion

            }

            public interface IEnemyAnimation : IEntityAnimation
            {
                public void Idle();
                public void Move();
                public void Attack();
            }
        }
    }
}


