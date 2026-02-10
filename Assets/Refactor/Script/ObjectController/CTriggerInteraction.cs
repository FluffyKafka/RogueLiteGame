using EnemySystem;
using EntitySystem;
using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Assertions;

namespace ObjectController
{
    internal class CTriggerInteraction : CObjectComponentBase
    {
        [Tooltip("小于等于0表示不延迟")]
        [SerializeField] protected float allowTriggerDelay = 0.5f;
        [SerializeField] protected bool isGroundTrigger = false;
        [SerializeField] protected LayerMask whatIsGround;
        [SerializeField] EEntityType targetType;
        protected bool canTrigger = false;
        protected override void Awake()
        {
            controller = GetComponentInParent<AObjectController>();
            Assert.IsNotNull(controller, "CCollisionInteraction需要挂在一个AObjectController的子GameObject上");
            Assert.IsNotNull(GetComponent<Collider2D>(), "CCollisionInteraction需要管理一个Collider2D");
            Assert.IsTrue(GetComponent<Collider2D>().isTrigger, "CCollisionInteraction需要管理一个触发器");
            controller.ResetTrigger += ResetTrigger;
        }

        protected void ResetTrigger()
        {
            if(allowTriggerDelay > 0)
            {
                StartCoroutine(SetCanTriggerAfterDelay());
            }
        }
        protected IEnumerator SetCanTriggerAfterDelay()
        {
            canTrigger = false;
            yield return new WaitForSeconds(allowTriggerDelay);
            canTrigger = true;
        }

        public void SwitchTargetTo(EEntityType _type)
        {
            targetType = _type;
        }

        protected void OnTriggerEnter2D(Collider2D _collision)
        {
            if(!canTrigger)
            {
                return;
            }

            if(targetType == EEntityType.Player && _collision.GetComponent<IObjectPlayer>() != null)
            {
                controller.InvokeAction(controller.HitPlayer, _collision.GetComponent<IObjectPlayer>());
            }

            if(targetType == EEntityType.Enemy && _collision.GetComponent<IObjectEnemy>() != null)
            {
                controller.InvokeAction(controller.HitEnemy, _collision.GetComponent<IObjectEnemy>());
            }

            if(isGroundTrigger && _collision.gameObject.layer == whatIsGround)
            {
                controller.InvokeAction(controller.HitGround, _collision.transform);
            }
        }
    }
}

