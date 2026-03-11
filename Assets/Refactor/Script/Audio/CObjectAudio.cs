using ObjectController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioSystem
{
    [Serializable]
    internal struct DObjectAduioSound
    {
        public AudioSource audioSource;
        public EObjectAudioType type;
        public bool canRandomPitch;
    }
    internal class CObjectAudio : CSoundManagerBase, IObjectAudio
    {
        [Header("Environment Sounds")]
        [SerializeField] protected List<DObjectAduioSound> objectAduioList;
        protected Dictionary<EObjectAudioType, TSound> soundDictionary = new();

        private void Start()
        {
            foreach(var data in objectAduioList)
            {
                if(soundDictionary.ContainsKey(data.type))
                {
                    Debug.LogWarning("已存在指定类型音效源，此条目将被忽视: " + data.type.ToString());
                    continue;
                }

                soundDictionary.Add(data.type, GetSound(data.audioSource, data.canRandomPitch));
            }
        }

        public void PlayObjectAudioByType(EObjectAudioType _type, Transform _object, bool _isPlay)
        {
            TSound target;
            if(!soundDictionary.TryGetValue(_type, out target))
            {
                Debug.LogWarning("不存在指定音效: " + _type.ToString());
                return;
            }

            if(_isPlay && !target.IsPlaying())
            {
                target.Play(_object);
            }
            else if(!_isPlay && target.IsPlaying())
            {
                target.Stop();
            }
        }
    }
}

