using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagerSystem
{
    internal class CSceneManager : CGameManagerComponentBase
    {
        private AsyncOperation asyncOperation;
        private float lastLoadRate = 0f;
        private float targetLoadRate = 0f;
        private bool isLoading = false;

        // 平滑参数
        [SerializeField] private float smoothSpeed = 5f; // 平滑速度
        private Coroutine smoothCoroutine = null;

        protected override void Awake()
        {
            base.Awake();
            game.SwitchSceneToNotice += SceneSwitchTo;
            game.CheckCurrentSceneNameNotice += CheckCurrentSceneName;
            game.CheckSceneLoadRateNotice += CheckSceneLoadRate;
        }

        protected void SceneSwitchTo(string _sceneName)
        {
            // 如果已经在加载中，先停止之前的加载
            if (isLoading)
            {
                return;
            }

            // 启动协程进行异步加载
            StartCoroutine(LoadSceneAsync(_sceneName));
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            isLoading = true;

            // 开始异步加载场景
            asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;

            // 启动平滑进度更新协程
            if (smoothCoroutine != null)
                StopCoroutine(smoothCoroutine);
            smoothCoroutine = StartCoroutine(SmoothProgressUpdate());

            // 等待加载完成，同时确保至少持续0.1秒
            while (targetLoadRate < 0.99f)
            {
                // 计算原始进度（0-0.9转为0-1）
                targetLoadRate = Mathf.Clamp01(asyncOperation.progress / 0.9f);

                yield return null;
            }         

            // 确保最终进度为1
            targetLoadRate = 1f;

            while(lastLoadRate < 0.99)
            {
                yield return null;
            }

            // 停止平滑协程
            if (smoothCoroutine != null)
            {
                StopCoroutine(smoothCoroutine);
                smoothCoroutine = null;
            }

            // 加载完成，清理引用
            asyncOperation.allowSceneActivation = true;
            asyncOperation = null;
            isLoading = false;
        }

        private IEnumerator SmoothProgressUpdate()
        {
            while (isLoading)
            {
                // 使用平滑插值更新显示进度
                lastLoadRate = Mathf.Lerp(lastLoadRate, targetLoadRate, Time.deltaTime * smoothSpeed);

                // 防止进度倒退
                if (lastLoadRate > targetLoadRate + 0.01f)
                    lastLoadRate = targetLoadRate;

                // 确保进度在有效范围内
                lastLoadRate = Mathf.Clamp01(lastLoadRate);

                yield return null;
            }
        }

        protected string CheckCurrentSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }

        protected float CheckSceneLoadRate()
        {
            if (!isLoading || asyncOperation == null)
            {
                return -1f;
            }

            return lastLoadRate;
        }

        // 可选：重置加载状态（用于错误恢复）
        protected void ResetLoadState()
        {
            isLoading = false;
            asyncOperation = null;
            lastLoadRate = 0f;
            targetLoadRate = 0f;

            if (smoothCoroutine != null)
            {
                StopCoroutine(smoothCoroutine);
                smoothCoroutine = null;
            }
        }
    }
}