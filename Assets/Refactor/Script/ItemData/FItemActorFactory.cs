using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public interface IEquipmentFactory
    {
        public IEquipment GenerateEquipment(IItemData _equipment);
        public IItem GenerateItem(IItemData _equipment);
    }

    internal class FItemActorFactory : MonoBehaviour, IEquipmentFactory
    {
        protected List<WItem> items = new();

        IItem IEquipmentFactory.GenerateItem(IItemData _data)
        {
            WItem newItem = new();
            newItem.Init(_data);
            items.Add(newItem);
            return newItem;
        }

        IEquipment IEquipmentFactory.GenerateEquipment(IItemData _equipment)
        {
            WEquipment newEquipment = new();
            newEquipment.Init(_equipment);
            items.Add(newEquipment);
            return newEquipment;
        }
    }
}

