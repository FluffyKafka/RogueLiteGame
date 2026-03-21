using EntityBehaviour;
using PlayerSystem;
using StatsData;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

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
        public Action ToExitMove;
        public Action ToWallSlide;
        public Action StandStillNotice;
        public Action<bool> SetGravityToZeroNotice;

        //Input
        public Action<float> HorizonInput;
        public Action<float> VerticalInput;
        public Action JumpInput;
        public Action AttackInput;
        public Action<float> UpdateYVelocity;
        public Func<GameObject, bool> StunCheck;
        public Action InteractToNPCInputNotice;

        //Skill
        public Action<float> OnDashBegin;
        public Action OnDashEnd;
        public Action OnDashMovementUpdate;
        public Action OnAimmingBegin;
        public Action OnAimmingFinish;
        public Action<DProjectileAimmingData> OnAimmingUpdate;
        public Action OnCounterAttackBegin;
        public Action OnCounterAttackEnd;
        public Action OnCounterAttackSuccessFinish;
        public Func<bool> CounterAttackCheckNotice;

        //BattleCheck
        public Action<bool> SetPlayerToBattleNotice;

        //NPCInteract
        public Action InteractToNPCNotice;
        public Action InteractFinishNotice;
        public Action NPCEffectFinishNotice;
        public Action NPCEffectFailNotice;

        //ObjectInteract
        public Action InteractToObjectNotice;

        //UI
        public Action CommunicateFinishNotice;
        #endregion

        #region Func
        public Func<float> CheckUnmovableDurationAfterAttack;
        public Func<bool> IsGroundedOrPlatform_Strict;
        public Func<float> CheckHorizonInput;
        public Func<float> CheckVerticalInput;
        public Func<GameObject, bool> IsEnemy;
        public Func<GameObject, bool> IsEnemyAlive;
        public Func<GameObject, WReadOnlyDamageData,  WReadOnlyDamageData> DamageTo;
        public Func<bool> CanEffectBehaviourSkillNotice;
        public Func<bool> CheckIsPlayerInBattleNotice;
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
            ToDead += playerSystem.ToDead;
            ToExitMove += playerSystem.ToExitMove;
        }

        public void ToCounterAttack()
        {
            playerSystem.ToCounterAttack();
        }
        public void ToCounterAttackSuccess()
        {
            playerSystem.ToCounterAttackSuccess();
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

        public void DashBegin(float _speed)
        {
            InvokeAction(OnDashBegin, _speed);//状态组件进入dash，移动组件dash
        }

        public void DashEnd()
        {
            InvokeAction(OnDashEnd);
        }

        public bool CanEffectBehaviourSkill()
        {
            return InvokeFunc(CanEffectBehaviourSkillNotice);
        }

        public void AimmingBegin()
        {
            InvokeAction(OnAimmingBegin);
        }

        public void AimmingUpdate(DProjectileAimmingData _data)
        {
            InvokeAction(OnAimmingUpdate, _data);
        }

        public void AimmingFinish()
        {
            InvokeAction(OnAimmingFinish);
        }

        public void CatchSwordBegin()
        {
            InvokeAction(OnAimmingBegin);
        }

        public void CatchSwordFinish()
        {
            InvokeAction(OnAimmingFinish);
        }

        public void CounterAttackBegin()
        {
            InvokeAction(OnCounterAttackBegin);
        }

        public void CounterAttackEnd()
        {
            InvokeAction(OnCounterAttackEnd);
        }
        public void CounterAttackSuccessFinish()
        {
            InvokeAction(OnCounterAttackSuccessFinish);
        }

        public void SetPlayerToBattle(bool _isBattle)
        {
            InvokeAction(SetPlayerToBattleNotice, _isBattle);
        }
        public bool CheckIsPlayerInBattle()
        {
            if(CheckIsPlayerInBattleNotice == null)
            {
                return false;
            }    

            return InvokeFunc(CheckIsPlayerInBattleNotice);
        }

        public void InteractToNPC(IPlayerNPC _npc)
        {
            playerSystem.InteractToNPC(_npc);
        }

        public void InteractToNPCInput()
        {
            InvokeAction(InteractToNPCInputNotice);
        }
        public void CommunicateFinish()
        {
            InvokeAction(CommunicateFinishNotice);
        }
        public void InteractFinish()
        {
            InvokeAction(InteractFinishNotice);
        }
        public void NPCEffectFinish()
        {
            InvokeAction(NPCEffectFinishNotice);
        }
        public void NPCEffectFail()
        {
            InvokeAction(NPCEffectFailNotice);
        }
        public KeyCode CheckNPCInteractInputKey()
        {
            return playerSystem.CheckNPCInteractInputKey();
        }

        public void GeneratePopUpText(string _text)
        {
            playerSystem.GeneratePopUpText(_text);
        }

        public KeyCode CheckObjectInteractInputKey()
        {
            return playerSystem.CheckObjectInteractInputKey();
        }

        public void InteractToObject(IPlayerInteractable _object)
        {
            playerSystem.InteractToObject(_object);
        }

        public void ObjectInteractInput()
        {
            InvokeAction(InteractToObjectNotice);
        }
    }
}

