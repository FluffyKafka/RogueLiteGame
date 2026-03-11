using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum BGMType
{
    Normal,
    Fight
}
public class BGMManager : SoundManagerBase
{
    [SerializeField] private AudioSource[] normalBGMList;
    [SerializeField] private AudioSource[] fightBGMList;

    [SerializeField] public bool isPlayBGM;
    [SerializeField] public BGMType type;
    [SerializeField] private int bgmIndex;
    private float currentBGMDefaultVolume = 1;

    private void Update()
    {
        if (!isPlayBGM)
        {
            StopAllBGM();
        }
        else if (!GetAudioSourceListByType(type)[bgmIndex].isPlaying)
        {
            currentBGMDefaultVolume = GetAudioSourceListByType(type)[bgmIndex].volume;
            PlayBGMOfType(type, bgmIndex);
        }
    }

    private AudioSource[] GetAudioSourceListByType(BGMType _type)
    {
        switch (_type)
        {
            case BGMType.Normal: return normalBGMList;
            case BGMType.Fight:  return fightBGMList;
            default: return null;
        }
    }

    public void SetRandomBGMOfTypeInRange(BGMType _type, int _begin, int _end)
    {
        int randomIndex = Random.Range(_begin, _end);
        SetBGM(_type, randomIndex);
    }

    public void SetRandomBGMOfType(BGMType _type)
    {
        int maxIndex;
        switch(_type)
        {
            case BGMType.Normal: 
                maxIndex = normalBGMList.Length; 
                break;
            case BGMType.Fight: 
                maxIndex = fightBGMList.Length;  
                break;
            default:
                maxIndex = -1;
                Assert.IsFalse(true, "未定义的bgm类型：" + _type.ToString());                    
                break;
        }

        int randomIndex = Random.Range(0, maxIndex);
        SetBGM(_type, randomIndex);
    }

    public void SetBGM(BGMType _type, int _index)
    {
        GetAudioSourceListByType(type)[bgmIndex].volume = currentBGMDefaultVolume;

        bgmIndex = _index;
        type = _type;
        currentBGMDefaultVolume = GetAudioSourceListByType(type)[bgmIndex].volume;
    }

    public void SetCurrentBGMVolume(float _volume)
    {
        if(_volume < 0)
        {
            return;
        }

        GetAudioSourceListByType(type)[bgmIndex].volume = currentBGMDefaultVolume * _volume;
    }

    public float GetCurrentBGMVolume()
    {
        return currentBGMDefaultVolume;
    }

    private void PlayBGMOfType(BGMType _type, int _bgmIndex)
    {
        if (_bgmIndex < GetAudioSourceListByType(_type).Length)
        {
            bgmIndex = _bgmIndex;
            StopAllBGM();
            GetAudioSourceListByType(_type)[_bgmIndex].Play();
        }
    }

    protected void StopAllBGM()
    {
        foreach (var bgm in normalBGMList)
        {
            bgm.Stop();
        }

        foreach (var bgm in fightBGMList)
        {
            bgm.Stop();
        }
    }
}
