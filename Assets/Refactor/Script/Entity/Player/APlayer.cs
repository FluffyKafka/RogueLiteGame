using EntitySystem.EntityActor;
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
        namespace PlayerActor
        {
            public interface IInitPlayer
            {
                public void Init(IPlayerInput _inputSource);
            }

            public interface IInputPlayer
            {
                public void HorizonInput(float _input);
                public void VerticalInput(float _input);
                public void JumpInput();
                public void AttackInput();
            }

            public interface IEnemyPlayer
            {
                public bool IsDead();
                public Vector3 CheckPosition();

                public WReadOnlyDamageData TakeDamage(WReadOnlyDamageData _damageData);
            }

            public interface IAnimPlayer: IAnimEntity
            {

            }

            internal class APlayer : AEntity, IInitPlayer, IInputPlayer, IAnimPlayer, IEnemyPlayer
            {
                protected IPlayerInput input;

                #region Action
                public Action<float> HorizonInput;
                public Action<float> VerticalInput;
                public Action JumpInput;
                public Action AttackInput;

                public Action Jump;
                public Action<float> Move;
                public Action<float> WallSlide;
                public Action WallJump;
                public Action<float> UpdateYVelocity;

                public Action AttackRaw;
                public Action<int> Attack;

                public Action ToIdle;
                public Action ToMove;
                public Action ToWallSlide;
                #endregion

                #region Func
                public Func<bool> IsGroundedOrPlatform_Strict;//确保只有当Player接触地面时才返回true
                public Func<float> CheckHorizonInput;
                public Func<float> CheckVerticalInput;
                public Func<float> CheckUnmovableDurationAfterAttack;
                #endregion

                #region Init
                void IInitPlayer.Init(IPlayerInput _inputSource)
                {
                    input = _inputSource;
                }
                #endregion

                #region Input
                void IInputPlayer.HorizonInput(float _input)
                {
                    InvokeAction(HorizonInput, _input);
                }
                void IInputPlayer.VerticalInput(float _input)
                {
                    InvokeAction(VerticalInput, _input);
                }
                void IInputPlayer.JumpInput()
                {                    
                    InvokeAction(JumpInput);
                }
                void IInputPlayer.AttackInput()
                {
                    InvokeAction(AttackInput);
                }
                #endregion

                #region Enemy
                bool IEnemyPlayer.IsDead()
                {
                    return isDead;
                }
                Vector3 IEnemyPlayer.CheckPosition()
                {
                    return transform.position;
                }
                WReadOnlyDamageData IEnemyPlayer.TakeDamage(WReadOnlyDamageData _damageData)
                {
                    WReadOnlyDamageData damage = InvokeFunc(CalculateDamageTaken, _damageData);
                    InvokeAction(TakeDamage, damage);
                    return damage;
                }
                #endregion

                protected override void Awake()
                {
                    base.Awake();

                    Assert.IsTrue(anim is IPlayerAnimation, "Player需要一个IPlayerAnimation的动画组件");
                    IPlayerAnimation playerAnim = anim as IPlayerAnimation;
                    Attack += playerAnim.Attack;
                    WallJump += playerAnim.Air;
                    Jump += playerAnim.Air;
                    UpdateYVelocity += playerAnim.UpdateYVelocity;
                    ToIdle += playerAnim.Idle;
                    ToMove += playerAnim.Move;
                    ToWallSlide += playerAnim.WallSlide;


                    CheckHorizonInput += input.CheckHorizonInput;
                    CheckVerticalInput += input.CheckVerticalInput;
                }
                override protected void ComponentValidCheck()
                {
                    Assert.IsNotNull(GetComponent<CPlayerMovement>(), "缺少玩家运动组件");
                    Assert.IsNotNull(GetComponent<CPlayerBattle>(), "缺少玩家战斗组件");
                    Assert.IsNotNull(GetComponent<CPlayerStateMachine>(), "缺少玩家状态机组件");
                }
            }

            public interface IPlayerAnimation : IEntityAnimation
            {
                public abstract void Idle();
                public abstract void Move();
                public abstract void Attack(int _count);
                public abstract void Air();
                public abstract void UpdateYVelocity(float _yVelocity);
                public abstract void WallSlide();
            }

            public interface IPlayerInput
            {
                public float CheckHorizonInput();
                public float CheckVerticalInput();
            }

            public interface IPlayerEnemy
            {
                public bool IsDead();
                public Vector3 CheckPosition();

                public WReadOnlyDamageData TakeDamage(WReadOnlyDamageData _damageData);

                public void StunCheck();
            }
        }        
    }
}