using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace InventorySystem
{
    internal class CEquipment : CInventoryComponentBase
    {
        protected IInventoryEquipment armor = null;
        protected IInventoryEquipment weapon = null;
        protected IInventoryEquipment amulet = null;
        protected IInventoryEquipment flask = null;

        protected override void Awake()
        {
            base.Awake();
            inventory.CheckEquipment += CheckEquipment;
            inventory.Equip += Equip;
            inventory.UnEquip += UnEquip;
            inventory.EffectEquipment += EffectEquipment;
        }
        
        protected IEquipment CheckEquipment(EEquipmentType _type)
        {
            return GetEquipmentByType(_type);
        }

        protected void Equip(IEquipment _data)
        {
            Assert.IsNull(GetEquipmentByType(_data.CheckEquipmentData().CheckEquipmentType()), "装备槽位上已经有装备，必须先将装备撤下");
            switch (_data.CheckEquipmentData().CheckEquipmentType())
            {
                case EEquipmentType.Weapon: weapon = _data as IInventoryEquipment; break;
                case EEquipmentType.Armor: armor = _data as IInventoryEquipment; break;
                case EEquipmentType.Amulet: amulet = _data as IInventoryEquipment; break;
                case EEquipmentType.Flask: flask = _data as IInventoryEquipment; break;
                default: Assert.IsTrue(false, "未知装备类型"); break;
            }
        }

        protected void UnEquip(IEquipment _data)
        {
            Assert.IsTrue(_data == GetEquipmentByType(_data.CheckEquipmentData().CheckEquipmentType()), "错误的信号参数：不能取消装备未装备的装备");
            switch (_data.CheckEquipmentData().CheckEquipmentType())
            {
                case EEquipmentType.Weapon: weapon = null; break;
                case EEquipmentType.Armor: armor = null; break;
                case EEquipmentType.Amulet: amulet = null; break;
                case EEquipmentType.Flask: flask = null; break;
                default: Assert.IsTrue(false, "未知装备类型"); break;
            }
        }

        protected IInventoryEquipment GetEquipmentByType(EEquipmentType _type)
        {
            switch (_type)
            {
                case EEquipmentType.Weapon: return weapon;
                case EEquipmentType.Armor: return armor;
                case EEquipmentType.Amulet: return amulet;
                case EEquipmentType.Flask: return flask;
                default: Assert.IsTrue(false, "未知装备类型"); return weapon;
            }
        }

        protected void EffectEquipment(EEquipmentType _type, DEffectExcuteData _data)
        {
            GetEquipmentByType(_type).TryUseEffect(_data);
        }
    }
}

