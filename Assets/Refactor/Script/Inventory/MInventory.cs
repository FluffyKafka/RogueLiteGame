using Item;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace InventorySystem
{
    internal class MInventory : ComponentManagerBase
    {
        #region Action 

        #endregion

        #region Func
        public Func<IEquipment, bool> TryAddEquipment;
        public Func<IItemData, bool> TryAddItem;
        public Func<int> CheckEquipmentStashSize;
        public Func<int> CheckItemStashSize;
        #endregion

        [SerializeField] protected List<IItemData> startItems;

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
                if(item is )
                {
                    if (!InvokeFunc(TryAddEquipment, item as ))
                    {
                        return;
                    }
                }
                else
                {
                    if (!InvokeFunc(TryAddItem, item))
                    {
                        return;
                    }
                }
                
            }
        }
    }
}

