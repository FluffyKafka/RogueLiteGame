using ObjectController;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Assertions;

namespace Tool
{
    //对象池必须与一个工厂对象联合使用，工厂负责其初始化和销毁，对象池仅进行对象管理
    public interface IObjectPool
    {
        public void InitPool(GameObject _initObject, int _initSize);
        public GameObject GetObject();
        public void RecycleObject(GameObject _object);
    }
    internal class TObjectPool : MonoBehaviour, IObjectPool
    {
        protected GameObject prototype;
        protected Stack<GameObject> itemPool;
        protected HashSet<GameObject> usingItem;

        public void InitPool(GameObject _initObject, int _initSize)
        {
            prototype = _initObject;
            itemPool = new(_initSize);
            usingItem = new(_initSize);

            for (int i = 0; i < _initSize; ++i)
            {
                GameObject newObject = Instantiate(prototype);
                newObject.SetActive(false);
                itemPool.Push(newObject);
            }
        }

        public GameObject GetObject()
        {
            if(itemPool.Count == 0)
            {
                GameObject newObject = Instantiate(prototype);
                newObject.SetActive(false);
                itemPool.Push(newObject);
            }

            GameObject res = itemPool.Peek();
            itemPool.Pop();
            usingItem.Add(res);
            res.SetActive(true);
            return res;
        }

        public void RecycleObject(GameObject _object)
        {
            usingItem.Remove(_object);
            itemPool.Push(_object);
        }
    }
}