using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UISystem
{
    internal class SLCraftMaterialSlot : SLItemDataDisplaySlot, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(item is IEquipmentData)
            {
                ui.InvokeAction(ui.ShowEquipmentDetail, item as IEquipmentData);
            }
            else
            {
                ui.InvokeAction(ui.ShowMaterialDetail, item);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ui.InvokeAction(ui.HideTooltip);
        }
    }
}

