using EntitySystem.EntityActor.PlayerActor;
using Item;
using StatsData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace InventorySystem
{
    public interface IInitInventory
    {
        public abstract void Init(IEquipmentFactory _factory, IInventoryPlayer _player);
    }

    internal class MInventory : ComponentManagerBase, IInitInventory, IPlayerInventory
    {
        protected IInventoryPlayer player;

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
        #endregion

        [SerializeField] protected List<IItemData> startItems;
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
        }

        #region Init
        void IInitInventory.Init(IEquipmentFactory _factory, IInventoryPlayer _player)
        {
            itemFactory = _factory;
            player = _player;
        }
        #endregion

        #region Player
        void IPlayerInventory.Equip(IEquipment _newEquip)
        {
            IEquipment oldEquipment = CheckEquipment(_newEquip.CheckEquipmentData().CheckEquipmentType());
            if(oldEquipment != null)
            {
                UnEquipEquipment(oldEquipment);
            }
            InvokeAction(Equip, _newEquip);
            player.AddModifier(_newEquip.CheckEquipmentData().CheckStatsModifierData());
        }
        void IPlayerInventory.UnEquip(IEquipment _equip)
        {
            IEquipment oldEquipment = CheckEquipment(_equip.CheckEquipmentData().CheckEquipmentType());
            Assert.IsNotNull(oldEquipment, "行为错误：尝试卸除一个未装备的装备");
            UnEquipEquipment(oldEquipment);
        }

        IEquipment IPlayerInventory.CheckEquipment(EEquipmentType _type)
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
            }
            else
            {
                InvokeAction(RemoveFromMaterialStash, _data);
            }
            DropItem(_data);
        }

        public bool TryCraft(IEquipmentData _data)
        {
            IReadOnlyList<IEquipmentData> equipmentLack = InvokeFunc(CheckCraft_EquipLack, _data);
            IReadOnlyList<IItemData> materialLack = InvokeFunc(CheckCraft_MaterialLack, _data);
            //发出警告？
            if (equipmentLack.Count == 0 && materialLack.Count == 0)
            {
                InvokeAction(CraftConsumeStash, _data);
                IEquipment newEquipment = itemFactory.GenerateEquipment(_data);
                if (InvokeFunc(TryAddEquipment, newEquipment))
                {
                    StashFull(newEquipment);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        int IPlayerInventory.CheckEquipmentStashSize()
        {
            return InvokeFunc(CheckEquipmentStashMaxSize);
        }
        public int CheckMaterialStashSize()
        {
            return InvokeFunc(CheckMaterialStashMaxSize);
        }

        public void EffectEquipmentByType(EEquipmentType _type, DEffectExcuteData _data)
        {
            InvokeAction(EffectEquipment, _type, _data);
        }
        #endregion

        //未完成
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

        //未实现
        protected void StashFull(IItem _data)
        {
            //生成提示

            DropItem(_data);
        }
        protected void DropItem(IItem _data)
        {
            //生成一个掉落物
        }
        #endregion
    }
}

