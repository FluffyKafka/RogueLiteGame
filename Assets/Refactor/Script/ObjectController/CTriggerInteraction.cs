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
        [SerializeField] bool CanMultipleTrigger = false;
        protected bool canTrigger = false;
        protected override void Awake()
        {
            controller = GetComponentInParent<AObjectController>();
            Assert.IsNotNull(controller, "CCollisionInteraction需要挂在一个AObjectController的子GameObject上");
            Assert.IsNotNull(GetComponent<Collider2D>(), "CCollisionInteraction需要管理一个Collider2D");
            Assert.IsTrue(GetComponent<Collider2D>().isTrigger, "CCollisionInteraction需要管理一个触发器");
            controller.ResetTrigger += ResetTrigger;
            canTrigger = true;
            controller.ClearNotice += Clear;
        }

        protected void ResetTrigger()
        {
            if(allowTriggerDelay > 0)
            {
                StartCoroutine(SetCanTriggerAfterDelay());
            }
            else
            {
                canTrigger = true;
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
                Debug.Log("HitPlayer");
                canTrigger = CanMultipleTrigger;
            }

            if(targetType == EEntityType.Enemy && _collision.GetComponent<IObjectEnemy>() != null)
            {
                controller.InvokeAction(controller.HitEnemyNotice, _collision.GetComponent<IObjectEnemy>());
                canTrigger = CanMultipleTrigger;
            }

            //LayerMask的value为掩码（3 => 1000 => 8）
            //1 << _collision.gameObject.layer将1左移3位（0001 -> 1000）最后或运算
            if (isGroundTrigger && (whatIsGround.value & (1 << _collision.gameObject.layer)) != 0)
            {
                controller.InvokeAction(controller.HitGroundNotice, _collision.transform);
                Debug.Log("HitGround");
                canTrigger = CanMultipleTrigger;
            }
        }

        protected void Clear()
        {
            canTrigger = true;
        }
    }
}

