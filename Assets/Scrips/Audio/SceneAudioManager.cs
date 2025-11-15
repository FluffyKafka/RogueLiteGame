using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceDefault
{
    public float pitch { get; private set; }
    public float volume { get; private set; }

    public AudioSourceDefault(AudioSource _source)
    {
        pitch = _source.pitch;
        volume = _source.volume;
    }

    public void SetDefault(ref AudioSource _target)
    {
        _target.volume = volume;
        _target.pitch = pitch;
    }
}

public class SceneAudioManager : MonoBehaviour
{
    public static SceneAudioManager instance;
    [SerializeField] public GameObject soundPrfab;
    [SerializeField] public float maxAudibleDistance;
    [SerializeField] public float minAudibleDistance;
    [SerializeField] public float pitchRandomRange;

    #region Sounds
    public BGMManager bgm { get; private set; }
    public EnvSoundManager env { get; private set; }
    public PlayerSFX playerSFX { get; private set; }
    public ItemSFX itemSFX { get; private set; }
    public UISFX uiSFX { get; private set; }
    public SkeletonSFX skeletonSFX { get; private set; }
    public ArcherSFX archerSFX { get; private set; }
    public BombermanSFX bombermanSFX { get; private set; }
    public MimicSFX mimicSFX { get; private set; }
    public NecromancerSFX necromancerSFX { get; private set; }
    public SlimeSFX slimeSFX { get; private set; }
    public DeathBrinerSFX deathBrinerSFX { get; private set; }
    #endregion

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        bgm = GetComponent<BGMManager>();
        env = GetComponent<EnvSoundManager>();
        playerSFX = GetComponent<PlayerSFX>();
        itemSFX = GetComponent<ItemSFX>();
        uiSFX = GetComponent<UISFX>();
        skeletonSFX = GetComponent<SkeletonSFX>();
        archerSFX = GetComponent<ArcherSFX>();
        bombermanSFX = GetComponent<BombermanSFX>();
        mimicSFX = GetComponent<MimicSFX>();
        necromancerSFX = GetComponent<NecromancerSFX>();
        slimeSFX = GetComponent<SlimeSFX>();
        deathBrinerSFX = GetComponent<DeathBrinerSFX>();
    }
}
