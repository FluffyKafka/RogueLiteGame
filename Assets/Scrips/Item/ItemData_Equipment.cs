using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Equipment Data", menuName = "Item Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    [SerializeField] public EquipmentType equipmentType;
    [SerializeField] public StatsModifierData statsModifierData;
    [SerializeField] public List<ItemEffect> effects;
    [SerializeField] public float cooldown;
    [TextArea][SerializeField] public string detail;

    [Header("TEST")]
    [SerializeField] private float cooldownFinishTime = -1;

    public void AddModifiers()
    {
        PlayerManager.instance.player.cs.AddModifier(statsModifierData);
    }

    public void RemoveModifiers()
    {
        PlayerManager.instance.player.cs.RemoveModifier(statsModifierData);
    }

    public bool TryExcuteItemEffect(EffectExcuteData _target)
    {
        if(IsCooldownFinish())
        {
            foreach (var effect in effects)
            {
                _target.equipment = this;
                effect.ExcuteEffect(_target);
            }
            SetCooldownFinishTime();
            return true;
        }
        return false;
    }

    public override string GetEffectText()
    {
        sb.Clear();
        if(cooldown > 0)
        {
            sb.Append("--");
            sb.Append("装备冷却时间：");
            sb.Append(cooldown.ToString());
            sb.Append("--");
            sb.AppendLine();
        }
        sb.Append(detail);
        return sb.ToString();
    }

    public bool IsCooldownFinish()
    {
        return cooldownFinishTime < Time.time;
    }

    public void SetCooldownFinishTime()
    {
        cooldownFinishTime = cooldown + Time.time;
    }

    public float CheckCooldownPercentage()
    {
        float cooldownTime = cooldownFinishTime - Time.time;
        if(cooldownTime < 0)
        {
            return 0;
        }
        else if(cooldownTime > cooldown)
        {
            return 1;
        }
        else
        {
            return cooldownTime / cooldown;
        }
    }

    public float CheckCooldownRemainingTime()
    {
        return cooldownFinishTime - Time.time;
    }

    public void LoadCooldownFinishTime(float _time)
    {
        cooldownFinishTime = Time.time + _time;
    }
}
