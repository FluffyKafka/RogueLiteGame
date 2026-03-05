using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioSystem
{
    internal class TSound : MonoBehaviour
    {
        private AudioSource sound;
        private DAudioSourceDefault soundDefalut;
        private Coroutine decreaseCoroutine;
        private bool isRandomPitch;
        private MAudioManager manager;

        public void Setup(AudioSource _envSound, bool _isRandomPitch, MAudioManager _manager)
        {
            sound = _envSound;
            soundDefalut = new DAudioSourceDefault(_envSound);
            decreaseCoroutine = null;
            isRandomPitch = _isRandomPitch;
            manager = _manager;
        }

        public void Play(Transform _soundSourceTransform)
        {
            soundDefalut.SetDefault(ref sound);

            if (_soundSourceTransform != null)
            {
                Transform player = manager.CheckPlayerTransform();
                if (Vector2.Distance(_soundSourceTransform.position, player.position) > manager.maxAudibleDistance)
                {
                    return;
                }
            }

            if (decreaseCoroutine != null)
            {
                StopCoroutine(decreaseCoroutine);
                decreaseCoroutine = null;
                sound.volume = soundDefalut.volume;
            }

            sound.pitch = soundDefalut.pitch;
            if (isRandomPitch)
            {
                sound.pitch = Random.Range(
                    sound.pitch - manager.pitchRandomRange,
                    sound.pitch + - manager.pitchRandomRange
                );
            }

            if (_soundSourceTransform != null)
            {
                Transform player = manager.CheckPlayerTransform();
                float sourceDistance = Vector2.Distance(_soundSourceTransform.position, player.position);
                if (sourceDistance > manager.minAudibleDistance && sourceDistance < manager.maxAudibleDistance)
                {
                    sound.volume *=
                        (manager.maxAudibleDistance - sourceDistance) /
                        (manager.maxAudibleDistance - manager.minAudibleDistance);
                }
            }

            sound.Play();
        }

        public void Stop()
        {
            if (decreaseCoroutine != null)
            {
                StopCoroutine(decreaseCoroutine);
                decreaseCoroutine = null;
                sound.volume = soundDefalut.volume;
            }
            sound.Stop();
        }

        public void StopWithinTime(float duration = 1f, float _smooth = 0.1f)
        {
            if (sound != null && decreaseCoroutine == null)
            {
                decreaseCoroutine = StartCoroutine(DecreaseVolume(duration, _smooth));
            }
        }
        private IEnumerator DecreaseVolume(float _time, float _smooth)
        {
            float decreaseSpeed = _smooth / _time;
            while (sound.volume > 0.1f)
            {
                sound.volume -= sound.volume * decreaseSpeed;
                yield return new WaitForSeconds(_smooth);

                if (sound.volume < 0.1f)
                {
                    sound.Stop();
                    sound.volume = soundDefalut.volume;
                    decreaseCoroutine = null;
                    break;
                }
            }
        }

        public bool IsPlaying()
        {
            return sound.isPlaying;
        }

        public void SetVolume(float _volume)
        {
            sound.volume = soundDefalut.volume * _volume;
        }
    }
}

