using StatsData;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Item
{
    public enum EEquipmentType
    {
        Weapon,
        Armor,
        Amulet,
        Flask
    }

    public interface IEquipmentData: IItemData
    {
        public EEquipmentType CheckEquipmentType();
        public WReadOnlyStatsData CheckStatsModifierData();
        public void ExcuteItemEffect(DEffectExcuteData _target);
        public float CheckCooldown();
        public string GetEffectText();
    }


    [CreateAssetMenu(fileName = "New Equipment Data", menuName = "Item Data/Equipment")]
    internal class SOEquipmentData : SOItemData, IEquipmentData
    {
        [SerializeField] protected EEquipmentType equipmentType;
        [SerializeField] protected DStatsData statsModifierData;
        [SerializeField] protected List<SOItemEffect> effects;
        [SerializeField] protected float cooldown;
        [SerializeField][TextArea] protected string detail;

        protected StringBuilder sb = new StringBuilder();

        public EEquipmentType CheckEquipmentType()
        {
            return equipmentType;
        }
        public WReadOnlyStatsData CheckStatsModifierData()
        {
            return new WReadOnlyStatsData(statsModifierData);
        }

        public void ExcuteItemEffect(DEffectExcuteData _target)
        {
            foreach (var effect in effects)
            {                
                effect.ExcuteEffect(_target, this);
            }
        }
        public float CheckCooldown()
        {
            return cooldown;
        }
        public string GetEffectText()
        {
            sb.Clear();
            if (cooldown > 0)
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

        public float CheckCoolDown()
        {
            return cooldown;
        }
    }
}
