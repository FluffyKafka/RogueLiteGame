using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

namespace Item
{
    public enum EItemType
    {
        Material,
        Equipment
    }

    public interface IItemData
    {
        public string ChechItemId();
        public Sprite CheckIcon();
        public string CheckItemName();
        public EItemType CheckItemType();
        public IReadOnlyList<IItemData> CheckCraftingMaterials();
        public string CheckDescription();
        public int CheckPrice();
    }

    [CreateAssetMenu(fileName = "New Material Data", menuName = "Item Data/Material")]
    internal class SOItemData : ScriptableObject, IItemData
    {
        [SerializeField] public string itemId;

        [SerializeField] public Sprite icon;
        [SerializeField] public string itemName;
        [SerializeField] public EItemType itemType;
        [SerializeField] public List<IItemData> craftingMaterials;
        [TextArea][SerializeField] public string description;
        [SerializeField] public int price;
        [SerializeField] public List<int> craftingMaterialsId;

        public string ChechItemId()
        {
            return itemId;
        }

        public IReadOnlyList<IItemData> CheckCraftingMaterials()
        {
            return craftingMaterials.AsReadOnly();
        }

        string IItemData.CheckDescription()
        {
            return description;
        }

        Sprite IItemData.CheckIcon()
        {
            return icon;
        }

        string IItemData.CheckItemName()
        {
            return itemName;
        }

        EItemType IItemData.CheckItemType()
        {
            return itemType;
        }

        int IItemData.CheckPrice()
        {
            return price;
        }


    }
}

