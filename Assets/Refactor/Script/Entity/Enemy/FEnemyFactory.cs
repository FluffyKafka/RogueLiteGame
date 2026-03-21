using PlayerSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EnemySystem
{
    public interface IInitEnemyFactory
    {
        public void Init(IEnemyPlayer _player, IEnemyObjectFactory _objectFactory, IEnemyAduio[] _enemyAudioManagers);
    }
    public interface IMapEnemyFactory
    {
        public GameObject GetEmemyGameObjectAt(EEnemyType _type, Vector3 _position);
    }
    internal class FEnemyFactory : MonoBehaviour, IInitEnemyFactory, IEnemyFactory, IMapEnemyFactory
    {
        protected static FEnemyFactory instance;
        protected static IEnemyPlayer player;
        protected static IEnemyObjectFactory objectFactory;
        protected IEnemyAduio[] enemyAduioManagers;

        [Header("EnemyPrefab")]
        [SerializeField] protected GameObject skeletonPrefab;
        [SerializeField] protected GameObject archerPrefab;
        [SerializeField] protected GameObject necromancerPrefab;
        [SerializeField] protected GameObject slimePrefab;
        [SerializeField] protected GameObject subSlimePrefab;
        [SerializeField] protected GameObject minSlimePrefab;

        [Header("Test")]
        [SerializeField] protected bool isTestMode;
        protected static bool isTestMode_Class;

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

        public static FEnemyFactory GetInstance_TestMode()
        {
            Assert.IsTrue(isTestMode_Class, "此方法只能在测试时执行，运行时所有敌人都由工厂生产而不是直接摆放入场景");
            return instance;
        }


        public void InitEnemyNotGenerateByFactory_TestMode(AEnemy _enemy, EEnemyType _type)
        {
            Assert.IsTrue(isTestMode, "此方法只能在测试时执行，运行时所有敌人都由工厂生产而不是直接摆放入场景");
            _enemy.Init(player, objectFactory, this, TryGetEnemyAudioByEnemyType(_type));
        }


        void IInitEnemyFactory.Init(IEnemyPlayer _player, IEnemyObjectFactory _objectFactory, IEnemyAduio[] _enemyAudioManagers)
        {
            player = _player;
            objectFactory = _objectFactory;
            enemyAduioManagers = _enemyAudioManagers;
            TryInitPrefab(skeletonPrefab);
            TryInitPrefab(archerPrefab);
            TryInitPrefab(necromancerPrefab);
            TryInitPrefab(slimePrefab);
            TryInitPrefab(subSlimePrefab);
            TryInitPrefab(minSlimePrefab);
        }
        protected void TryInitPrefab(GameObject _prefab)
        {
            if (_prefab == null)
            {
                return;
            }

            _prefab.GetComponent<AEnemy>().Init(player, objectFactory, this, TryGetEnemyAudioByEnemyType(_prefab.GetComponent<AEnemy>().CheckEnemyType()));
        }

        public GameObject GetEmemyGameObjectAt(EEnemyType _type, Vector3 _worldPosition)
        {
            switch(_type)
            {
                case EEnemyType.Skeleton:
                    Assert.IsNotNull(skeletonPrefab, "Skeleton的Prefab未设置");
                    return Instantiate(skeletonPrefab, _worldPosition, Quaternion.identity);
                case EEnemyType.Archer:
                    Assert.IsNotNull(archerPrefab, "Archer的Prefab未设置");
                    return Instantiate(archerPrefab, _worldPosition, Quaternion.identity);
                case EEnemyType.Necromancer:
                    Assert.IsNotNull(necromancerPrefab, "Necromancer的Prefab未设置");
                    return Instantiate(necromancerPrefab, _worldPosition, Quaternion.identity);
                case EEnemyType.Slime:
                    Assert.IsNotNull(slimePrefab, "Slime的Prefab未设置");
                    return Instantiate(slimePrefab, _worldPosition, Quaternion.identity);
                case EEnemyType.SubSlime:
                    Assert.IsNotNull(subSlimePrefab, "SubSlime的Prefab未设置");
                    return Instantiate(subSlimePrefab, _worldPosition, Quaternion.identity);
                case EEnemyType.MinSlime:
                    Assert.IsNotNull(minSlimePrefab, "MinSlime的Prefab未设置");
                    return Instantiate(minSlimePrefab, _worldPosition, Quaternion.identity);
                default:
                    Assert.IsFalse(true, "未知敌人类型：" + _type);
                    return null;
            }
            
        }

        public GameObject GenerateEnemyByTypeAt(EEnemyType _type, Vector3 _position)
        {
            return GetEmemyGameObjectAt(_type, _position);
        }

        protected IEnemyAduio TryGetEnemyAudioByEnemyType(EEnemyType _type)
        {
            foreach(var audio in enemyAduioManagers)
            {
                if(audio.CheckEnemyType() == _type)
                {
                    return audio;
                }
            }
            Debug.Log("Not Find: " + _type.ToString());
            return null;
        }
    }
}

