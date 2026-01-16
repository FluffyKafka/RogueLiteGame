using EntitySystem.EntityActor.PlayerActor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntitySystem
{
    namespace EntityActor
    {
        namespace EnemyActor
        {
            public interface IInitEnemy
            {
                public void Init(IEnemyPlayer _player);
            }

            public interface IAnimEnemy : IAnimEntity
            {
                public void OpenStun(bool _isOpen);
            }

            public interface IPlayerEnemy
            {
                public void StunCheck();
            }

            internal class AEnemy : AEntity, IInitEnemy, IAnimEnemy, IPlayerEnemy
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
                #endregion

                #region Func
                public Func<float> CheckIdleDuration;
                public Func<bool> IsDetectPlayer;
                public Func<int> CheckBattleMoveDir;
                #endregion

                protected virtual void Start()
                {
                    if(player == null)
                    {
                        MEnemyFactory.GetInstance_TestMode().InitEnemyNotGenerateByFactory_TestMode(this);
                    }
                }

                void IInitEnemy.Init(IEnemyPlayer _player)
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

            //尚未实现Fx系统，实现后需要注册到对应事件
            public interface IEnemyFx
            {
                public void PlayStunFx();
                public void PlayTakeDamageFx();
            }
        }
    }
}


