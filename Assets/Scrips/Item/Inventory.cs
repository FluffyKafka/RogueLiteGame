using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.VisualScripting;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory instance;

    [SerializeField] private List<ItemData> startItems;

    [SerializeField] private GameObject itemObjectPrefab;
    [SerializeField] private Vector2 itemObjectThrowVelocity;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;


    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipmentSlot[] equipmentSlot;

    [Header("Dead Drop Info")]
    [SerializeField] private Vector2 minDropVelocity;
    [SerializeField] private Vector2 maxDropVelocity;

    [Header("Equipment Cooldown")]
    private float flaskCoolDownDuration;
    private float flaskCoolDownTimer;


    [Header("Database")]
    [SerializeField] private List<InventoryItem> loadedItems_item = new List<InventoryItem>();
    [SerializeField] private List<InventoryItem> loadedItems_equipment = new List<InventoryItem>();
    [SerializeField] private bool isNewGame = true;
    [SerializeField] private SerializableDictionary<string, ItemData> itemDatabase;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();
    }

    private void Start()
    {
        UI.instance.ActivateAllUI();
        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        UI.instance.HideAllUI();

        AddStartItems();
    }

    private void Update()
    {

    }

    private void AddStartItems()
    {
        if(!isNewGame)
        {
            foreach(InventoryItem item in loadedItems_item)
            {
                TryAddItem(item.data, item.stackSize);
            }
            foreach (InventoryItem item in loadedItems_equipment)
            {
                AddEquipment(item.data as ItemData_Equipment);
                UpdateEquipmentUI();
            }
            return;
        }
        foreach(var item in startItems)
        {
            isNewGame = false;
            if(!TryAddItem(item))
            {
                return;
            }
        }
    }

    public ItemData_Equipment TryGetEquipedEquipmentByType(EquipmentType _type)
    {
        foreach(var item in equipmentDictionary.Keys)
        {
            if(_type == item.equipmentType)
            {
                return item;
            }
        }
        return null;
    }

    public float GetFlaskCooldownPercentage()
    {
        ItemData_Equipment equipmentData = TryGetEquipedEquipmentByType(EquipmentType.Flask);
        if(equipmentData == null)
        {
            return 0;
        }
        return equipmentData.CheckCooldownPercentage();
    }

    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment itemToEquip = _item as ItemData_Equipment;
        if(itemToEquip != null)
        {
            ItemData_Equipment equiped = TryGetSameTypeEquipmentEquiped(itemToEquip);
            if (equiped != null)
            {
                RemoveEquipment(equiped);
                TryAddItem(equiped);
            }

            RemoveItem(itemToEquip);
            AddEquipment(itemToEquip);

            UpdateEquipmentUI();
        }
    }
    public void UnequipItem(ItemData _item)
    {
        ItemData_Equipment equipment = _item as ItemData_Equipment;
        if (equipment != null)
        {
            RemoveEquipment(equipment);
            TryAddItem(_item);
            UpdateEquipmentUI();
        }
    }
    private void AddEquipment(ItemData_Equipment _equipment)
    {
        InventoryItem newItem = new InventoryItem(_equipment);
        equipment.Add(newItem);
        equipmentDictionary.Add(_equipment, newItem);

        _equipment.AddModifiers();
    }
    private void RemoveEquipment(ItemData_Equipment _equipment)
    {
        if (equipmentDictionary.TryGetValue(_equipment, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(_equipment);

            _equipment.RemoveModifiers();
        }
        else
        {
            Assert.IsTrue(equipmentDictionary.ContainsKey(_equipment), "Try To Unequip Unequiped Equipment");
        }
    }
    private ItemData_Equipment TryGetSameTypeEquipmentEquiped(ItemData_Equipment _item)
    {
        foreach(ItemData_Equipment item in equipmentDictionary.Keys)
        {
            if(_item.equipmentType == item.equipmentType)
            {
                return item;
            }
        }
        return null;
    }

    private void UpdateEquipmentUI()
    {
        for (int j = 0; j < equipmentSlot.Length; ++j)
        {
            equipmentSlot[j].Clear();
            foreach (var item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[j].equipmentType)
                {
                    equipmentSlot[j].UpdateSlot(item.Value);
                    break;
                }
            }
        }
    }

    private void UpdateSlotUI()
    {
        int i = 0;
        for (; i < inventory.Count; ++i)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }
        for(; i < inventoryItemSlot.Length; ++i)
        {
            if (inventoryItemSlot[i].IsNull())
            {
                break;
            }
            inventoryItemSlot[i].Clear();
        }

        i = 0;
        for (;i < stash.Count; ++i)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }
        for (; i < stashItemSlot.Length; ++i)
        {
            if (stashItemSlot[i].IsNull())
            {
                break;
            }
            stashItemSlot[i].Clear();
        }
    }

    public bool TryAddItem(ItemData _item, int _count = 1)
    {
        if(CanAddItem(_item, _count))
        {
            if (_item.itemType == ItemType.Material)
            {
                AddToStash(_item, _count);
            }
            else if (_item.itemType == ItemType.Equipment)
            {
                AddToInventory(_item, _count);
            }
            UpdateSlotUI();
            return true;
        }
        else
        {
            return false;
        }
    }
    private void AddToInventory(ItemData _item, int _count)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack(_count);
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
            newItem.stackSize = _count;
        }
    }
    private void AddToStash(ItemData _item, int _count)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack(_count);
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
            newItem.stackSize = _count;
        }
    }

    public void RemoveItem(ItemData _item, int _count = 1)
    {
        if (_item.itemType == ItemType.Material)
        {
            RemoveFromStash(_item, _count);
        }
        else if (_item.itemType == ItemType.Equipment)
        {
            RemoveFromInventory(_item, _count);
        }
        else
        {
            Assert.IsTrue(_item.itemType == ItemType.Material || _item.itemType == ItemType.Equipment, "Undefine ItemType");
        }

        UpdateSlotUI();
    }
    private void RemoveFromInventory(ItemData _item, int _count)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.RemoveStack(_count);
            if (value.stackSize <= 0)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
        }
        else
        {
            Assert.IsTrue(inventoryDictionary.ContainsKey(_item), "_item don't exist in inventory!");
        }
    }
    private void RemoveFromStash(ItemData _item, int _count)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.RemoveStack(_count);
            if (value.stackSize <= 0)
            {
                stash.Remove(value);
                stashDictionary.Remove(_item);
            }
        }
        else
        {
            Assert.IsTrue(stashDictionary.ContainsKey(_item), "_item don't exist in stash!"); ;
        }
    }

    public List<InventoryItem> TryGetMaterialsLackToCraft(ItemData_Equipment _equipment)
    {
        List<InventoryItem> laskMaterials = new List<InventoryItem>();

        foreach(InventoryItem item in _equipment.craftingMaterials)
        {
            if(stashDictionary.TryGetValue(item.data, out InventoryItem material))
            {
                if(material.stackSize < item.stackSize)
                {
                    InventoryItem lack = item;
                    lack.stackSize = item.stackSize - material.stackSize;
                    laskMaterials.Add(lack);
                }
            }
            else if(inventoryDictionary.TryGetValue(item.data, out InventoryItem equip))
            {
                if(equip.stackSize < item.stackSize)
                {
                    InventoryItem lack = item;
                    lack.stackSize = item.stackSize - equip.stackSize;
                    laskMaterials.Add(lack);
                }
            }
            else
            {
                laskMaterials.Add(item);
            }
        }
        return laskMaterials;
    }
    public void CraftEquipment_AfterCheck(ItemData_Equipment _equipment)
    {
        foreach (InventoryItem item in _equipment.craftingMaterials)
        {
            RemoveItem(item.data, item.stackSize);
        }
        TryAddItem(_equipment);
    }

    public void DropItem(ItemData _item)
    {
        RemoveItem(_item);
        ThrowItemObjectToward(_item, new Vector2(itemObjectThrowVelocity.x * PlayerManager.instance.player.facingDir, itemObjectThrowVelocity.y));
    }
    public void DropAllItem()
    {
        foreach (var item in equipmentDictionary)
        {
            ThrowItemObjectToward(item.Key, GetRandomVelocity());
        }

        foreach (var item in stashDictionary)
        {
            ThrowItemObjectToward(item.Key, GetRandomVelocity());
        }

        foreach (var item in inventoryDictionary)
        {
            ThrowItemObjectToward(item.Key, GetRandomVelocity());
        }

        equipmentDictionary.Clear();
        equipment.Clear();

        stashDictionary.Clear();
        stash.Clear();

        inventoryDictionary.Clear();
        inventory.Clear();

        UpdateSlotUI();
        UpdateEquipmentUI();
    }
    private void ThrowItemObjectToward(ItemData _item, Vector2 _velocity)
    {
        ItemObject newItemObject = Instantiate(itemObjectPrefab, PlayerManager.instance.player.transform.position, Quaternion.identity).GetComponent<ItemObject>();
        newItemObject.ThrowToward(_velocity);
        newItemObject.SetItemData(_item);
    }
    private Vector2 GetRandomVelocity()
    {
        return new Vector2(Random.Range(-1, 1) * Random.Range(minDropVelocity.x, maxDropVelocity.x), Random.Range(minDropVelocity.y, maxDropVelocity.y));
    }

    public ItemData_Equipment TryGetEquipmentByType(EquipmentType _type)
    {
        foreach(var item in equipmentDictionary.Keys)
        {
            if(item.equipmentType == _type)
            {
                return item;
            }
        }
        return null;
    }

    public bool TryUseFlask()
    {
        ItemData_Equipment flask = TryGetEquipmentByType(EquipmentType.Flask);
        if (flask != null && (PlayerManager.instance.player.GetStats() as PlayerStats).IsFlaskUsable())
        {
            EffectExcuteData data = new EffectExcuteData(EffectExcuteTime.UseFlask);
            if(flask.TryExcuteItemEffect(data))
            {
                (PlayerManager.instance.player.GetStats() as PlayerStats).ReduceFlaskUsageTime(1);
                return true;
            }
            return false;
        }
        return false;
    }

    public bool CanAddItem(ItemData _item, int count = 1)
    {
        if(_item.itemType == ItemType.Material)
        {
            return stashDictionary.ContainsKey(_item) || 1 <= stashItemSlot.Length - stash.Count;
        }
        else if( _item.itemType == ItemType.Equipment)
        {
            return stashDictionary.ContainsKey(_item) || 1 <= inventoryItemSlot.Length - inventory.Count;
        }
        else
        {
            Assert.IsTrue(false, "Undefine ItemType");
            return false;
        }
    }

    public void LoadData(GameData _data)
    {
        isNewGame = _data.isNewGame;

        foreach(var item in _data.items)
        {
            if(itemDatabase.TryGetValue(item.Key, out ItemData value))
            {
                InventoryItem itemToLoad = new InventoryItem(value);
                itemToLoad.stackSize = item.Value;
                loadedItems_item.Add(itemToLoad);
            }
            else
            {
                Assert.IsTrue(false, "Missing Item:" + item.Key);
            }
        }

        foreach(var item in _data.equipment)
        {
            if (itemDatabase.TryGetValue(item.Key, out ItemData value))
            {
                InventoryItem itemToLoad = new InventoryItem(value);
                itemToLoad.stackSize = item.Value;
                loadedItems_equipment.Add(itemToLoad);
            }
            else
            {
                Assert.IsTrue(false, "Missing Item:" + item.Key);
            }
        }

        foreach(var item in itemDatabase)
        {
            ItemData_Equipment equipment = item.Value as ItemData_Equipment;
            if(equipment != null)
            {
                equipment.LoadCooldownFinishTime(-1);
            }
        }

        foreach(var item in _data.equipmentCooldown)
        {
            if (itemDatabase.TryGetValue(item.Key, out ItemData value))
            {
                ItemData_Equipment equipment = value as ItemData_Equipment;
                equipment.LoadCooldownFinishTime(item.Value);
            }
            else
            {
                Assert.IsTrue(false, "Missing Item:" + item.Key);
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.items.Clear();
        _data.equipment.Clear();

        _data.isNewGame = isNewGame;
        foreach(var pair in inventoryDictionary)
        {
            _data.items.Add(pair.Key.itemId, pair.Value.stackSize);
        }
        foreach (var pair in stashDictionary)
        {
            _data.items.Add(pair.Key.itemId, pair.Value.stackSize);
        }
        foreach (var pair in equipmentDictionary)
        {
            _data.equipment.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach(var pair in itemDatabase)
        {
            ItemData_Equipment equipment = pair.Value as ItemData_Equipment;
            if(equipment != null)
            {
                if (!equipment.IsCooldownFinish())
                {
                    float remainingTime = equipment.CheckCooldownRemainingTime();
                    _data.equipmentCooldown.Add(pair.Key, remainingTime);
                }
            }
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Fill Up Item Database")]
    private void GetItemDatabase()
    {
        string[] assetNames = AssetDatabase.FindAssets("t:ItemData", new[] {"Assets/Scrips/Item/ItemData"});

        foreach(string SOName in assetNames)
        {
            var SOPath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOPath);
            itemDatabase.Add(itemData.itemId, itemData);
        }

        foreach (string SOName in assetNames)
        {
            var SOPath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOPath);
            if(itemData is ItemData_Equipment)
            {
                var equipment = itemData as ItemData_Equipment;
                equipment.craftingMaterials.Clear();
                foreach (int id in equipment.craftsId)
                {
                    ItemData craft;
                    if(id >= 0 && itemDatabase.TryGetValue(id.ToString(), out craft))
                    {
                        equipment.craftingMaterials.Add(new InventoryItem(craft));
                    }
                }
            }
        }
    }
#endif
}