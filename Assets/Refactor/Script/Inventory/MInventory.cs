using Item;
using PlayerSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace InventorySystem
{
    public interface IInitInventory
    {
        public abstract void Init(IEquipmentFactory _factory, IInventoryPlayer _player, IItemDataBase _itemDatabase);
    }

    public interface ISaveInventory
    {
        public class DInventoryData
        {
            public List<string> itemStash = new();
            public Dictionary<string, float> equipmentStash = new();
            public Dictionary<string, float> equipment = new();
        }

        public void Save(ref DInventoryData _data);
        public void Load(DInventoryData _data);
    }

    internal class MInventory : ComponentManagerBase, IInitInventory, IPlayerInventory, ISaveInventory
    {
        protected IInventoryPlayer player;
        protected IItemDataBase itemDataBase;

        #region Action 
        public Action<IEquipment> Equip;
        public Action<IEquipment> UnEquip;
        public Action<IEquipment> RemoveFromEquipmentStash;
        public Action<IItem> RemoveFromMaterialStash;
        public Action<EEquipmentType, DEffectExcuteData> EffectEquipment;
        public Action<IEquipmentData> CraftConsumeStash;
        #endregion

        #region Func
        public Func<IEquipment, bool> TryAddEquipment;
        public Func<IItem, bool> TryAddItem;
        public Func<int> CheckEquipmentStashMaxSize;
        public Func<int> CheckMaterialStashMaxSize;
        public Func<IEquipmentData, IReadOnlyList<IEquipmentData>> CheckCraft_EquipLack;
        public Func<IEquipmentData, IReadOnlyList<IItemData>> CheckCraft_MaterialLack;
        public Func<EEquipmentType, IEquipment> CheckEquipment;
        public Func<IReadOnlyList<IEquipment>> CheckEquipmentStash;
        public Func<IReadOnlyList<IItem>> CheckItemStash;
        public Func<bool> CanCraftNotice_BlackSmith;
        #endregion

        protected IEquipmentFactory itemFactory;

        protected virtual void Awake()
        {

        }
        protected virtual void Start()
        {
            AddStartItems();
        }
        protected void AddStartItems()
        {
            IReadOnlyList<IItemData> startItems = itemDataBase.CheckStartItemList();
            foreach (var item in startItems)
            {
                if(item is IEquipmentData)
                {
                    IEquipment newEquipment = itemFactory.GenerateEquipment(item as IEquipmentData);
                    if (!InvokeFunc(TryAddEquipment, newEquipment))
                    {
                        return;
                    }
                }
                else
                {
                    IItem newItem = itemFactory.GenerateItem(item);
                    if (!InvokeFunc(TryAddItem, newItem))
                    {
                        return;
                    }
                }   
            }
            player.EquipmentStashChangeNotice(InvokeFunc(CheckEquipmentStash));
            player.MaterialStashChangeNotice(InvokeFunc(CheckItemStash));
        }

        #region Init
        void IInitInventory.Init(IEquipmentFactory _factory, IInventoryPlayer _player, IItemDataBase _itemDatabase)
        {
            itemFactory = _factory;
            player = _player;
            itemDataBase = _itemDatabase;
        }
        #endregion

        #region Player
        void IPlayerInventory.Equip(IEquipment _newEquip)
        {
            InvokeAction(RemoveFromEquipmentStash, _newEquip);
            IEquipment oldEquipment = CheckEquipment(_newEquip.CheckEquipmentData().CheckEquipmentType());
            if(oldEquipment != null)
            {
                UnEquipEquipment(oldEquipment);
            }
            InvokeAction(Equip, _newEquip);
            player.AddModifier(_newEquip.CheckEquipmentData().CheckStatsModifierData());

            player.EquipmentChangeNotice(_newEquip.CheckEquipmentData().CheckEquipmentType(), _newEquip);
            player.EquipmentStashChangeNotice(InvokeFunc(CheckEquipmentStash));
        }
        void IPlayerInventory.UnEquip(IEquipment _equip)
        {
            IEquipment oldEquipment = CheckEquipment(_equip.CheckEquipmentData().CheckEquipmentType());
            Assert.IsNotNull(oldEquipment, "行为错误：尝试卸除一个未装备的装备");
            UnEquipEquipment(oldEquipment);

            player.EquipmentChangeNotice(_equip.CheckEquipmentData().CheckEquipmentType(), null);
            player.EquipmentStashChangeNotice(InvokeFunc(CheckEquipmentStash));
        }

        IEquipment IPlayerInventory.CheckEquipmentByType(EEquipmentType _type)
        {
            return InvokeFunc(CheckEquipment, _type);
        }
        IReadOnlyList<IEquipment> IPlayerInventory.CheckEquipmentStash()
        {
            return InvokeFunc(CheckEquipmentStash);
        }
        public IReadOnlyList<IItem> CheckMaterialStash()
        {
            return InvokeFunc(CheckItemStash);
        }

        public void DropFromStash(IItem _data)
        {
            if(_data is IEquipment)
            {
                IEquipment equip = _data as IEquipment;
                InvokeAction(RemoveFromEquipmentStash, equip);
                player.EquipmentStashChangeNotice(InvokeFunc(CheckEquipmentStash));
            }
            else
            {
                InvokeAction(RemoveFromMaterialStash, _data);
                player.MaterialStashChangeNotice(InvokeFunc(CheckItemStash));
            }
            player.DiscardItem(_data);        
        }


        IReadOnlyList<IItemData> IPlayerInventory.TryCraft(IEquipmentData _data)
        {
            IReadOnlyList<IEquipmentData> equipmentLack = InvokeFunc(CheckCraft_EquipLack, _data);
            IReadOnlyList<IItemData> materialLack = InvokeFunc(CheckCraft_MaterialLack, _data);
            if (equipmentLack.Count == 0 && materialLack.Count == 0)
            {
                InvokeAction(CraftConsumeStash, _data);
                IEquipment newEquipment = itemFactory.GenerateEquipment(_data);
                if (!InvokeFunc(TryAddEquipment, newEquipment))
                {
                    StashFull(newEquipment);
                }

                player.MaterialStashChangeNotice(InvokeFunc(CheckItemStash));
                player.EquipmentStashChangeNotice(InvokeFunc(CheckEquipmentStash));
                return null;
            }
            else
            {
                return equipmentLack.Cast<IItemData>().Concat(materialLack.Cast<IItemData>()).ToList().AsReadOnly();
            }
        }

        int IPlayerInventory.CheckEquipmentStashMaxSize()
        {
            return InvokeFunc(CheckEquipmentStashMaxSize);
        }
        int IPlayerInventory.CheckMaterialStashMaxSize()
        {
            return InvokeFunc(CheckMaterialStashMaxSize);
        }

        public void EffectEquipmentByType(EEquipmentType _type, DEffectExcuteData _data)
        {
            InvokeAction(EffectEquipment, _type, _data);
        }

        IReadOnlyList<IEquipmentData> IPlayerInventory.CheckCraftableEquipmentByType(EEquipmentType _type)
        {
            switch(_type)
            {
                case EEquipmentType.Weapon: return itemDataBase.CheckCraftableWeapon();
                case EEquipmentType.Amulet: return itemDataBase.CheckCraftableAmulet();
                case EEquipmentType.Armor: return itemDataBase.CheckCraftableArmor();
                case EEquipmentType.Flask: return itemDataBase.CheckCraftableFlask();
                default: Assert.IsFalse(true, "未知的装备类型"); return null;
            }           
        }

        bool IPlayerInventory.TryTakeItem(IItem _item)
        {
            if (_item is IEquipment)
            {
                if(InvokeFunc(TryAddEquipment, _item as IEquipment))
                {
                    player.EquipmentStashChangeNotice(InvokeFunc(CheckEquipmentStash));
                    return true;
                }                
            }
            else
            {
                if(InvokeFunc(TryAddItem, _item))
                {
                    player.MaterialStashChangeNotice(InvokeFunc(CheckItemStash));
                    return true;
                }
            }
            return false;
        }

        public bool CheckCanCraft_Blacksmith()
        {
            return InvokeFunc(CanCraftNotice_BlackSmith);
        }
        #endregion

        #region Self
        protected void UnEquipEquipment(IEquipment _oldEquipment)
        {
            InvokeAction(UnEquip, _oldEquipment);
            bool isEquipStashFull = !InvokeFunc(TryAddEquipment, _oldEquipment);
            if (isEquipStashFull)
            {
                StashFull(_oldEquipment);
            }
            player.RemoveModifier(_oldEquipment.CheckEquipmentData().CheckStatsModifierData());
        }
        protected void StashFull(IItem _data)
        {
            player.StashFullNotice(_data);
        }
        #endregion

        public void Save(ref ISaveInventory.DInventoryData _data)
        {
            _data.equipment.Clear();

            SaveEquipment(ref _data, EEquipmentType.Weapon);
            SaveEquipment(ref _data, EEquipmentType.Amulet);
            SaveEquipment(ref _data, EEquipmentType.Armor);
            SaveEquipment(ref _data, EEquipmentType.Flask);

            IReadOnlyList<IEquipment> equipmentStash = InvokeFunc(CheckEquipmentStash);
            foreach (var equip in equipmentStash)
            {
                _data.equipmentStash.Add(
                    equip.CheckData().ChechItemId(), 
                    (equip as IInventoryEquipment).CheckCoolDownRaw()
                    );
            }

            IReadOnlyList<IItem> itemStash = InvokeFunc(CheckItemStash);
            foreach(var item in itemStash)
            {
                _data.itemStash.Add(item.CheckData().ChechItemId());
            }
        }
        protected void SaveEquipment(ref ISaveInventory.DInventoryData _data, EEquipmentType _type)
        {
            IEquipment equipment = InvokeFunc(CheckEquipment, _type);
            if(equipment == null)
            {
                return;
            }
            _data.equipment.Add(
                equipment.CheckData().ChechItemId(), 
                (equipment as IInventoryEquipment).CheckCoolDownRaw()
                );
        }

        public void Load(ISaveInventory.DInventoryData _data)
        {
            foreach(var equipData in _data.equipment)
            {
                IEquipmentData equip = itemDataBase.TryCheckItemDataById(equipData.Key) as IEquipmentData;
                Assert.IsNotNull(equip, "id：" + equipData.Key + "不是装备");
                IEquipment equipment = itemFactory.GenerateEquipment(equip, equipData.Value);
                InvokeAction(Equip, equipment);
            }

            foreach(var equipData in _data.equipmentStash)
            {
                IEquipmentData equip = itemDataBase.TryCheckItemDataById(equipData.Key) as IEquipmentData;
                Assert.IsNotNull(equip, "id：" + equipData.Key + "不是装备");
                IEquipment equipment = itemFactory.GenerateEquipment(equip, equipData.Value);
                bool isFull = !InvokeFunc(TryAddEquipment, equipment);
                if(isFull)
                {
                    Debug.LogWarning("装备仓库已经满了，原有装备无法附加");
                }
            }

            foreach(var itemData in _data.itemStash)
            {
                IItemData item = itemDataBase.TryCheckItemDataById(itemData);
                IItem itemActor = itemFactory.GenerateItem(item);
                bool isFull = !InvokeFunc(TryAddItem, itemActor);
                if (isFull)
                {
                    Debug.LogWarning("物品仓库已经满了，原有物品无法附加");
                }
            }
        }

        public Transform CheckPlayerTransform()
        {
            return player.CheckTransform();
        }

        public List<IItemData> CheckAllItemsCanBeSale()
        {
            return itemDataBase.CheckAllItemsCanBeSale();
        }

        public void AddItemRaw(IItemData _item)
        {
            if(_item.CheckItemType() == EItemType.Material)
            {
                IItem newItem = itemFactory.GenerateItem(_item);
                InvokeFunc(TryAddItem, newItem);
            }
            else
            {
                IEquipment newEquipment = itemFactory.GenerateEquipment(_item);
                InvokeFunc(TryAddEquipment, newEquipment);
            }
        }
    }
}

