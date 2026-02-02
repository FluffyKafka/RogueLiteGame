using EntityBehaviour;
using PlayerSystem;
using StatsData;
using System;
using UnityEngine;

namespace PlayerBebaviour
{
    internal class MPlayerBeviour : MEntityBehaviour, IPlayerBehaviour
    {
        protected IBehaviourPlayer playerSystem;

        #region Action
        public Action AttackRaw;
        public Action<int> Attack;
        public Action Jump;
        public Action<float> Move;
        public Action<float> WallSlide;
        public Action WallJump;
        public Action ToIdle;
        public Action ToMove;
        public Action ToWallSlide;

        //Input
        public Action<float> HorizonInput;
        public Action<float> VerticalInput;
        public Action JumpInput;
        public Action AttackInput;
        public Action<float> UpdateYVelocity;
        public Action<GameObject> StunCheck;
        #endregion

        #region Func
        public Func<float> CheckUnmovableDurationAfterAttack;
        public Func<bool> IsGroundedOrPlatform_Strict;
        public Func<float> CheckHorizonInput;
        public Func<float> CheckVerticalInput;
        public Func<GameObject, bool> IsEnemy;
        public Func<GameObject, bool> IsEnemyAlive;
        public Func<GameObject, WReadOnlyDamageData,  WReadOnlyDamageData> DamageTo;
        #endregion

        protected void Awake()
        {
            playerSystem = GetComponent<IBehaviourPlayer>();
            StunCheck += playerSystem.StunCheck;
            IsEnemy += playerSystem.IsEnemy;
            IsEnemyAlive += playerSystem.IsEnemyAlive;
            DamageTo += playerSystem.DamageTo;
            CheckHorizonInput += playerSystem.CheckHorizonInput;
            CheckVerticalInput += playerSystem.CheckVerticalInput;
            Attack += playerSystem.ToAttack;
            Jump += playerSystem.ToJump;
            ToIdle += playerSystem.ToIdle;
            ToMove += playerSystem.ToMove;
            ToWallSlide += playerSystem.ToWallSlide;
            WallJump += playerSystem.ToWallJump;
            UpdateYVelocity += playerSystem.UpdateYVelocity;
            GetPrimaryAttackDamage += playerSystem.GetPrimaryAttackDamage;
        }

        void IPlayerBehaviour.AttackInput()
        {
            InvokeAction(AttackInput);
        }

        void IPlayerBehaviour.HorizonInput(float _xInput)
        {
            InvokeAction(HorizonInput, _xInput);
        }

        void IPlayerBehaviour.JumpInput()
        {
            InvokeAction(JumpInput);
        }

        void IPlayerBehaviour.VerticalInput(float _yInput)
        {
            InvokeAction(VerticalInput, _yInput);
        }
    }
}

