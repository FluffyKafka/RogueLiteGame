using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New EquipmentSCOBGenerator", menuName = "ExcelData/Equipment")]
public class EquipmentSCOBGenerator : ScriptableObject
{
    [Header("填写下方三者后右键上方标题导入数据")]
    [SerializeField] private string targetFile_Equipment;
    [SerializeField] private string targetFile_Material;
    [SerializeField] private EquipmentData dataSet;

    [ContextMenu("导入数据")]
    public void ImportData()
    {
#if UNITY_EDITOR
        if(dataSet == null)
        {
            Debug.LogError("未提供数据来源");
            return;
        }

        foreach(ExcelEquipmentData data in dataSet.NewEquipment)
        {
            
            if (data.EquipmentType == "Material")
            {
                ItemData newItem;
                newItem = AssetDatabase.LoadAssetAtPath<ItemData>(targetFile_Material + "/" + data.ObjectName + ".asset");
                if (newItem == null)
                {
                    newItem = ScriptableObject.CreateInstance<ItemData>();
                    LoadData_Material(newItem, data);
                    UnityEditor.AssetDatabase.CreateAsset(newItem, targetFile_Material + "/" + data.ObjectName + ".asset");
                }
                else
                {
                    LoadData_Material(newItem, data);
                }
            }
            else
            {
                ItemData_Equipment newEquipment;
                newEquipment = AssetDatabase.LoadAssetAtPath<ItemData_Equipment>(targetFile_Equipment + "/" + data.ObjectName + ".asset");
                if (newEquipment == null)
                {
                    newEquipment = ScriptableObject.CreateInstance<ItemData_Equipment>();
                    LoadData_Equipment(newEquipment, data);
                    UnityEditor.AssetDatabase.CreateAsset(newEquipment, targetFile_Equipment + "/" + data.ObjectName + ".asset");
                }
                else
                {
                    LoadData_Equipment(newEquipment, data);
                }
            }

            
        }
        UnityEditor.AssetDatabase.SaveAssets();
#endif
    }

    private void LoadData_Equipment(ItemData_Equipment _newEquipment, ExcelEquipmentData _data)
    {
        _newEquipment.itemId = _data.Id;
        _newEquipment.itemName = _data.ItemName;
        _newEquipment.itemType = ItemType.Equipment;
        _newEquipment.equipmentType = TransTypeFromString(_data.EquipmentType);

        _newEquipment.description = _data.Description;
        _newEquipment.detail = _data.Detail;
        _newEquipment.price = (int)_data.Price;
        _newEquipment.cooldown = _data.CoolDown;

        _newEquipment.statsModifierData = new StatsModifierData();
        _newEquipment.statsModifierData.strength = _data.Strength;
        _newEquipment.statsModifierData.agility = _data.Agility;
        _newEquipment.statsModifierData.intelligence = _data.Intelligence;
        _newEquipment.statsModifierData.vitality = _data.Vitality;

        _newEquipment.statsModifierData.maxHealth = _data.MaxHealth;
        _newEquipment.statsModifierData.armor = _data.Armor;
        _newEquipment.statsModifierData.evasion = _data.Evasion;
        _newEquipment.statsModifierData.magicResistance = _data.MagicResistance;
        _newEquipment.statsModifierData.maxFlaskUsageTime = _data.MaxFlaskUsageTime;
        _newEquipment.statsModifierData.flaskUsageRecover = _data.FlaskUsageRecover;

        _newEquipment.statsModifierData.damage = _data.Damage;
        _newEquipment.statsModifierData.critChance = _data.CritChance;
        _newEquipment.statsModifierData.critPower = _data.CritPower;
        _newEquipment.statsModifierData.attackSpeed = _data.AttackSpeed;

        _newEquipment.statsModifierData.fireDamage = _data.FireDamage;
        _newEquipment.statsModifierData.iceDamage = _data.IceDamage;
        _newEquipment.statsModifierData.lightningDamage = _data.LightningDamage;
        _newEquipment.statsModifierData.fireDuration = _data.FireDuration;
        _newEquipment.statsModifierData.iceDuration = _data.IceDuration;
        _newEquipment.statsModifierData.lightningDuration = _data.LightningDuration;

        _newEquipment.statsModifierData.fireDamageCooldown = _data.FireDamageCooldown;
        _newEquipment.statsModifierData.fireDamageTransform = _data.FireDamageTransform;
        _newEquipment.statsModifierData.chillSlowRate = _data.ChillSlowRate;
        _newEquipment.statsModifierData.chillArmorReduce = _data.ChillArmorReduce;
        _newEquipment.statsModifierData.shockAccuracyReduce = _data.ShockAccuracyReduce;
        _newEquipment.statsModifierData.thunderStrikeCount = _data.ThunderStrikeCount;
        _newEquipment.statsModifierData.thunderStrikeRate = _data.ThunderStrikeRate;

        _newEquipment.craftsId.Add(_data.Craft_0);
        _newEquipment.craftsId.Add(_data.Craft_1);
        _newEquipment.craftsId.Add(_data.Craft_2);
        _newEquipment.craftsId.Add(_data.Craft_3);
        _newEquipment.craftsId.Add(_data.Craft_4);
    }
    private void LoadData_Material(ItemData _newItem, ExcelEquipmentData _data)
    {
        _newItem.itemId = _data.Id;
        _newItem.itemName = _data.ItemName;
        _newItem.itemType = ItemType.Material;
        _newItem.description = _data.Description;
        _newItem.price = (int)_data.Price;
    }

    private EquipmentType TransTypeFromString(string _type)
    {
        switch(_type)
        {
            case "Amulet": return EquipmentType.Amulet;
            case "Weapon": return EquipmentType.Weapon;
            case "Flask": return EquipmentType.Flask;
            case "Armor": return EquipmentType.Armor;
            default: Debug.LogError("Undefine equipment type: " + _type); return EquipmentType.Weapon;
        }
    }
}
