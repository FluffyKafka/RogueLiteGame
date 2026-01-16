using EntitySystem.EntityActor.PlayerActor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace EntitySystem
{
    namespace EntityActor
    {
        namespace EnemyActor
        {
            public interface IInitEnemyFactory
            {
                public void Init(IEnemyPlayer _player);
            }
            internal class MEnemyFactory : MonoBehaviour, IInitEnemyFactory
            {
                protected static MEnemyFactory instance;
                protected static IEnemyPlayer player;

                [Header("EnemyPrefab")]
                [SerializeField] protected GameObject skeletonPrefab;

                [Header("Test")]
                [SerializeField] protected bool isTestMode;
                protected static bool isTestMode_Class;

                protected virtual void Awake()
                {
                    if(instance == null)
                    {
                        instance = this;
                        isTestMode_Class = isTestMode;
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }

                public static MEnemyFactory GetInstance_TestMode()
                {
                    Assert.IsTrue(isTestMode_Class, "此方法只能在测试时执行，运行时所有敌人都由工厂生产而不是直接摆放入场景");
                    return instance;
                }


                public void InitEnemyNotGenerateByFactory_TestMode(IInitEnemy _enemy)
                {
                    Assert.IsTrue(isTestMode, "此方法只能在测试时执行，运行时所有敌人都由工厂生产而不是直接摆放入场景");
                    _enemy.Init(player);
                }


                void IInitEnemyFactory.Init(IEnemyPlayer _player)
                {
                    player = _player;
                    TryInitPrefab<ASkeleton>(skeletonPrefab);
                }
                protected void TryInitPrefab<T>(GameObject _prefab)
                {
                    if (_prefab == null)
                    {
                        Debug.LogWarning(typeof(T).Name +  "的Prefab未设置");
                        return;
                    }

                    if (_prefab.GetComponent<T>() == null)
                    {
                        Debug.LogWarning(typeof(T).Name + "的Prefab处填入的GameObject不是" + typeof(T).Name);
                        return;
                    }

                    _prefab.GetComponent<IInitEnemy>().Init(player);
                }

                public GameObject GetSkeleton(Vector3 _worldPosition)
                {
                    AssertPrefabIsSet<ASkeleton>(skeletonPrefab);
                    return Instantiate(skeletonPrefab, _worldPosition, Quaternion.identity);
                }
                protected void AssertPrefabIsSet<T>(GameObject _prefab)
                {
                    Assert.IsNotNull(_prefab, typeof(T).Name + "的Prefab未设置");
                    Assert.IsNotNull(_prefab.GetComponent<ASkeleton>(), typeof(T).Name + "的Prefab处填入的GameObject不是" + typeof(T).Name);
                }
            }
        }
    }
}

