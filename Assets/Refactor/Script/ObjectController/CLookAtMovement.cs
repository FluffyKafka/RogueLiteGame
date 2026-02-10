using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class CLookAtMovement : CObjectComponentBase
    {
        protected Rigidbody2D rg;
        protected override void Awake()
        {
            base.Awake();
            rg = GetComponent<Rigidbody2D>();
        }
        protected void Update()
        {
            transform.right = rg.velocity;
        }
    }
}

