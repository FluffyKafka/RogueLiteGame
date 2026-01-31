using PlayerSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Assertions;

namespace ObjectController
{
    internal class CTriggerInteraction : CObjectControllerComponentBase
    {
        [SerializeField] protected float allowTriggerDelay = 0.5f;
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
            StartCoroutine(SetCanTriggerAfterDelay());
        }
        protected IEnumerator SetCanTriggerAfterDelay()
        {
            canTrigger = false;
            yield return new WaitForSeconds(allowTriggerDelay);
            canTrigger = true;
        }

        protected void OnTriggerEnter2D(Collider2D _collision)
        {
            if(!canTrigger)
            {
                return;
            }

            if (_collision.GetComponent<IObjectPlayer>() != null)
            {
                controller.InvokeAction(controller.HitPlayer, _collision.GetComponent<IObjectPlayer>());
            }
        }
    }
}

