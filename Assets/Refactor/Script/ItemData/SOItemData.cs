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
        public IReadOnlyCollection<IItemData> CheckCraftingMaterials();
        public string CheckDescription();
        public int CheckPrice();
    }

    [CreateAssetMenu(fileName = "New Material Data", menuName = "Item Data/Material")]
    internal class SOItemData : ScriptableObject, IItemData
    {
        [SerializeField] protected string itemId;

        [SerializeField] protected Sprite icon;
        [SerializeField] protected string itemName;
        [SerializeField] protected EItemType itemType;
        [SerializeField] protected List<IItemData> craftingMaterials;
        [TextArea][SerializeField] protected string description;
        [SerializeField] protected int price;

        string IItemData.ChechItemId()
        {
            return itemId;
        }

        IReadOnlyCollection<IItemData> IItemData.CheckCraftingMaterials()
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

