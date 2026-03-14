using InventorySystem;
using Item;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace UISystem
{
    internal class CCraftWindowUI : CUIComponentBase
    {
        [SerializeField] protected string craftSuccessText = "制造成功";
        [SerializeField] protected string craftFailText = "制造失败，缺少材料：";
        [SerializeField] protected string craftFailText_separator = "、";
        [SerializeField] protected string CannotDetachBlacksmithText = "无法在附近没有铁匠的情况下制作物品";

        protected IEquipmentData data;
        protected CEquipmentDetailUI detail;
        protected CCraftMaterialBlockUI materials;
        protected Button craftButton;
        protected StringBuilder sb;

        public void Setup(IEquipmentData _equipment)
        {
            if (_equipment == null)
            {
                return;
            }

            gameObject.SetActive(true);

            if (detail == null)
            {
                detail = GetComponentInChildren<CEquipmentDetailUI>();
            }
            if(materials == null)
            {
                materials = GetComponentInChildren<CCraftMaterialBlockUI>();
            }
            if(craftButton == null)
            {
                craftButton = GetComponentInChildren<Button>();
            }

            data = _equipment;

            materials.DisplayEquipmentCraftMaterials(_equipment.CheckCraftingMaterials());
            detail.ShowDetail(_equipment);
            craftButton.onClick.AddListener(TryCraft);
        }

        protected void TryCraft()
        {
            if(!ui.CheckCanCraft_BlackSmith())
            {
                ui.InvokeAction(ui.ShowWarning, CannotDetachBlacksmithText);
                return;
            }

            IReadOnlyList<IItemData> lackList = ui.InvokeFunc(ui.TryCraft, data);
            if (lackList == null)
            {
                ui.InvokeAction(ui.ShowWarning, craftSuccessText);
            }
            else
            {
                if (sb == null)
                {
                    sb = new();
                }

                sb.Append(craftFailText);
                foreach (var lack in lackList)
                {
                    sb.Append(lack.CheckItemName());
                    sb.Append(craftFailText_separator);
                }
                --sb.Length;
                ui.InvokeAction(ui.ShowWarning, sb.ToString());
                sb.Clear();
            }
        }
    }
}
