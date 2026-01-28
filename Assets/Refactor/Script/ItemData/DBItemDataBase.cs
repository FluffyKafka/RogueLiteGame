using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Item
{
    public interface IItemDataBase
    {
        public IReadOnlyList<IItemData> CheckStartItemList();
        public IReadOnlyList<IEquipmentData> CheckCraftableWeapon();
        public IReadOnlyList<IEquipmentData> CheckCraftableArmor();
        public IReadOnlyList<IEquipmentData> CheckCraftableAmulet();
        public IReadOnlyList<IEquipmentData> CheckCraftableFlask();
        public IItemData TryCheckItemDataById(string _id);
    }
    internal class DBItemDataBase : MonoBehaviour, IItemDataBase
    {
        [SerializeField] protected string assetDirectorPath;
        [SerializeField] protected List<SOItemData> startItems;

        [Header("Debug")]
        [SerializeField] protected SerializableDictionary<string, SOItemData> itemDatabase = new();
        [SerializeField] protected List<SOEquipmentData> weaponList = new();
        [SerializeField] protected List<SOEquipmentData> armorList = new();
        [SerializeField] protected List<SOEquipmentData> amuletList = new();
        [SerializeField] protected List<SOEquipmentData> flaskList = new();

        public IReadOnlyList<IEquipmentData> CheckCraftableWeapon()
        {
            return weaponList;
        }
        public IReadOnlyList<IEquipmentData> CheckCraftableArmor()
        {
            return armorList;
        }
        public IReadOnlyList<IEquipmentData> CheckCraftableAmulet()
        {
            return amuletList;
        }
        public IReadOnlyList<IEquipmentData> CheckCraftableFlask()
        {
            return flaskList;
        }
        public IReadOnlyList<IItemData> CheckStartItemList()
        {
            return startItems.Cast<IItemData>().ToList().AsReadOnly();
        }

        public IItemData TryCheckItemDataById(string _id)
        {
            SOItemData data;
            if (itemDatabase.TryGetValue(_id, out data))
            {
                return data;
            }
            else
            {
                Debug.LogWarning("物品查找失败, id: " + _id);
                return null;
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Fill Up Item Database And Craft And TradeGoods")]
        protected void GetItemDatabase()
        {
            string[] assetNames = AssetDatabase.FindAssets("t:SOItemData", new[] { assetDirectorPath });

            itemDatabase.Clear();
            weaponList.Clear();
            armorList.Clear();
            amuletList.Clear();
            flaskList.Clear();
            foreach (string SOName in assetNames)
            {
                var SOPath = AssetDatabase.GUIDToAssetPath(SOName);
                var itemData = AssetDatabase.LoadAssetAtPath<SOItemData>(SOPath);

                //数据库填充          
                itemDatabase.Add(itemData.ChechItemId(), itemData);

                if(itemData is SOEquipmentData)
                {
                    SOEquipmentData equip = itemData as SOEquipmentData;
                    switch(equip.CheckEquipmentType())
                    {
                        case EEquipmentType.Weapon:
                            weaponList.Add(equip);break;
                        case EEquipmentType.Armor:
                            armorList.Add(equip);break;
                        case EEquipmentType.Amulet:
                            amuletList.Add(equip);break;
                        case EEquipmentType.Flask:
                            flaskList.Add(equip);break;
                    }
                }               
            }

            foreach (string SOName in assetNames)
            {
                var SOPath = AssetDatabase.GUIDToAssetPath(SOName);
                var itemData = AssetDatabase.LoadAssetAtPath<SOItemData>(SOPath);

                //制作材料列表获取
                if (itemData is SOEquipmentData)
                {
                    var equipment = itemData as SOEquipmentData;
                    equipment.craftingMaterials.Clear();
                    foreach (int id in equipment.craftingMaterialsId)
                    {
                        SOItemData craft;
                        if (id >= 0 && itemDatabase.TryGetValue(id.ToString(), out craft))
                        {
                            equipment.craftingMaterials.Add(craft);
                        }
                    }
                    EditorUtility.SetDirty(itemData);
                }
            }
        }
#endif
    }
}