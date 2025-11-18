using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset(LogOnImport = true)]
public class EquipmentData : ScriptableObject
{
    //装备更新流程：
    //  将Excel文件Reimport从而将其中的数据输入EquipmentData对象
    //  -> 使用EquipmentSCOBGenerator(itemData文件夹中)将数据转为一个个的对象
    //  -> 在Inventory处重新注册这些装备
    //  -> 在UI处将能组装的装备置入Craft系统
    [Header("将Excel文件Reimport从而将其中的数据输入EquipmentData对象")]
	public List<ExcelEquipmentData> NewEquipment; // Replace 'EntityType' to an actual type that is serializable.
}
