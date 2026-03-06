using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioSystem
{
    internal class CPlayerSFX : CSoundManagerBase, IPlayerAudio
    {
        [Header("Sounds")]
        [SerializeField] private List<AudioSource> attack_SS;
        [SerializeField] private AudioSource rockGround_SS;
        [SerializeField] private AudioSource jump_SS;
        [SerializeField] private AudioSource dash_SS;
        [SerializeField] private AudioSource swordThrow_SS;
        [SerializeField] private AudioSource swordGround_SS;
        [SerializeField] private AudioSource swordCatch_SS;
        [SerializeField] private AudioSource counterAttack_SS;
        [SerializeField] private AudioSource counterAttackSuccess_SS;
        [SerializeField] private AudioSource blackHoleLoop_SS;
        [SerializeField] private AudioSource crystalPlace_SS;
        [SerializeField] private AudioSource crystalFlashBack_SS;
        [SerializeField] private AudioSource crystalExplode_SS;
        [SerializeField] private AudioSource evasionSuccess_SS;
        [SerializeField] private AudioSource playerHit_SS;
        [SerializeField] private AudioSource swordHit_SS;


        private List<TSound> attack = new List<TSound>();
        private TSound ground;
        private TSound jump;
        private TSound dash;
        private TSound swordThrow;
        private TSound swordGround;
        private TSound swordCatch;
        private TSound counterAttack;
        private TSound counterAttackSuccess;
        private TSound blackHoleLoop;
        private TSound crystalPlace;
        private TSound crystalFlashBack;
        private TSound crystalExplode;
        private TSound evasionSuccess;
        private TSound playerTakeHit;
        private TSound swordHit;


        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            for (int i = 0; i < attack_SS.Count; ++i)
            {
                attack.Add(new TSound());
                attack[i] = GetSound(attack_SS[i], true);
            }
            ground = GetSound(rockGround_SS, true);
            jump = GetSound(jump_SS, true);
            dash = GetSound(dash_SS, true);
            swordThrow = GetSound(swordThrow_SS, true);
            swordGround = GetSound(swordGround_SS, true);
            swordCatch = GetSound(swordCatch_SS, true);
            counterAttack = GetSound(counterAttack_SS, true);
            counterAttackSuccess = GetSound(counterAttackSuccess_SS, true);
            blackHoleLoop = GetSound(blackHoleLoop_SS, false);
            crystalPlace = GetSound(crystalPlace_SS, true);
            crystalFlashBack = GetSound(crystalFlashBack_SS, true);
            crystalExplode = GetSound(crystalExplode_SS, true);
            evasionSuccess = GetSound(evasionSuccess_SS, true);
            playerTakeHit = GetSound(playerHit_SS, true);
            swordHit = GetSound(swordHit_SS, true);
        }

        public void Attack(int _count, Transform _source, bool _isPlay)
        {
            if(_isPlay)
            {
                attack[_count].Stop();
                attack[_count].Play(_source);
            }
            else
            {
                attack[_count].Stop();
            }
        }

        public void Ground(Transform _sourceTransform, bool _isPlay)
        {
            if (_isPlay)
            {
                ground.Play(_sourceTransform);
            }
            else
            {
                ground.Stop();
            }
        }

        public void Jump(Transform _sourceTransform, bool _isPlay)
        {
            if (_isPlay)
            {
                jump.Play(_sourceTransform);
            }
            else
            {
                jump.Stop();
            }
        }

        public void Dash(Transform _sourceTransform, bool _isPlay)
        {
            if (_isPlay)
            {
                dash.Play(_sourceTransform);
            }
            else
            {
                dash.Stop();
            }
        }

        public void SwordThrow(Transform _sourceTransform, bool _isPlay)
        {
            if (_isPlay)
            {
                swordThrow.Play(_sourceTransform);
            }
            else
            {
                swordThrow.Stop();
            }
        }

        public void SwordGround(Transform _sourceTransform, bool _isPlay)
        {
            if (_isPlay)
            {
                swordGround.Play(_sourceTransform);
            }
            else
            {
                swordGround.Stop();
            }
        }

        public void SwordCatch(Transform _sourceTransform, bool _isPlay)
        {
            if (_isPlay)
            {
                swordCatch.Play(_sourceTransform);
            }
            else
            {
                swordCatch.Stop();
            }
        }

        public void CounterAttack(Transform _sourceTransform, bool _isPlay)
        {
            if (_isPlay)
            {
                counterAttack.Play(_sourceTransform);
            }
            else
            {
                counterAttack.Stop();
            }
        }

        public void CounterAttackSuccess(Transform _sourceTransform, bool _isPlay)
        {
            if (_isPlay)
            {
                counterAttackSuccess.Play(_sourceTransform);
            }
            else
            {
                counterAttackSuccess.Stop();
            }
        }

        public void BlackHoleLoop(Transform _sourceTransform, bool _isPlay)
        {
            if (_isPlay)
            {
                blackHoleLoop.Play(_sourceTransform);
            }
            else
            {
                blackHoleLoop.Stop();
            }
        }

        public void CrystalPlace(Transform _sourceTransform, bool _isPlay)
        {
            if (_isPlay)
            {
                crystalPlace.Play(_sourceTransform);
            }
            else
            {
                crystalPlace.Stop();
            }
        }

        public void CrystalFlashBack(Transform _sourceTransform, bool _isPlay)
        {
            if (_isPlay)
            {
                crystalFlashBack.Play(_sourceTransform);
            }
            else
            {
                crystalFlashBack.Stop();
            }
        }

        public void CrystalExplode(Transform _sourceTransform, bool _isPlay)
        {
            if (_isPlay)
            {
                crystalExplode.Play(_sourceTransform);
            }
            else
            {
                crystalExplode.Stop();
            }
        }

        public void EvasionSuccess(Transform _sourceTransform, bool _isPlay)
        {
            if (_isPlay)
            {
                evasionSuccess.Play(_sourceTransform);
            }
            else
            {
                evasionSuccess.Stop();
            }
        }

        public void PlayerTakeHit(Transform _sourceTransform, bool _isPlay)
        {
            if (_isPlay)
            {
                playerTakeHit.Play(_sourceTransform);
            }
            else
            {
                playerTakeHit.Stop();
            }
        }

        public void SwordHit(Transform _sourceTransform, bool _isPlay)
        {
            if (_isPlay)
            {
                swordHit.Play(_sourceTransform);
            }
            else
            {
                swordHit.Stop();
            }
        }
    }
}

