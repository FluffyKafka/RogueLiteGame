using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace InventorySystem
{
    internal abstract class CInventoryComponentBase : MonoBehaviour
    {
        protected MInventory inventory;

        protected virtual void Awake()
        {
            inventory = GetComponent<MInventory>();
            Assert.IsNotNull(inventory, GetType().Name + "组件需要附加至一个MInventory");
        }
    }
}