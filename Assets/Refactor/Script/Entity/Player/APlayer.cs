using EntitySystem.EntityActor;
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
            }

            public interface IPlayerAnimation : IEntityAnimation
            {
                public void Idle();
                public void Move();
                public void Attack(int _count);
                public void Air();
                public void UpdateYVelocity(float _yVelocity);
                public void WallSlide();
            }

            public interface IPlayerInput
            {
                public float CheckHorizonInput();
                public float CheckVerticalInput();
            }

            public interface IPlayerEnemy
            {

            }
        }        
    }
}