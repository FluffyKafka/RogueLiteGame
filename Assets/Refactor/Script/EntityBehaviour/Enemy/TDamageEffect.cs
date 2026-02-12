using EntityBehaviour;
using EntitySystem;
using StatsData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyBehaviour
{
    internal abstract class TDamageEffect : MonoBehaviour
    {
        protected MEnemyBehaviour behaviour;

        protected void Awake()
        {
            behaviour = GetComponentInParent<MEnemyBehaviour>();
        }

        public abstract void EffectDamage(WReadOnlyDamageData _damage, EEntityType _target);
    }
}

