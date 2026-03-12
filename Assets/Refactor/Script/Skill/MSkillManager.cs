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

    public interface ISaveSkill
    {
        public class DSkillSaveData
        {
            public Dictionary<string, bool> skillUnlock = new();
        }
        public void Save(ref DSkillSaveData _data);
        public void Load(DSkillSaveData _data);
    }

    internal class MSkillManager : ComponentManagerBase, IInitSkillManager, IPlayerSkillManager, ISaveSkill
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

        #region SkillEntityManager
        public Action<string, bool> InitSkillNotice;
        public Func<List<DSkillEntityUIData>> ShowAllSkillEntityToUINotice;
        public Func<List<DSkillUnlockData>> CheckAllSkillUnlockStateNotice;
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
        public void SwordHitGround(Transform _sword)
        {
            player.SwordHitGround(_sword);
        }
        public void SwordHitEnemy(Transform _sword)
        {
            player.SwordHitEnemy(_sword);
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

        public void SelfHealByPercent(float _percent)
        {
            player.SelfHealByPercent(_percent);
        }
        public void AddStatsModifier(WReadOnlyStatsData _data)
        {
            player.AddStatsModifier(_data);
        }
        public void RemoveStatsModifier(WReadOnlyStatsData _data)
        {
            player.RemoveStatsModifier(_data);
        }

        public List<DSkillEntityUIData> ShowAllSkillEntityToUi()
        {
            return InvokeFunc(ShowAllSkillEntityToUINotice);
        }
        public List<DSkillUnlockData> CheckAllSkillUnlockState()
        {
            return InvokeFunc(CheckAllSkillUnlockStateNotice);
        }

        public void Save(ref ISaveSkill.DSkillSaveData _data)
        {
            List<DSkillUnlockData> skills = CheckAllSkillUnlockState();
            _data.skillUnlock.Clear();
            foreach(var skill in skills)
            {
                _data.skillUnlock.Add(skill.skillId, skill.isUnlock);
            }
        }
        public void Load(ISaveSkill.DSkillSaveData _data)
        {
            foreach(var skill in _data.skillUnlock)
            {
                InvokeAction(InitSkillNotice, skill.Key, skill.Value);
            }
        }
    }

    public interface ISkillObject
    {
        public Transform GetTransform();
        public void RecycleObject();

        public void TakeBack();
    }
}