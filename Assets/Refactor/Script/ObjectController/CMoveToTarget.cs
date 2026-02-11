using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class CMoveToTarget : CObjectComponentBase
    {
        [SerializeField] protected float moveSpeed;
        protected Transform target;

        protected override void Awake()
        {
            base.Awake();
            controller.SetMoveToTarget += SetMoveToTarget;
            controller.ClearNotice += Clear;
        }

        protected void Update()
        {
            if(target != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            }
        }

        protected void SetMoveToTarget(Transform _target, float _speed = -1)
        {
            target = _target;
            if(_speed > 0)
            {
                moveSpeed = _speed;
            }      
        }

        protected void Clear()
        {
            target = null;
        }
    }
}