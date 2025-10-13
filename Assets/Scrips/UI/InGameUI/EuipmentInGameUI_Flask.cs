using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EuipmentInGameUI_Flask : EuipmentInGameUI
{
    [SerializeField] protected TextMeshProUGUI usageTime;
    [SerializeField] protected Slider usageTimeRecoverPercentage;

    public override void EquipmentCoolDownUpdate()
    {
        base.EquipmentCoolDownUpdate();

        ItemData_Equipment curEquip = Inventory.instance.TryGetEquipedEquipmentByType(EquipmentType.Flask);
        if (curEquip != null)
        {
            usageTime.text = ((PlayerManager.instance.player.cs) as PlayerStats).CheckFlaskUsageTimeInInt().ToString();
            usageTimeRecoverPercentage.value = ((PlayerManager.instance.player.cs) as PlayerStats).CheckFlaskUsageTimeRecoverPercentage();
        }
    }
}
