using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectController
{    
    internal class CProjectileMovement : CObjectComponentBase
    {
        [SerializeField] protected Vector2 randomOriginProjectXRange;
        [SerializeField] protected Vector2 randomOriginProjectYRange;
        [SerializeField] protected Vector2 randomSecondaryProjectXRange;
        [SerializeField] protected Vector2 randomSecondaryProjectYRange;
        protected Rigidbody2D rg;

        protected override void Awake()
        {
            base.Awake();
            rg = GetComponent<Rigidbody2D>();
            controller.OriginProjectToward += (int _dir) => { Project(GetRandomOriginProjectVelocity(), _dir); };
            controller.SecondaryProjectToward += (int _dir) => { Project(GetRandomSecondaryProjectVelocity(), _dir); };
            controller.Project += (Vector2 _velocity) => { Project(_velocity, 1); };
        }

        protected void Project(Vector2 _velocity, int _dir)
        {
            if(_dir != 0)
            {
                _velocity *= _dir;
            }
            rg.velocity = _velocity;
        }

        protected Vector2 GetRandomOriginProjectVelocity()
        {
            float x = Random.Range(randomOriginProjectXRange.x, randomOriginProjectXRange.y);
            float y = Random.Range(randomOriginProjectYRange.x, randomOriginProjectYRange.y);
            return new Vector2(x, y);
        }
        protected Vector2 GetRandomSecondaryProjectVelocity()
        {
            float x = Random.Range(randomSecondaryProjectXRange.x, randomSecondaryProjectXRange.y);
            float y = Random.Range(randomSecondaryProjectYRange.x, randomSecondaryProjectYRange.y);
            return new Vector2(x, y);
        }
    }
}

