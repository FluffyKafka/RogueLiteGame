using EnemySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioSystem
{
    internal class CEnemySFX : CSoundManagerBase, IEnemyAduio
    {
        [SerializeField] protected EEnemyType type;

        [Header("Sounds")]
        [SerializeField] protected AudioSource roar_SS;
        [SerializeField] protected AudioSource attack_SS;
        [SerializeField] protected AudioSource effect_SS;
        [SerializeField] protected AudioSource bulletEffect_SS;
        [SerializeField] protected AudioSource takeHit_SS;

        protected TSound roar;
        protected TSound attack;
        protected TSound effect;
        protected TSound bulletEffect;
        protected TSound takeHit;

        protected void Start()
        {
            roar = GetSound(roar_SS, true);
            attack = GetSound(attack_SS, true);
            effect = GetSound(effect_SS, true);
            bulletEffect = GetSound(bulletEffect_SS, true);
            takeHit = GetSound(takeHit_SS, true);
        }

        public void Attack(Transform _enemy, bool _isPlay)
        {
            if (_isPlay)
            {
                attack?.Play(_enemy);
            }
            else
            {
                attack?.Stop();
            }
        }

        public void BulletEffect(Transform _bullet, bool _isPlay)
        {
            if (_isPlay)
            {
                bulletEffect?.Play(_bullet);
            }
            else
            {
                bulletEffect?.Stop();
            }
        }

        public void Effect(Transform _enemy, bool _isPlay)
        {
            if (_isPlay)
            {
                effect?.Play(_enemy);
            }
            else
            {
                effect?.Stop();
            }
        }

        public void Roar(Transform _enemy, bool _isPlay)
        {
            if (_isPlay)
            {
                roar?.Play(_enemy);
            }
            else
            {
                roar?.Stop();
            }
        }

        public void TakeHit(Transform _enemy, bool _isPlay)
        {
            if (_isPlay)
            {
                takeHit?.Play(_enemy);
            }
            else
            {
                takeHit?.Stop();
            }
        }

        public EEnemyType CheckEnemyType()
        {
            return type;
        }
    }
}


