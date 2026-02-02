using PlayerSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace EnemySystem
{
    public interface IInitEnemyFactory
    {
        public void Init(IEnemyPlayer _player);
    }
    internal class FEnemyFactory : MonoBehaviour, IInitEnemyFactory
    {
        protected static FEnemyFactory instance;
        protected static IEnemyPlayer player;

        [Header("EnemyPrefab")]
        [SerializeField] protected GameObject skeletonPrefab;

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


        public void InitEnemyNotGenerateByFactory_TestMode(AEnemy _enemy)
        {
            Assert.IsTrue(isTestMode, "此方法只能在测试时执行，运行时所有敌人都由工厂生产而不是直接摆放入场景");
            _enemy.Init(player);
        }


        void IInitEnemyFactory.Init(IEnemyPlayer _player)
        {
            player = _player;
            TryInitPrefab(skeletonPrefab);
        }
        protected void TryInitPrefab(GameObject _prefab)
        {
            if (_prefab == null)
            {
                return;
            }

            _prefab.GetComponent<AEnemy>().Init(player);
        }

        public GameObject GetSkeleton(Vector3 _worldPosition)
        {
            Assert.IsNotNull(skeletonPrefab, "Skeleton的Prefab未设置");
            return Instantiate(skeletonPrefab, _worldPosition, Quaternion.identity);
        }
    }
}

