using EntityBehaviour;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyBehaviour
{
    internal class CEnemyDrop : CEntityComponentBase
    {
        [Serializable]
        protected class DDropItemRate
        {
            public ScriptableObject item;
            [Range(0, 1)] public float rate;
        }
        [SerializeField] protected List<DDropItemRate> dropsRateList;

        protected MEnemyBehaviour enemy;

        protected override void Awake()
        {
            base.Awake();
            enemy = entity as MEnemyBehaviour;
            enemy.Die += DropItems;
        }

        protected void DropItems()
        {
            List<ScriptableObject> drops = new();
            foreach(var dropRate in dropsRateList)
            {
                if(UnityEngine.Random.Range(0.0f, 1.0f) < dropRate.rate)
                {
                    drops.Add(dropRate.item);
                }
            }

            foreach(var drop in drops)
            {
                enemy.GenerateDropItemByDataAt(drop, transform.position);
            }
        }
    }
}

