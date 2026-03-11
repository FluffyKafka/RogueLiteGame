using ObjectController;
using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioSystem
{
    public interface IInitAudio
    {
        public void Init(IAudioPlayer _player);
    }
    //只负责管理通用音效相关设置项，此GameObject下将挂一系列的具体音效系统，外界使用时用GetComponet对应接口从此处获取自己的对应音效库
    internal class MAudioManager : MonoBehaviour, IInitAudio
    {
        [SerializeField] public TSound soundPrfab;
        [SerializeField] public float maxAudibleDistance;
        [SerializeField] public float minAudibleDistance;
        [SerializeField] public float pitchRandomRange;

        protected IAudioPlayer player;
        
        public void Init(IAudioPlayer _player)
        {
            player = _player;
        }

        public Transform CheckPlayerTransform()
        {
            return player.CheckTransform();
        }

        public bool CheckIsPlayerInBattle()
        {
            return player.CheckIsPlayerInBattle();
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

