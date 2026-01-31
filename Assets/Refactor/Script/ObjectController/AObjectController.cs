using PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    public interface IAnimObject
    {

    }

    internal abstract class AObjectController : ComponentManagerBase, IPlayerEnterable, IPlayerInteractable, IPlayerReflectable, IAnimObject
    {
        #region Action
        public Action<IObjectPlayer> PlayerEnter;
        public Action<IObjectPlayer> PlayerInteract;
        public Action<IObjectPlayer> PlayerReflect;
        public Action<int> OriginProjectToward;
        public Action<int> SecondaryProjectToward;
        public Action<IObjectPlayer> HitPlayer;
        public Action<Sprite> InitAnimSprite;
        public Action ResetTrigger;
        public Action SelfRecycleNotice;
        #endregion

        protected IObjectAnim anim;

        protected virtual void Awake()
        {
            anim = GetComponentInChildren<IObjectAnim>();
            if(anim == null)
            {
                Debug.LogWarning("对象控制器: " + GetType().Name + "没有动画组件");
            }
            else
            {
                InitAnimSprite += anim.InitAnimImage;
            }
        }

        protected abstract void SelfRecycle();

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
    }

    public interface IObjectAnim
    {
        public void InitAnimImage(Sprite _image);
    }
}