using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stats Increase Buff Effect", menuName = "Item Effect/Stats Increase Buff")]
public class StatsIncreaseBuff_Effect : ItemEffect
{
    [SerializeField] protected StatsModifierData modifierData;
    [SerializeField] protected float buffDuration;
    [SerializeField] protected GameObject psfxPrefab;
    public override void ExcuteEffect(EffectExcuteData _target)
    {
        PlayerManager.instance.player.cs.StartCoroutine(PlayerManager.instance.player.cs.ModifyStatsInDurationCoroutine(modifierData, buffDuration));

        if(psfxPrefab != null)
        {
            GameObject newPSFX = Instantiate(psfxPrefab, PlayerManager.instance.player.transform.position, Quaternion.identity);
            newPSFX.transform.SetParent(PlayerManager.instance.player.transform);
            ParticleSystemFXEffectController newPSFXController = newPSFX.GetComponent<ParticleSystemFXEffectController>();
            newPSFXController.PlayFX(_target, buffDuration, false);
        }
    }
}
