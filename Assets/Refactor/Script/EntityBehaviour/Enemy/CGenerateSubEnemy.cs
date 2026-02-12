using EnemySystem;
using EntityBehaviour;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyBehaviour
{


    internal class CGenerateSubEnemy : CEntityComponentBase
    {
        [Serializable]
        internal class DSubEnemyGenerateData
        {
            public Transform location;
            public EEnemyType type;
        }
        [SerializeField] protected List<DSubEnemyGenerateData> subEnemyGenerateData;

        protected MEnemyBehaviour behaviour;
        protected List<GameObject> subEnemies = new();

        protected override void Awake()
        {
            base.Awake();

            behaviour = entity as MEnemyBehaviour;
            behaviour.GenerateSubEnemyNotice += GenerateSubEnemy;
        }

        protected void GenerateSubEnemy()
        {
            foreach(var data in subEnemyGenerateData)
            {
                GameObject newEnemy = behaviour.InvokeFunc(behaviour.ToGenerateSubEnemy, data.type, data.location.position);
                subEnemies.Add(newEnemy);
            }
        }

    }
}

