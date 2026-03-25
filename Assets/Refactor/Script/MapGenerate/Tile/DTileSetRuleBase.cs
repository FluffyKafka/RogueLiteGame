using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

namespace MapGenerate
{
    [CreateAssetMenu(fileName = "New Tile Set Rule", menuName = "Map Generate System/Tile Set Rule")]
    internal class DTileSetRuleBase : ScriptableObject
    {
        [Serializable]
        protected class RuleList
        {
            public List<DRuleData> list;
        }
        [Serializable]
        protected class DRuleData
        {
            [Header("是否无条件")]
            [SerializeField] protected bool isEmpty;
            [Header("原型中是否有tile，0表示无视， -1表示必须无tile")]
            [SerializeField] protected int isNotNullInPrototype;
            [Header("实际房间中已经放置的tile是否拥有tag")]
            [SerializeField] protected string tag;

            public bool isMatch(DTile _tile, bool _isPrototype)
            {
                if(isEmpty)
                {
                    return true;
                }

                if(isNotNullInPrototype != 0)
                {
                    if(isNotNullInPrototype < 0 && _isPrototype)
                    {
                        return false;
                    }
                    if(isNotNullInPrototype > 0 && !_isPrototype)
                    {
                        return false;
                    }
                }

                if(_tile != null)
                {
                    if(_tile.tag != string.Empty && _tile.tag != tag)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        [Header("tile可以在数组列举的情况下被放置，数组为空则任意情况可以放置")]
        [SerializeField] protected List<RuleList> tileRuleArray;
        public bool CanPlace(DTile[] neighborTiles, bool[] prototypeNeighbors)
        {            
            if(tileRuleArray.Count == 0)
            {
                return true;
            }

            for(int i = 0; i < tileRuleArray.Count; ++i)
            {
                if (CheckMatchIndex(i, ref neighborTiles, ref prototypeNeighbors))
                {
                    return true;
                }
            }
            return false;
        }
        protected bool CheckMatchIndex(int _index, ref DTile[] neighborTiles, ref bool[] prototypeNeighbors)
        {
            Assert.IsTrue(tileRuleArray[_index].list.Count == 8);
            for (int i = 0; i < 8; ++i)
            {
                if (!tileRuleArray[_index].list[i].isMatch(neighborTiles[i], prototypeNeighbors[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}


