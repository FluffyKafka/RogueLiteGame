using EntitySystem;
using ObjectGenerateData;
using PlayerSystem;
using StatsData;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SkillSystem
{
    public interface IInitSkillManager
    {
        public void Init(ISkillManagerPlayer _player);
    }

    internal class MSkillManager : ComponentManagerBase, IInitSkillManager, IPlayerSkillManager
    {
        protected ISkillManagerPlayer player;

        public Action<int> SkillInputEndNotice;
        public Action<int> SkillInputBeginNotice;

        #region Dash
        public Action DashEnd;
        public Action<float> DashBegin;
        #endregion

        #region CounterAttack
        public Action CounterAttackSuccessNotice;
        #endregion

        protected void Awake()
        {
            DashBegin += player.DashBegin;
            DashEnd += player.DashEnd;
        }

        public void Init(ISkillManagerPlayer _player)
        {
            player = _player;
        }

        public void SkillInputEnd(int _input)
        {
            InvokeAction(SkillInputEndNotice, _input);
        }

        public void SkillInputBegin(int _input)
        {
            InvokeAction(SkillInputBeginNotice, _input);
        }

        public Vector3 CheckMousePosition()
        {
            return player.CheckMousePosition();
        }

        public Transform CheckPlayerTransform()
        {
            return player.CheckPlayerTransform();
        }
        public int CheckPlayerFacingDir()
        {
            return player.CheckPlayerFacingDir();
        }

        public bool CanEffectBehaviourSkill()
        {
            return player.CanEffectBehaviourSkill();
        }

        public void AimmingBegin()
        {
            player.AimmingBegin();
        }
        public void AimmingUpdate(DProjectileAimmingData _data)
        {
            player.AimmingUpdate(_data);
        }

        public void AimmingFinish()
        {
            player.AimmingFinish();
        }

        public void CatchSwordBegin()
        {
            player.CatchSwordBegin();
        }

        public void CatchSwordEnd()
        {
            player.CatchSwordFinish();
        }

        public ISkillObject ThrowSword(DProjectileData _data)
        {
            return player.ThrowSword(_data).GetComponent<ISkillObject>();
        }
        public ISkillObject ThrowSpinSword(DSpinSwordData _data)
        {
            return player.ThrowSpinSword(_data).GetComponent<ISkillObject>();
        }
        public ISkillObject ThrowPierceSword(DProjectileData _data)
        {
            return player.ThrowPierceSword(_data).GetComponent<ISkillObject>();
        }
        public ISkillObject ThrowBounceSword(DBounceSwordData _data)
        {
            return player.ThrowBounceSword(_data).GetComponent<ISkillObject>();
        }

        public WReadOnlyDamageData CheckPlayerPrimaryDamage()
        {
            return player.CheckPlayerDamage();
        }

        public void GeneratePlayerCloneAt(DPlayerCloneData _data, Vector3 _position)
        {
            player.GeneratePlayerClone(_data, _position);
        }

        public void CounterAttackSuccess()
        {
            InvokeAction(CounterAttackSuccessNotice);
        }
        public void CounterAttackBegin()
        {
            player.CounterAttackBegin();
        }
        public void CounterAttackEnd()
        {
            player.CounterAttackEnd();
        }
    }

    public interface ISkillObject
    {
        public Transform GetTransform();
        public void RecycleObject();

        public void TakeBack();
    }
}