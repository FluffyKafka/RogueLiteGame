using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private Player player;
    private Entity entity;

    [Header("Falsh FX")]
    [SerializeField] private Material hitMat;
    [SerializeField] private float fleshDuration = 0.3f;
    public Material originalMat;

    [Header("Stunned FX")]
    [SerializeField] public float stunBlinkRate;

    [Header("Aliment FX")]
    [SerializeField] private Color chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;
    [SerializeField] private float shockFlashRate = 0.1f;
    private float chillDuration;
    private float shockDuration;
    [SerializeField] private float igniteFlashRate = 0.1f;
    private float igniteDuration;
    private bool isIgnite;
    private bool isChill;
    [SerializeField] private bool isShock;
    [Space]
    [SerializeField] private ParticleSystem igniteParticleFX;
    [SerializeField] private ParticleSystem chillParticleFX;
    [SerializeField] private ParticleSystem shockParticleFX;

    [Header("Hit FX")]
    [SerializeField] private GameObject hitFXPrefab;
    [SerializeField] private Vector2 maxXYRandomPositionOffset_Hit;
    [SerializeField] private GameObject hitCritFXPrefab;
    [SerializeField] private Vector2 maxXYRandomPositionOffset_CritHit;
    [SerializeField] private float rotateAngle_CritHit;

    [Header("After Image FX")]
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private float colorLooseSpeed;
    [SerializeField] public float afterImageGenerateCooldown;

    [Header("Screen Shake FX")]
    [SerializeField] public float shakeMultiplier;
    [SerializeField] public Vector2 shakePower_sword;
    private CinemachineImpulseSource screenShake;

    [Header("PopUp Text")]
    [SerializeField] GameObject popUpTextPrefab;
    [SerializeField] float popTextPosition_minOffsetX = -1;
    [SerializeField] float popTextPosition_maxOffsetX = 1;
    [SerializeField] float popTextPosition_minOffsetY = 3;
    [SerializeField] float popTextPosition_maxOffsetY = 5;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
        player = PlayerManager.instance.player;
        entity = GetComponent<Entity>();
        screenShake = GetComponent<CinemachineImpulseSource>();
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        sr.color = Color.white;
        yield return new WaitForSeconds(fleshDuration);
        sr.material = originalMat;

        if(isChill)
        {
            Invoke("ChillColorFX", 0);
        }
    }

    private void RedColerBlink()
    {
        if (sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        else
        {
            sr.color = Color.red;
        }
    }

    private void CancelColerBlink()
    {
        CancelInvoke();
        sr.color = Color.white;
        ReInvokeMagicFX();
    }

    private void ReInvokeMagicFX()
    {
        if (isChill)
        {
            Invoke("ChillColorFX", 0);
            Invoke("CancelColerBlink_MagicFX", chillDuration);
        }
        if (isShock)
        {
            InvokeRepeating("ShockColorFX", 0, shockFlashRate);
            Invoke("CancelColerBlink_MagicFX", shockDuration);
        }
        if (isIgnite)
        {
            InvokeRepeating("IgniteColorFX", 0, igniteFlashRate);
            Invoke("CancelColerBlink_MagicFX", igniteDuration);
        }
    }

    private void CancelColerBlink_MagicFX()
    {
        CancelInvoke();
        sr.color = Color.white;
        isChill = false;
        isIgnite = false;
        isShock = false;

        igniteParticleFX.Stop();
        chillParticleFX.Stop();
        shockParticleFX.Stop();
    }

    public void IgniteFX(float _igniteDuration)
    {
        isIgnite = true;
        igniteDuration = _igniteDuration;
        InvokeRepeating("IgniteColorFX", 0, igniteFlashRate);
        Invoke("CancelColerBlink_MagicFX", _igniteDuration);
        igniteParticleFX.Play();
    }
    private void IgniteColorFX()
    {
        if(sr.color != igniteColor[0])
        {
            sr.color = igniteColor[0];
        }
        else
        {
            sr.color = igniteColor[1];
        }
    }

    public void ChillFX(float _chillDuration)
    {
        isChill = true;
        chillDuration = _chillDuration;
        Invoke("ChillColorFX", 0);
        Invoke("CancelColerBlink_MagicFX", _chillDuration);
        chillParticleFX.Play();
    }
    private void ChillColorFX()
    {
        sr.color = chillColor;
    }

    public void ShockFX(float _shockDuration)
    {
        isShock = true;
        shockDuration = _shockDuration;
        InvokeRepeating("ShockColorFX", 0, shockFlashRate);
        Invoke("CancelColerBlink_MagicFX", _shockDuration);
        shockParticleFX.Play();
    }
    private void ShockColorFX()
    {
        if (sr.color != shockColor[0])
        {
            sr.color = shockColor[0];
        }
        else
        {
            sr.color = shockColor[1];
        }
    }

    public void CreateHitFX(Entity _source, Entity _target)
    {
        GameObject newHitFX = 
            Instantiate(
                hitFXPrefab, 
                _target.transform.position + new Vector3(
                    Random.Range(-1, 1) * maxXYRandomPositionOffset_Hit.x,
                    Random.Range(-1, 1) * maxXYRandomPositionOffset_Hit.y
                ),
                Quaternion.identity,
                _target.transform
            );
        newHitFX.transform.Rotate(0, 0, Random.Range(0, 180));
        Destroy(newHitFX, 0.5f);
    }

    public void CreateCritHitFX(Entity _source, Entity _target, Transform _damageDir)
    {
        GameObject newHitFX =
            Instantiate(
                hitCritFXPrefab,
                _target.transform.position + new Vector3(
                    Random.Range(-1, 1) * maxXYRandomPositionOffset_CritHit.x,
                    Random.Range(-1, 1) * maxXYRandomPositionOffset_CritHit.y
                ),
                Quaternion.identity,
                _target.transform
            );
        newHitFX.transform.Rotate(0, 0, Random.Range(-rotateAngle_CritHit, rotateAngle_CritHit));
        if(_damageDir.position.x > _target.transform.position.x)
        {
            newHitFX.transform.Rotate(0, 180, 0);
        }
        Destroy(newHitFX, 0.5f);
    }

    //此处的isFacingLeft参数针对的时初始面向右边的实体，若初始面向左边，应为isFacingRight
    public void CreateAfterImage(bool _isFacingLeft)
    {
        AfterImageFX newAfterImage = Instantiate(afterImagePrefab, transform.position, Quaternion.identity).GetComponent<AfterImageFX>();
        newAfterImage.Setup(colorLooseSpeed, sr.sprite, _isFacingLeft);
    }

    public void ScreenShake(Vector2 _shakePower)
    {
        screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * player.facingDir, _shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();
    }

    public void CreatePopUpText(string _text)
    {
        float randomX = Random.Range(popTextPosition_minOffsetX, popTextPosition_maxOffsetX);
        float randomY = Random.Range(popTextPosition_minOffsetY, popTextPosition_maxOffsetY);
        Vector3 positionOffset = new Vector3(randomX, randomY);

        PopUpText popUpText = Instantiate(popUpTextPrefab, transform.position + positionOffset, Quaternion.identity).GetComponent<PopUpText>();

        popUpText.SetUp(_text);
    }
}
