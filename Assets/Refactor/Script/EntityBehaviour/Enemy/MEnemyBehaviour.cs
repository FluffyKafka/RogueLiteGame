using EnemySystem;
using EntityBehaviour;
using EntitySystem;
using ObjectGenerateData;
using StatsData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyBehaviour
{
    internal class MEnemyBehaviour : MEntityBehaviour, IEnemyBehaviour
    {
        protected IBehaviourEnemy enemySystem;

        public Func<float> CheckIdleDuration;
        public Action<int> MoveForward;
        public Action<int> MoveToward;
        public Action<int> MoveToward_Battle;
        public Action FacingToPlayer;
        public Action BeStunned;
        public Action<bool> OpenStun;
        public Action StunFinishNotice;
        public Action StandStill;
        public Func<bool> IsDetectPlayer;
        public Action UpdateBattle;
        public Func<bool> StunCheck;
        public Func<int> CheckBattleMoveDir;
        public Action AttackCheck;
        public Action StopBattle;
        public Action Attack;
        public Action ToIdle;
        public Action ToMove;
        public Action ToFall;
        public Action ToControll;
        public Action GenerateSubEnemyNotice;//通知内部组件
        public Func<EEnemyType, Vector3, GameObject> ToGenerateSubEnemy;//通知外部
        public Action<bool> ToBattle;

        #region Arrow
        public Action<DProjectileData, Vector3> GenerateArrowAt;
        public Func<float> CheckArrowGravity;
        #endregion

        #region Ammo
        public Action<DAmmoData, Vector3> GenerateSkullAmmo;
        #endregion

        #region Object
        public Action ObjectFinishNotice;
        #endregion

        #region SelfExplode
        public Func<float> CheckSelfExplodeHoldingDurationNotice;
        public Action<bool> SelfExplodeNotice_isReflect;
        public Action ToSelfExplode;
        public Action ToSelfExplodeHolding;
        public Action OnSelfExplodeDamageTrigger;
        public Action OnSelfExplodeFinish;
        #endregion

        #region PullBack
        public Func<bool> CanPullBack;
        public Action PullBackUpdate;
        public Action ToPullBack;
        #endregion

        #region PullBackJump
        public Func<bool> CanPullBackJump;
        public Action EffectPullBackJump;
        public Action ToPullbackJump;
        #endregion

        public Func<GameObject, bool> IsPlayer;
        public Func<Vector3> CheckPlayerPosition;
        public Func<Vector3> CheckPlayerVelocity;
        public Func<float> CheckPlayerGravityScale;
        public Func<Transform> CheckPlayerTransform;
        public Func<GameObject, WReadOnlyDamageData, WReadOnlyDamageData> DamageToPlayer;
        public Func<GameObject, WReadOnlyDamageData, WReadOnlyDamageData> DamageToEnemy;
        public Func<bool> IsPlayerAlive;
        public Func<GameObject, bool> IsThisPlayerAlive;

        protected void Awake()
        {
            enemySystem = GetComponentInParent<IBehaviourEnemy>();
            IsPlayer += enemySystem.IsPlayer;
            CheckPlayerPosition += enemySystem.CheckPlayerPosition;
            CheckPlayerVelocity += enemySystem.CheckPlayerVelocity;
            CheckPlayerGravityScale += enemySystem.CheckPlayerGravityScale;
            DamageToPlayer += enemySystem.DamageToPlayer;
            IsPlayerAlive += enemySystem.IsPlayerAlive;
            IsThisPlayerAlive += enemySystem.IsPlayerAlive;
            ToIdle += enemySystem.ToIdle;
            ToMove += enemySystem.ToMove;
            Attack += enemySystem.ToAttack;
            BeStunned += enemySystem.BeStunned;
            StunFinishNotice += enemySystem.StunnedFinish;
            GetPrimaryAttackDamage += enemySystem.GetPrimaryAttackDamage;
            ToDead += enemySystem.ToDead;
            ToPullBack += enemySystem.ToPullBack;
            ToPullbackJump += enemySystem.ToPullBackJump;
            CheckArrowGravity += enemySystem.CheckArrowGravity;
            GenerateArrowAt += enemySystem.GenerateArrowAt;
            GenerateSkullAmmo += enemySystem.GenerateSkullAmmoAt;
            CheckPlayerTransform += enemySystem.CheckPlayerTransform;
            ToControll += enemySystem.ToControll;
            ToGenerateSubEnemy += enemySystem.GenerateEnemyByTypeAt;
            DamageToEnemy += enemySystem.DamageToEnemy;
            ToSelfExplode += enemySystem.ToSelfExplode;
            ToSelfExplodeHolding += enemySystem.ToSelfExplodeHolding;
            ToBattle += (bool _isbattle) => enemySystem.SetPlayerToBattle(_isbattle);
        }

        void IEnemyBehaviour.OpenStun(bool _isOpen)
        {
            InvokeAction(OpenStun, _isOpen);
        }

        bool IEnemyBehaviour.StunCheck()
        {
            return InvokeFunc(StunCheck);
        }

        void IEnemyBehaviour.ObjectFinish()
        {
            InvokeAction(ObjectFinishNotice);
        }
        void IEnemyBehaviour.OnSelfExplodeFinish()
        {
            InvokeAction(OnSelfExplodeFinish);
        }

        void IEnemyBehaviour.OnSelfExplodeDamageTrigger()
        {
            InvokeAction(OnSelfExplodeDamageTrigger);
        }
    }
}