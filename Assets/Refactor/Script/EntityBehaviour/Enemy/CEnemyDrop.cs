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
        [SerializeField] protected Vector2 dropSoulRange;
        [SerializeField] protected Vector2 dropCoinRange;

        protected MEnemyBehaviour enemy;

        protected override void Awake()
        {
            base.Awake();
            enemy = entity as MEnemyBehaviour;
            enemy.Die += DropItems;
            enemy.CheckDropSoulAmountNotice += CheckDropSoulAmount;
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

            enemy.GenerateCoin(UnityEngine.Random.Range(dropCoinRange.x, dropCoinRange.y));
        }

        protected float CheckDropSoulAmount()
        {
            return UnityEngine.Random.Range(dropSoulRange.x, dropSoulRange.y);
        }
    }
}

