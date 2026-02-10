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
        }

        protected void StuckInto(Transform _self, Transform _target)
        {
            col.enabled = false;
            rg.isKinematic = true;
            rg.constraints = RigidbodyConstraints2D.FreezeAll;
            _self.parent = _target.transform;
            isStuck = true;
        }        
    }
}

