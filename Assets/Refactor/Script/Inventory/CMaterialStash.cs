using Item;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace InventorySystem
{
    internal class CMaterialStash : CInventoryComponentBase
    {
        [SerializeField] protected int stashSize;
        protected HashSet<IItem> materialsStash = new();
        protected List<IItemData> craftLackMaterial = new();

        protected override void Awake()
        {
            base.Awake();

            inventory.TryAddItem += TryAddStash;
            inventory.CheckItemStash += CheckStash;
            inventory.CheckCraft_MaterialLack += CheckCraftLack;
            inventory.CheckMaterialStashMaxSize += CheckStashMaxSize;
            inventory.RemoveFromMaterialStash += RemoveFromStash;
            inventory.CraftConsumeStash += CraftConsume;
        }

        protected bool TryAddStash(IItem _equipment)
        {
            if (materialsStash.Count >= stashSize)
            {
                return false;
            }
            else
            {
                materialsStash.Add(_equipment);
                return true;
            }
        }

        protected IReadOnlyList<IItem> CheckStash()
        {
            return materialsStash.ToList().AsReadOnly();
        }

        protected IReadOnlyList<IItemData> CheckCraftLack(IEquipmentData _data)
        {
            craftLackMaterial.Clear();
            IReadOnlyList<IItemData> materials = _data.CheckCraftingMaterials();
            foreach (var material in materials)
            {
                if (!(material is IEquipmentData))
                {
                    if (!ChechStashHaveMaterial(material))
                    {
                        craftLackMaterial.Add(material);
                    }
                }
            }
            return craftLackMaterial.AsReadOnly();
        }
        protected bool ChechStashHaveMaterial(IItemData _data)
        {
            foreach (var sta in materialsStash)
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

        protected void RemoveFromStash(IItem _item)
        {
            Assert.IsTrue(materialsStash.Contains(_item), "行为错误：尝试从材料库中删除不存在的材料");
            materialsStash.Remove(_item);
        }

        protected void CraftConsume(IEquipmentData _data)
        {
            IReadOnlyList<IItemData> equips = _data.CheckCraftingMaterials();
            foreach (var equip in equips)
            {
                if (!(equip is IEquipmentData))
                {
                    bool isFind = TryRemoveCraftMaterial(equip);
                    Assert.IsTrue(isFind, "尝试消耗不存在的材料，制作前需调用检查函数");
                }
            }
        }
        protected bool TryRemoveCraftMaterial(IItemData _data)
        {
            foreach (var equip in materialsStash)
            {
                if (equip.CheckData() == _data)
                {
                    materialsStash.Remove(equip);
                    return true;
                }
            }
            return false;
        }
    }
}

