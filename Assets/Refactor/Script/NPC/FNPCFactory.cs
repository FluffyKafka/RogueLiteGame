using EnemySystem;
using PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace NPCSystem
{
    public interface IInitNPCFactory
    {
        public void Init(INPCObjectFactory _objectFactory);
    }

    internal class FNPCFactory : MonoBehaviour, IInitNPCFactory
    {
        [SerializeField] protected bool isTestMode;

        protected static FNPCFactory instance;
        protected static bool isTestMode_Class;

        protected INPCObjectFactory objectFactory;

        [Serializable]
        public class DNPCPrefabData
        {
            public ENPCType type;
            public GameObject prefab;
        }
        [Header("NPCPrefab")]
        [SerializeField] protected List<DNPCPrefabData> npcPrefabs;

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this;
                isTestMode_Class = isTestMode;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Init(INPCObjectFactory _objectFactory)
        {
            objectFactory = _objectFactory;
            foreach(var npc in npcPrefabs)
            {
                npc.prefab.GetComponent<ANPC>().Init(objectFactory);
            }
        }

        protected GameObject GetPrefabByType(ENPCType _type)
        {
            foreach(var data in npcPrefabs)
            {
                if(data.type == _type)
                {
                    return data.prefab;
                }
            }
            return null;
        }

        public static FNPCFactory GetInstance_TestMode()
        {
            Assert.IsTrue(isTestMode_Class, "此方法只能在测试时执行，运行时所有NPC都由工厂生产而不是直接摆放入场景");
            return instance;
        }

        public void InitEnemyNotGenerateByFactory_TestMode(ANPC _npc, ENPCType _type)
        {
            Assert.IsTrue(isTestMode, "此方法只能在测试时执行，运行时所有NPC都由工厂生产而不是直接摆放入场景");
            _npc.Init(objectFactory);
        }

        public GameObject GenerateNPCByTypeAt(ENPCType _type, Vector3 _position)
        {
            return Instantiate(GetPrefabByType(_type), _position, Quaternion.identity);
        }
    }
}

