using Item;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

namespace InventorySystem
{
    internal class CEquipmentStash : CInventoryComponentBase
    {
        [SerializeField] protected int stashSize;
        protected HashSet<IEquipment> equipments = new();
        protected List<IEquipmentData> craftLackEquip = new();

        protected override void Awake()
        {
            base.Awake();

            inventory.TryAddEquipment += TryAddStash;
            inventory.CheckEquipmentStash += CheckEquipmentStash;
            inventory.RemoveFromEquipmentStash += RemoveFromStash;
            inventory.CheckCraft_EquipLack += CheckCraftLack;
            inventory.CheckEquipmentStashMaxSize += CheckStashMaxSize;
            inventory.CraftConsumeStash += CraftConsume;
        }

        protected bool TryAddStash(IEquipment _equipment)
        {
            Assert.IsFalse(equipments.Contains(_equipment), "单一组件内不允许存在指向同一装备实体的多个指针");

            if(equipments.Count >= stashSize)
            {
                return false;
            }
            else
            {
                equipments.Add(_equipment);
                return true;
            }
        }

        protected IReadOnlyList<IEquipment> CheckEquipmentStash()
        {
            return equipments.ToList().AsReadOnly();
        }

        protected void RemoveFromStash(IEquipment _data)
        {
            Assert.IsTrue(equipments.Contains(_data), "尝试从装备仓库移除不存在的装备");
            equipments.Remove(_data);
        }

        protected IReadOnlyList<IEquipmentData> CheckCraftLack(IEquipmentData _data)
        {
            craftLackEquip.Clear();
            IReadOnlyList<IItemData> equips = _data.CheckCraftingMaterials();
            foreach(var equip in equips)
            {
                if(equip is IEquipmentData)
                {
                    if(!ChechStashHaveEquipment(equip))
                    {
                        craftLackEquip.Add(equip as IEquipmentData);
                    }
                }
            }
            return craftLackEquip.AsReadOnly();
        }
        protected bool ChechStashHaveEquipment(IItemData _data)
        {
            foreach (var sta in equipments)
            {
                if (_data == sta.CheckData())
                {
                    return true;
                }
            }
            return false;
        }

        protected int CheckStashMaxSize()
        {
            return stashSize;
        }

        protected void CraftConsume(IEquipmentData _data)
        {
            IReadOnlyList<IItemData> equips = _data.CheckCraftingMaterials();
            foreach(var equip in equips)
            {
                if(equip is IEquipmentData)
                {
                    bool isFind = TryRemoveCraftMaterial(equip);
                    Assert.IsTrue(isFind, "尝试消耗不存在的材料，制作前需调用检查函数");
                }
            }
        }
        protected bool TryRemoveCraftMaterial(IItemData _data)
        {
            foreach(var equip in equipments)
            {
                if(equip.CheckData() == _data)
                {
                    equipments.Remove(equip);
                    return true;
                }
            }
            return false;
        }
    } 
}

