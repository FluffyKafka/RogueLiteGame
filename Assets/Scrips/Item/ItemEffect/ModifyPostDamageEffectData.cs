using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ModifyPostDamageEffectData Effect", menuName = "Item Effect/Modify Post Damage Effect Data")]
public class ModifyPostDamageEffectData : ItemEffect
{
    [SerializeField] private PostDamageEffectData postDamageEffectData;
    [SerializeField] private float duration;
    [SerializeField] private GameObject activePSFXPrefab;
    public override void ExcuteEffect(EffectExcuteData _targetData)
    {
        (PlayerManager.instance.player.GetStats() as PlayerStats).ModifyPostDamageEffectDataInTime(postDamageEffectData, duration);

        ParticleSystemFXEffectController psfxController =
            Instantiate(activePSFXPrefab, PlayerManager.instance.player.transform.position, Quaternion.identity)
            .GetComponent<ParticleSystemFXEffectController>();
        psfxController.transform.SetParent(PlayerManager.instance.player.transform);
        psfxController.transform.localPosition = new Vector3(0, 0, 0);
        psfxController.PlayFX(_targetData, duration, false);
    }
}
