using EntitySystem;
using StatsData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectGenerateData
{
    public struct DAfterImageData
    {
        public Sprite image;
        public Vector3 position;
        public float duration;
        public int facingDir;
        public float fadeSpeed;
        public DAfterImageData(Sprite _image, Vector3 _position, float _duration, int _dir, float _fadeSpeed)
        {
            image = _image;
            position = _position;
            duration = _duration;
            facingDir = _dir;
            fadeSpeed = _fadeSpeed;
        }
    }
    public struct DProjectileData
    {
        public WReadOnlyDamageData damage;
        public EEntityType targetType;
        public Vector2 velocity;
        public float gravity;

        public GameObject manager;

        public DProjectileData(WReadOnlyDamageData _damage, EEntityType _targetType, Vector2 _velocity, float _gravity = -1, GameObject _manager = null)
        {
            damage = _damage;
            targetType = _targetType;
            velocity = _velocity;
            gravity = _gravity;
            manager = _manager;
        }
    }
    public struct DAmmoData
    {
        public WReadOnlyDamageData damage;
        public Transform target;
        public EEntityType targetType;
        public IObjectEntity originEntity;

        public GameObject manager;

        public DAmmoData(WReadOnlyDamageData _damage, EEntityType _targetType, Transform _target, IObjectEntity _origin, GameObject _manager = null)
        {
            damage = _damage;
            targetType = _targetType;
            target = _target;
            originEntity = _origin;
            manager = _manager;
        }
    }
    public struct DSpinSwordData
    {
        public WReadOnlyDamageData damage;
        public Vector2 velocity;
        public float gravity;
        public float spinDuration;
        public float damageCooldown;

        public GameObject manager;

        public DSpinSwordData(
            WReadOnlyDamageData damage,
            Vector2 velocity,
            float gravity,
            float spinDuration,
            float damageCooldown,
            GameObject _manager = null
            )
        {
            this.damage = damage;
            this.velocity = velocity;
            this.gravity = gravity;
            this.spinDuration = spinDuration;
            this.damageCooldown = damageCooldown;
            manager = _manager;
        }
    }

    public struct DBounceSwordData
    {
        public WReadOnlyDamageData damage;
        public EEntityType targetType;
        public Vector2 velocity;
        public float gravity;
        public int bounceCount;
        public float bounceSpeed;
        public float bounceRadius;

        public GameObject manager;

        public DBounceSwordData(
            WReadOnlyDamageData damage,
            EEntityType targetType,
            Vector2 velocity,
            float gravity,
            int bounceCount,
            float bounceSpeed,
            float bounceRadius,
            GameObject _manager = null
            )
        {
            this.damage = damage;
            this.targetType = targetType;
            this.velocity = velocity;
            this.gravity = gravity;
            this.bounceCount = bounceCount;
            this.bounceSpeed = bounceSpeed;
            this.bounceRadius = bounceRadius;
            manager = _manager;
        }
    }

    public struct DPlayerCloneData
    {
        public WReadOnlyDamageData damage;
        public bool canAttack;
        public int attackTypeCount;
        public DPlayerCloneData(WReadOnlyDamageData _damage, bool _canAttack, int _attackTypeCount)
        {
            damage = _damage;
            canAttack = _canAttack;
            attackTypeCount = _attackTypeCount;
        }
    }
}

