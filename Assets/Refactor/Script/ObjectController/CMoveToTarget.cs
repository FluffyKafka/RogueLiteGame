using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{
    internal class CMoveToTarget : CObjectComponentBase
    {
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected Transform rawTransformTarget;
        protected Transform target;

        protected override void Awake()
        {
            base.Awake();
            controller.SetMoveToTargetNotice += SetMoveToTarget;
            controller.SetMoveToTargetRawNotice += SetMoveToTargetRaw;
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
            CancelGravity();

            target = _target;
            if(_speed > 0)
            {
                moveSpeed = _speed;
            }      
        }
        protected void SetMoveToTargetRaw(Vector2 _position, float _speed = -1)
        {
            CancelGravity();

            rawTransformTarget.position = _position;
            if (_speed > 0)
            {
                moveSpeed = _speed;
            }
        }

        protected void CancelGravity()
        {
            if (GetComponent<Rigidbody2D>() != null)
            {
                GetComponent<Rigidbody2D>().isKinematic = true;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Rigidbody2D>().gravityScale = 0;
            }
        }

        protected void Clear()
        {
            target = null;
            if (GetComponent<Rigidbody2D>() != null)
            {
                GetComponent<Rigidbody2D>().isKinematic = false;
            }
        }
    }
}