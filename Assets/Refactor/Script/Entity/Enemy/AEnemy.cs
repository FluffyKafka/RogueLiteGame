using EntitySystem.EntityActor.PlayerActor;
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
                public void OpenStun(bool _isOpen);
            }

            public interface IPlayerEnemy
            {
                public void StunCheck();
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
                    BeStunned += enemyAnim.Stun;
                }

                protected virtual void Start()
                {
                    if(player == null)
                    {
                        MEnemyFactory.GetInstance_TestMode().InitEnemyNotGenerateByFactory_TestMode(this);
                    }

                }

                public void Init(IEnemyPlayer _player)
                {
                    player = _player;
                }

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

                void IPlayerEnemy.StunCheck()
                {
                    InvokeAction(StunCheck);
                }

            }

            public interface IEnemyAnimation
            {
                public void Idle();
                public void Move();
                public void Attack();
                public void Stun();
            }
        }
    }
}


