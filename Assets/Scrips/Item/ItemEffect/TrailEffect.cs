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

    [SerializeField] protected GameObject selfDamagePSFXPrefab;
    [SerializeField] protected GameObject healPSFXPrefab;
    [SerializeField] protected GameObject buffPSFXPrefab;

    public override void ExcuteEffect(EffectExcuteData _targetData)
    {
        Player player = PlayerManager.instance.player;

        DamageData data = selfDamageData.GetDamageData();
        player.GetStats().CalculateDamageDataWithStats(ref data, isMagicEffectUseStatsValue);
        player.GetStats().TakeDamage(data, player.transform);
        PlayPSFX(selfDamagePSFXPrefab, -1, _targetData);

        player.cs.HealAfterDelay(buffDelay, player.cs.GetMaxHealthValue());
        player.cs.StartCoroutine(PlayPSFX_Delay(healPSFXPrefab, -1, _targetData, buffDelay));
        player.cs.StartCoroutineAfterDelay(buffDelay, player.cs.ModifyStatsInDurationCoroutine(modifierData, buffDuration));
        player.cs.StartCoroutine(PlayPSFX_Delay(buffPSFXPrefab, buffDuration, _targetData, buffDelay));
    }

    protected void PlayPSFX(GameObject psfxPrefab, float _lifeTime, EffectExcuteData _targetData)
    {
        ParticleSystemFXEffectController psfxController = 
            Instantiate(psfxPrefab, PlayerManager.instance.player.transform.position, Quaternion.identity)
            .GetComponent<ParticleSystemFXEffectController>();
        psfxController.transform.SetParent(PlayerManager.instance.player.transform);
        psfxController.transform.localPosition = Vector3.zero;
        psfxController.PlayFX(_targetData, _lifeTime, false);
    }

    protected IEnumerator PlayPSFX_Delay(GameObject psfxPrefab, float _lifeTime, EffectExcuteData _targetData, float _delay)
    {
        yield return new WaitForSeconds(_delay);
        PlayPSFX(psfxPrefab, _lifeTime, _targetData);
    }
}
