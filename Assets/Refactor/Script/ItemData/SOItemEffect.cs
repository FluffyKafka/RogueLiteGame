using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public enum EEffectExcuteTime
    {
        PrimaryAttack,
        Clone,
        Crystal,
        Sword,
        CounterAttack,
        UseFlask,
        TakeDamage
    }

    public struct DEffectExcuteData
    {
        public EEffectExcuteTime excuteTime { get; private set; }
        //对护甲为受到的伤害；对护符和武器为造成的伤害；药瓶与此无关
        public float damage { get; private set; }
        //对护甲为伤害来源；对护符和武器为伤害对象；药瓶与此无关
        public Transform target { get; private set; }
        //对装备数据的引用

        public DEffectExcuteData(EEffectExcuteTime _excuteTime, float _damage, Transform _target)
        {
            excuteTime = _excuteTime;
            damage = _damage;
            target = _target;
        }
    }

    internal class SOItemEffect : ScriptableObject
    {
        [SerializeField] public string description;
        public virtual void ExcuteEffect(DEffectExcuteData _targetData, IEquipmentData _equipment)
        {

        }
    }
}
