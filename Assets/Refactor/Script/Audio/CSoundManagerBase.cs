using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioSystem
{
    internal class CSoundManagerBase : MonoBehaviour
    {
        protected TSound soundPrefab;
        protected MAudioManager manager;

        protected virtual void Awake()
        {
            manager = GetComponent<MAudioManager>();
            soundPrefab = manager.soundPrfab;           
        }
        protected virtual TSound GetSound(AudioSource _ss, bool _isRandomPitch)
        {
            if(_ss == null)
            {
                return null;
            }
            TSound sound = Instantiate(soundPrefab, transform).GetComponent<TSound>();
            sound.Setup(_ss, _isRandomPitch, manager);
            return sound;
        }
    }
}

