using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EuipmentInGameUI : MonoBehaviour
{
    [SerializeField] protected Image equipmentCoolDown;
    [SerializeField] protected EquipmentType equipmentType;

    public virtual void EquipmentCoolDownUpdate()
    {
        ItemData_Equipment curEquip = Inventory.instance.TryGetEquipedEquipmentByType(equipmentType);
        if(curEquip == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            GetComponent<Image>().sprite = curEquip.icon;
            equipmentCoolDown.sprite = curEquip.icon;
            equipmentCoolDown.fillAmount = curEquip.CheckCooldownPercentage();
        }
    }
}
