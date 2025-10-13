using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private UI_ItemToolTip itemDetail;

    [SerializeField] private Image[] materialImages;

    private ItemData_Equipment equipment;

    public void Setup(ItemData_Equipment _equipment)
    {
        if (_equipment == null)
        {
            return;
        }

        gameObject.SetActive(true);

        equipment = _equipment;

        itemDetail.ShowToolTip(_equipment);

        foreach(var material in materialImages)
        {
            TextMeshProUGUI amountText = material.GetComponentInChildren<TextMeshProUGUI>();
            amountText.color = Color.clear;
            material.color = Color.clear;
        }

        Assert.IsTrue(_equipment.craftingMaterials.Count <= materialImages.Length, "制作材料类型过多无法显示");

        for(int i = 0; i < _equipment.craftingMaterials.Count; ++i)
        {
            materialImages[i].color = Color.white;
            materialImages[i].sprite = _equipment.craftingMaterials[i].data.icon;
            TextMeshProUGUI amountText = materialImages[i].GetComponentInChildren<TextMeshProUGUI>();
            amountText.color = Color.white;
            amountText.text = _equipment.craftingMaterials[i].stackSize.ToString();
        }
    }
    public void Craft()
    {
        if (equipment != null)
        {
            List<InventoryItem> lacks = Inventory.instance.TryGetMaterialsLackToCraft(equipment);
            if(PlayerManager.instance.player.IsBlackSmithAround())
            {
                if (lacks.Count == 0)
                {
                    Inventory.instance.CraftEquipment_AfterCheck(equipment);
                    SceneAudioManager.instance.uiSFX.craft.Play(null);
                }
                else
                {
                    UI.instance.warningToolTip.ShowWarning("缺少必要材料");
                }
            }
            else
            {
                UI.instance.warningToolTip.ShowWarning("只有在铁匠身边才有制作工具");
            }
        }
    }
}
