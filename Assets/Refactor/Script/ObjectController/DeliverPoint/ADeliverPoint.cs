using PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    public interface IMapUIDeliverPoint
    {
        public void Choose(bool _isChosen);
        public bool IsChosen();
        public void Deliver();
    }

    internal class ADeliverPoint : AObjectController, IMapUIDeliverPoint
    {
        [SerializeField] protected string noticeText = "´«ËÍ";

        protected Transform playerTargetTransform;
        protected Transform nextTargetTransform;
        protected bool isChosen;
        protected bool isPlayer;
        protected bool isHolding;
        protected bool isActive;

        protected IObjectPlayer interactPlayer;

        public void Setup(FCDeliverPointFactory _factory)
        {
            Debug.Log("ADeliverPoint");
            factory = _factory;

            HitPlayer += PlayerEnter;
            PlayerExitNotice += PlayerExit;
            DamageFinishNotice += AnimFinish;
        }

        private void Update()
        {
            if(interactPlayer != null)
            {
                if (interactPlayer.IsGamePause())
                {
                    anim.SetSpeed(1 / interactPlayer.CheckPauseSlowRate());
                }
                else
                {
                    anim.SetSpeed(1);
                }
            }
        }

        public override void Clear()
        {
            base.Clear();
            HitPlayer -= PlayerEnter;
            PlayerExitNotice -= PlayerExit;
            DamageFinishNotice -= AnimFinish;
            interactPlayer = null;
        }

        public override string CheckInteractMessage()
        {
            return noticeText;
        }

        protected void PlayerEnter(IObjectPlayer _player)
        {
            isPlayer = true;
            interactPlayer = _player;

            if (!isActive)
            {
                isActive = true;
                anim.ToEffect(1);
            }
        }
        protected void PlayerExit(IObjectPlayer _player)
        {
            isPlayer = false;
            isHolding = false;
        }

        public override bool CanInteract()
        {
            return true;
        }

        public override void Interact(IObjectPlayer _player)
        {
            base.Interact(_player);
            isHolding = true;
           _player.ShowMapUIPage();           
        }

        public void Choose(bool _isChosen)
        {
            if (interactPlayer == null)
            {
                return;
            }
            isChosen = _isChosen;
            if(_isChosen)
            {
                anim.ToEffect(2);
            }
            else
            {
                anim.ToEffect(0);
            }
        }

        public bool IsChosen()
        {
            return isChosen;
        }

        public void Deliver()
        {
            if(interactPlayer == null)
            {
                return;
            }
            anim.ToEffect(3);
            interactPlayer.GetTransform().position = transform.position;
        }

        public void AnimFinish()
        {
            anim.ToEffect(0);
        }
    }
}

