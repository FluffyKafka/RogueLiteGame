using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageStroeController : MonoBehaviour
{
    private float storeTime;
    private float releaseTime;
    private float releaseCooldown;

    private bool isReleasing = false;
    private float damageStored = 0;
    private float lastHp;
    private Player player;
    private DamageData damageRelease = new DamageData();
    private float releaseTimer;

    [SerializeField] private GameObject damageStorePSFXPrefab;
    [SerializeField] private GameObject damageReleasePSFXPrefab;

    public void Setup(float _storeTime, float _releaseTime, float _releaseCooldown)
    {
        player = PlayerManager.instance.player;
        lastHp = player.GetStats().GetCurrentHealthValue();
        storeTime = _storeTime;
        releaseTime = _releaseTime;
        releaseCooldown = _releaseCooldown;

        DamageStorePSFX();
    }

    private void Update()
    {
        transform.position = player.transform.position;
        if(!isReleasing)
        {
            float currentHp = player.GetStats().GetCurrentHealthValue();
            if (currentHp < lastHp)
            {
                player.GetStats().Heal(lastHp - currentHp);
                damageStored += lastHp - currentHp;
            }
            else
            {
                lastHp = currentHp;
            }

            storeTime -= Time.deltaTime;
            if(storeTime < 0)
            {
                isReleasing = true;
                int releaseCount = (int)(releaseTime / releaseCooldown);
                float releaseSpeed = damageStored / releaseCount;
                damageRelease.SetPhysicsDamage(releaseSpeed, false);
                releaseTimer = releaseCooldown;
                DamageReleasePSFX();
            }
        }
        else
        {
            releaseTimer -= Time.deltaTime;
            if(releaseTimer < 0)
            {
                player.GetStats().TakeDamage(damageRelease, player.transform);
                releaseTimer = releaseCooldown;
            }

            releaseTime -= Time.deltaTime;
            if(releaseTime < 0)
            {
                SelfDestory();
            }
        }
    }

    private void DamageStorePSFX()
    {
        GameObject damageStorePSFXObject = Instantiate(damageStorePSFXPrefab, PlayerManager.instance.player.transform.position, Quaternion.identity);
        damageStorePSFXObject.transform.SetParent(PlayerManager.instance.player.transform);
        damageStorePSFXObject.transform.localPosition = new Vector3(0, 0, 0);
        ParticleSystemFXEffectController controller = damageStorePSFXObject.GetComponent<ParticleSystemFXEffectController>();
        controller.PlayFX(null, storeTime, false);
    }

    private void DamageReleasePSFX()
    {
        GameObject damageReleasePSFXObject = Instantiate(damageReleasePSFXPrefab, PlayerManager.instance.player.transform.position, Quaternion.identity);
        damageReleasePSFXObject.transform.SetParent(PlayerManager.instance.player.transform);
        damageReleasePSFXObject.transform.localPosition = new Vector3(0, 0, 0);
        ParticleSystemFXEffectController controller = damageReleasePSFXObject.GetComponent<ParticleSystemFXEffectController>();
        controller.PlayFX(null, releaseTime, false);
    }

    private void SelfDestory()
    {

        Destroy(gameObject);
    }
}
