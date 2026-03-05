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
        private TSound playerHit;
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
            playerHit = GetSound(playerHit_SS, true);
            swordHit = GetSound(swordHit_SS, true);
        }

        public void Attack(int _count, Transform _source = null)
        {
            attack[_count].Stop();
            attack[_count].Play(_source);
        }

        public void Ground(Transform _sourceTransform)
        {
            ground.Play(_sourceTransform);
        }
        public void Jump(Transform _sourceTransform)
        {
            jump.Play(_sourceTransform);
        }

        public void Dash(Transform _sourceTransform)
        {
            dash.Play(_sourceTransform);
        }

        public void SwordThrow(Transform _sourceTransform)
        {
            swordThrow.Play(_sourceTransform);
        }

        public void SwordGround(Transform _sourceTransform)
        {
            swordGround.Play(_sourceTransform);
        }

        public void SwordCatch(Transform _sourceTransform)
        {
            swordCatch.Play(_sourceTransform);
        }

        public void CounterAttack(Transform _sourceTransform)
        {
            counterAttack.Play(_sourceTransform);
        }

        public void CounterAttackSuccess(Transform _sourceTransform)
        {
            counterAttackSuccess.Play(_sourceTransform);
        }

        public void BlackHoleLoop(Transform _sourceTransform)
        {
            blackHoleLoop.Play(_sourceTransform);
        }

        public void CrystalPlace(Transform _sourceTransform)
        {
            crystalPlace.Play(_sourceTransform);
        }

        public void CrystalFlashBack(Transform _sourceTransform)
        {
            crystalFlashBack.Play(_sourceTransform);
        }

        public void CrystalExplode(Transform _sourceTransform)
        {
            crystalExplode.Play(_sourceTransform);
        }

        public void EvasionSuccess(Transform _sourceTransform)
        {
            evasionSuccess.Play(_sourceTransform);
        }

        public void PlayerHit(Transform _sourceTransform)
        {
            playerHit.Play(_sourceTransform);
        }

        public void SwordHit(Transform _sourceTransform)
        {
            swordHit.Play(_sourceTransform);
        }
    }
}

