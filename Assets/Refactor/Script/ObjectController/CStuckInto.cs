using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class CStuckInto : CObjectComponentBase
    {
        protected bool isStuck = false;
        protected Rigidbody2D rg;
        protected Collider2D col;
        protected override void Awake()
        {
            base.Awake();
            rg = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
            controller.StuckInto += StuckInto;
            controller.ClearNotice += Clear;
        }

        protected void StuckInto(Transform _target)
        {
            col.enabled = false;
            rg.isKinematic = true;
            rg.constraints = RigidbodyConstraints2D.FreezeAll;
            transform.SetParent(_target, true);
            isStuck = true;
        }   
        
        protected void Clear()
        {
            col.enabled = true;
            rg.isKinematic = false;
            rg.constraints = RigidbodyConstraints2D.None;
            transform.SetParent(null);
            isStuck = false;
        }
    }
}

