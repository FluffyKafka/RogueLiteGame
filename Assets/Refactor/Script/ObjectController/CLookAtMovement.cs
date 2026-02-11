using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class CLookAtMovement : CObjectComponentBase
    {
        protected Rigidbody2D rg;
        protected bool isLookAt = false;
        protected override void Awake()
        {
            base.Awake();
            rg = GetComponent<Rigidbody2D>();
            controller.SetLookAtMovement += SetLookAt;
            controller.ClearNotice += Clear;
        }

        protected void SetLookAt(bool _isLook)
        {
            isLookAt = _isLook;
        }
        protected void Update()
        {
            if(isLookAt)
            {
                transform.right = rg.velocity;
            }       
        }

        protected void Clear()
        {
            isLookAt = false;
        }
    }
}

