using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SelfDamage Effect", menuName = "Item Effect/SelfDamage")]
public class SelfDamage : DamageEffect
{
    [SerializeField] private GameObject psfxPrefab;

    public override void ExcuteEffect(EffectExcuteData _target)
    {
        DamageData data = CulculateDamageDamage();
        Player player = PlayerManager.instance.player;
        player.GetStats().TakeDamage(data, player.transform);

        ParticleSystemFXEffectController psfxController =
            Instantiate(psfxPrefab, PlayerManager.instance.player.transform.position, Quaternion.identity)
            .GetComponent<ParticleSystemFXEffectController>();
        psfxController.transform.SetParent(PlayerManager.instance.player.transform);
        psfxController.transform.localPosition = new Vector3(0, 0, 0);
        psfxController.PlayFX(_target, -1, false);
    }
}
