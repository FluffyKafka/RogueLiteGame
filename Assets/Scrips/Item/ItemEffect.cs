using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EffectExcuteTime
{
    PrimaryAttack,
    Clone,
    Crystal,
    Sword,
    CounterAttack, 
    UseFlask,
    TakeDamage
}
public class EffectExcuteData
{
    public EffectExcuteTime excuteTime { get; private set; }
    //对护甲为受到的伤害；对护符和武器为造成的伤害；药瓶与此无关
    public float damage { get; private set; }
    //对护甲为伤害来源；对护符和武器为伤害对象；药瓶与此无关
    public Entity target { get; private set; }
    //对装备数据的引用
    public ItemData_Equipment equipment;

    public EffectExcuteData(EffectExcuteTime _excuteTime, Entity _target = null, float _damage = 0)
    {
        excuteTime = _excuteTime;
        this.damage = _damage;
        this.target = _target;
    }
}

public class ItemEffect : ScriptableObject
{
    [SerializeField] public string description;
    public virtual void ExcuteEffect(EffectExcuteData _targetData)
    {

    }

    public virtual string GetDescription()
    {
        return description;
    }
}
