using System.Collections.Generic;
using System.Text;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public enum ItemType
{
    Material, 
    Equipment
}

[CreateAssetMenu(fileName = "New Material Data", menuName = "Item Data/Material")]
public class ItemData : ScriptableObject
{
    [SerializeField] public string itemId;

    [SerializeField] public Sprite icon;
    [SerializeField] public string itemName;
    [SerializeField] public ItemType itemType;
    [SerializeField] public List<InventoryItem> craftingMaterials;
    [TextArea]
    [SerializeField] public string description;
    [SerializeField] public int price;

    protected StringBuilder sb = new StringBuilder();

    public virtual string GetEffectText()
    {
        return "";
    }
}
