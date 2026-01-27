using Item;
using StatsData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UISystem
{
    internal class CUITooltipManager : CUIComponentBase
    {
        [SerializeField] protected CEquipmentDetailUI equipmentDetailTootip;
        [SerializeField] protected CItemDetailUI materialDetailTooltip;
        [SerializeField] protected CStatsDetailUI statsDetailTooltip;
        [SerializeField] protected CWarningToolTip warningToolTip;
        [SerializeField] protected CCraftWindowUI craftWindow;

        protected override void OnEnable()
        {
            base.OnEnable();
            if(equipmentDetailTootip != null)
            {
                ui.ShowEquipmentDetail += ShowEquipmentTootip;
            }
            if(materialDetailTooltip != null)
            {
                ui.ShowMaterialDetail += ShowMaterialTooltip;
            }
            if (statsDetailTooltip != null)
            {
                ui.ShowStatsDetail += ShowStatTooltip;
            }            
            if(warningToolTip != null)
            {
                ui.ShowWarning += ShowWarning;
            }
            if(craftWindow != null)
            {
                ui.ShowCraftWindow += ShowCraftWindow;
            }

            ui.HideTooltip += HideTooltip;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (equipmentDetailTootip != null)
            {
                ui.ShowEquipmentDetail -= ShowEquipmentTootip;
            }
            if (materialDetailTooltip != null)
            {
                ui.ShowMaterialDetail -= ShowMaterialTooltip;
            }
            if (statsDetailTooltip != null)
            {
                ui.ShowStatsDetail -= ShowStatTooltip;
            }
            ui.HideTooltip -= HideTooltip;
        }

        protected void ShowEquipmentTootip(IEquipmentData _data)
        {
            equipmentDetailTootip?.ShowDetail(_data);
        }

        protected void ShowMaterialTooltip(IItemData _data)
        {
            materialDetailTooltip?.ShowDetail(_data);
        }

        protected void ShowStatTooltip(EStatType _type)
        {
            statsDetailTooltip?.ShowToolTip(_type);
        }

        protected void ShowWarning(string _text)
        {
            warningToolTip.ShowWarning(_text);
        }

        protected void ShowCraftWindow(IEquipmentData _data)
        {
            craftWindow.Setup(_data);
        }

        protected void HideTooltip()
        {
            if(equipmentDetailTootip!=null && equipmentDetailTootip.gameObject.activeSelf)
            {
                equipmentDetailTootip.HideToolTip();
            }
            if (materialDetailTooltip != null && materialDetailTooltip.gameObject.activeSelf)
            {
                materialDetailTooltip.HideToolTip();
            }
            if(statsDetailTooltip != null && statsDetailTooltip.gameObject.activeSelf)
            {
                statsDetailTooltip.HideToolTip();
            }
            if(warningToolTip != null && warningToolTip.gameObject.activeSelf)
            {
                warningToolTip.Hide();
    }
        }
    }
}
