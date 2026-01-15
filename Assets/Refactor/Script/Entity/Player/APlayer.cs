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

            public interface IAnimPlayer
            {
                public void AttackFinish();
            }

            internal class APlayer : AEntity, IInitPlayer, IInputPlayer, IAnimPlayer
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
                public Action<float> UpdateAnimYVelocity;

                public Action AttackRaw;
                public Action<int> Attack;
                public Action AttackFinish;
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
                #region Animation
                void IAnimPlayer.AttackFinish()
                {
                    InvokeAction(AttackFinish);
                }
                #endregion

                protected override void Awake()
                {
                    base.Awake();
                    Assert.IsTrue(anim is IPlayerAnimation, "Player需要一个IPlayerAnimation的动画组件");
                    IPlayerAnimation playerAnim = anim as IPlayerAnimation;
                    UpdateAnimYVelocity += playerAnim.UpdateYVelocity;
                    Attack += playerAnim.Attack;
                    CheckHorizonInput += input.CheckHorizonInput;
                    CheckVerticalInput += input.CheckVerticalInput;
                }
            }

            public interface IPlayerAnimation : IEntityAnimation
            {
                public void UpdateYVelocity(float _yVelocity);
                public void Attack(int _count);
            }

            public interface IPlayerInput
            {
                public float CheckHorizonInput();
                public float CheckVerticalInput();
            }
        }        
    }
}