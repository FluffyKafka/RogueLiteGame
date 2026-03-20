using EnemySystem;
using EntitySystem;
using Item;
using PlayerSystem;
using SkillSystem;
using StatsData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    public interface IAnimObject
    {
        public void FadeFinishNotice();
        public void DamageTrigger();
        public void DamageFinish();
    }

    internal abstract class AObjectController : ComponentManagerBase, IPlayerEnterable, IPlayerInteractable, IPlayerReflectable, IAnimObject, ISkillObject
    {
        [SerializeField] protected bool isNoAnimation = false; 

        #region Action
        public Action<IObjectPlayer> PlayerEnter;
        public Action<IObjectPlayer> PlayerInteract;
        public Action<IObjectPlayer> PlayerReflect;
        public Action<int> OriginProjectToward;
        public Action<int> SecondaryProjectToward;
        public Action<Vector2> Project;
        public Action<IObjectPlayer> HitPlayer;
        public Action<IObjectEnemy> HitEnemyNotice;
        public Action<Transform> HitGroundNotice;
        public Action ResetTrigger;
        public Action SelfRecycleNotice;
        public Action<EEntityType> SwitchTargetTo;
        public Action FadeFinish;
        public Action ClearNotice;
        public Action<Transform> StuckInto;
        public Action<bool> SetLookAtMovement;
        public Action Launch;
        public Action<Transform, float> SetMoveToTargetNotice;
        public Action<Vector2, float> SetMoveToTargetRawNotice;
        public Action DamageTriggerNotice;
        public Action<WReadOnlyDamageData, EEntityType> EffectAreaDamageTo;
        public Action DamageFinishNotice;
        public Action<float> SetStayTriggerCooldownNotice;
        #endregion
        #region Func
        public Func<float, Transform> TryGetRandomEnemyInRadiusNotice;
        public Func<float, Transform> TryGetNearestEnemyInRadiusNotice;
        #endregion

        protected FCObjectFactoryComponentBase factory;

        protected IObjectAnim anim;

        protected virtual void Awake()
        {
            if(!isNoAnimation)
            {
                anim = GetComponentInChildren<IObjectAnim>();
                if (anim == null)
                {
                    Debug.LogWarning("对象控制器: " + GetType().Name + "没有动画组件");
                }
                else
                {
                    ClearNotice += anim.Clear;
                }
            }    
        }

        protected void SelfRecycle()
        {
            InvokeAction(SelfRecycleNotice);
            factory.RecycleObject(this);
        }

        public void Enter(IObjectPlayer _player)
        {
            InvokeAction(PlayerEnter, _player);
        }

        public void Interact(IObjectPlayer _player)
        {
            InvokeAction(PlayerInteract, _player);
        }

        public void Reflect(IObjectPlayer _player)
        {
            InvokeAction(PlayerReflect, _player);
        }

        public virtual void Clear()
        {
            InvokeAction(ClearNotice);
        }

        public void FadeFinishNotice()
        {
            InvokeAction(FadeFinish);
        }

        public void DamageTrigger()
        {
            InvokeAction(DamageTriggerNotice);
        }

        public void DamageFinish()
        {
            InvokeAction(DamageFinishNotice);
        }

        Transform ISkillObject.GetTransform()
        {
            return transform;
        }

        void ISkillObject.RecycleObject()
        {
            SelfRecycle();
        }

        public virtual void TakeBack()
        {
            
        }
    }

    public interface IObjectAnim
    {
        public void InitAnimImage(Sprite _image);
        public void SetFadeAway(float _speed, float _cooldown, float _delay);
        public void FadeAway();
        public void Clear();
        public void ShowTrail(bool _isShow);
        public void ToEffect();
        public void ShowHitFx();
        public void ToEffect(int _type);
    }

    public enum EObjectAudioType
    {
        Wind,
        LightTorch,
        TorchFire
    }
    public interface IObjectAudio
    {
        public void PlayObjectAudioByType(EObjectAudioType _type, Transform _object, bool _isPlay);
    }
}