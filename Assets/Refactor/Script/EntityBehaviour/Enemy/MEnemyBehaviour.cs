using EnemySystem;
using EntityBehaviour;
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
        public Action StunFinish;
        public Action StandStill;
        public Func<bool> IsDetectPlayer;
        public Action UpdateBattle;
        public Action StunCheck;
        public Func<int> CheckBattleMoveDir;
        public Action AttackCheck;
        public Action StopBattle;
        public Action Attack;
        public Action ToIdle;
        public Action ToMove;

        public Func<GameObject, bool> IsPlayer;
        public Func<Vector3> CheckPlayerPosition;
        public Func<GameObject, WReadOnlyDamageData, WReadOnlyDamageData> DamageTo;
        public Func<bool> IsPlayerAlive;
        public Func<GameObject, bool> IsThisPlayerAlive;

        protected void Awake()
        {
            enemySystem = GetComponentInParent<IBehaviourEnemy>();
            IsPlayer += enemySystem.IsPlayer;
            CheckPlayerPosition += enemySystem.CheckPlayerPosition;
            DamageTo += enemySystem.DamageTo;
            IsPlayerAlive += enemySystem.IsPlayerAlive;
            IsThisPlayerAlive += enemySystem.IsPlayerAlive;
        }

        void IEnemyBehaviour.OpenStun(bool _isOpen)
        {
            InvokeAction(OpenStun, _isOpen);
        }

        void IEnemyBehaviour.StunCheck()
        {
            InvokeAction(StunCheck);
        }
    }
}