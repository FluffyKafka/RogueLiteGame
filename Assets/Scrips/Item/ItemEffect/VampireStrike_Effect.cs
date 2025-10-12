using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Vampire Strike Effect", menuName = "Item Effect/Vampire Strike")]
public class VampireStrike_Effect : ItemEffect
{
    [Range(0f, 1f)][SerializeField] private float healPercentage;
    [SerializeField] private GameObject psfxPrefab;
    public override void ExcuteEffect(EffectExcuteData _data)
    {
        PlayerManager.instance.player.cs.Heal(healPercentage * _data.damage);

        ParticleSystemFXEffectController psfxController = 
            Instantiate(psfxPrefab, PlayerManager.instance.player.transform.position, Quaternion.identity)
            .GetComponent<ParticleSystemFXEffectController>();
        psfxController.transform.SetParent(PlayerManager.instance.player.transform);
        psfxController.transform.localPosition = new Vector3(0, 0, 0);
        psfxController.PlayFX(_data, -1, true);
    }
}
