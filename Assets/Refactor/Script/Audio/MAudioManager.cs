using ObjectController;
using PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.Rendering.DebugUI;

namespace AudioSystem
{
    public interface IInitAudio
    {
        public void Init(IAudioPlayer _player);
    }

    public interface ISaveAduio
    {
        public class DAudioSaveData
        {
            public float sfxVolume;
            public float bgmVolume;
            public float envVolume;
        }
        public void Save(ref DAudioSaveData _data);
        public void Load(DAudioSaveData _data);
    }

    [Serializable]
    public class DAudioData
    {
        public EAudioType type;
        public string parameter;
        public float multiplier;
        public float volume;
    }

    //只负责管理通用音效相关设置项，此GameObject下将挂一系列的具体音效系统，外界使用时用GetComponet对应接口从此处获取自己的对应音效库
    internal class MAudioManager : MonoBehaviour, IInitAudio, IPlayerAudioManager, ISaveAduio
    {
        [SerializeField] public TSound soundPrfab;
        [SerializeField] public float maxAudibleDistance;
        [SerializeField] public float minAudibleDistance;
        [SerializeField] public float pitchRandomRange;

        [Header("Audio Mix")]
        [SerializeField] protected AudioMixer audioMixer;
        [SerializeField] protected List<DAudioData> audioData;

        protected IAudioPlayer player;

        private void Awake()
        {
            
        }

        public void Init(IAudioPlayer _player)
        {
            player = _player;

        }

        public Transform CheckPlayerTransform()
        {
            if(player == null)
            {
                return transform;
            }
            return player.GetTransform();
        }

        public bool CheckIsPlayerInBattle()
        {
            if(player == null)
            {
                return false;
            }
            return player.CheckIsPlayerInBattle();
        }

        public void UpdateAudioVolumeByType(EAudioType _type, float _volume)
        {
            foreach(var audio in audioData)
            {
                if(audio.type == _type)
                {
                    audio.volume = _volume;
                    audioMixer.SetFloat(audio.parameter, (1 + Mathf.Log(_volume + 0.00000001f)) * audio.multiplier);
                    return;
                }
            }
        }

        public void Save(ref ISaveAduio.DAudioSaveData _data)
        {
            foreach(var audio in audioData)
            {
                if(audio.type == EAudioType.SFX)
                {
                    _data.sfxVolume = (int)audio.volume;
                }
                else if (audio.type == EAudioType.BGM)
                {
                    _data.bgmVolume = (int)audio.volume;
                }
                else if (audio.type == EAudioType.ENV)
                {
                    _data.envVolume = (int)audio.volume;
                }
            }
        }
        public void Load(ISaveAduio.DAudioSaveData _data)
        {
            foreach (var audio in audioData)
            {
                if (audio.type == EAudioType.SFX)
                {
                    audio.volume = _data.sfxVolume;
                    player.UpdateAduioVolumeByTypeToUi(EAudioType.SFX, _data.sfxVolume);
                }
                else if (audio.type == EAudioType.BGM)
                {
                    audio.volume = _data.bgmVolume;
                    player.UpdateAduioVolumeByTypeToUi(EAudioType.BGM, _data.bgmVolume);
                }
                else if (audio.type == EAudioType.ENV)
                {
                    audio.volume = _data.envVolume;
                    player.UpdateAduioVolumeByTypeToUi(EAudioType.ENV, _data.envVolume);
                }
            }
        }
    }

    internal class DAudioSourceDefault
    {
        public float pitch { get; private set; }
        public float volume { get; private set; }

        public DAudioSourceDefault(AudioSource _source)
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
}

