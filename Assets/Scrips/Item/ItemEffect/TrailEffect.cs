using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stats Trail Effect", menuName = "Item Effect/TrailEffect")]
public class TrailEffect : ItemEffect
{
    [SerializeField] protected DamageDataSerializable selfDamageData;
    [SerializeField] protected bool isMagicEffectUseStatsValue = true;
    [Space]
    [SerializeField] protected float buffDelay;
    [Space]
    [SerializeField] protected StatsModifierData modifierData;
    [SerializeField] protected float buffDuration;

    public override void ExcuteEffect(EffectExcuteData _targetData)
    {
        Player player = PlayerManager.instance.player;

        DamageData data = selfDamageData.GetDamageData();
        player.GetStats().CalculateDamageDataWithStats(ref data, isMagicEffectUseStatsValue);
        player.GetStats().TakeDamage(data, player.transform);

        player.cs.HealAfterDelay(buffDelay, player.cs.GetMaxHealthValue());
        player.cs
            .StartCoroutineAfterDelay(buffDelay, player.cs.ModifyStatsInDurationCoroutine(modifierData, buffDuration));
    }
}
