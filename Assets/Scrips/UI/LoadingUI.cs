using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingUI : MonoBehaviour, ILoadingUI
{
    [SerializeField] private Animator charactorAnim;
    private UI_DarkScreen darkScreen;
    [SerializeField] private float left;
    [SerializeField] private float right;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float smoothTime;
    private float curSpeed;
    public void LoadingBegin()
    {
        if (UI.instance != null)
        {
            if (UI.instance.darkScreen != null)
            {
                darkScreen = UI.instance.darkScreen;

            }
        }
        else if (UI_MainMenu.instance != null)
        {
            if (UI_MainMenu.instance.fadeScreen != null)
            {
                darkScreen = UI_MainMenu.instance.fadeScreen;
            }
        }
        darkScreen.FadeOut();
        charactorAnim.gameObject.SetActive(true);
        charactorAnim.GetComponent<RectTransform>().localPosition = new Vector3(left, charactorAnim.GetComponent<RectTransform>().localPosition.y);
    }

    public void LoadingUpdate(float _progress)
    {
        float tarX = Mathf.Lerp(left, right, _progress);
        float posX = Mathf.SmoothDamp(charactorAnim.GetComponent<RectTransform>().localPosition.x, tarX, ref curSpeed, smoothTime, maxSpeed);
        charactorAnim.GetComponent<RectTransform>().localPosition = new Vector3(posX, charactorAnim.GetComponent<RectTransform>().localPosition.y);
    }

    public void LoadingEnd()
    {
        charactorAnim.gameObject.SetActive(false);
    }

    public bool IsAnimFinish()
    {
        float eclipse = 300f;
        return Mathf.Abs(charactorAnim.GetComponent<RectTransform>().localPosition.x - right) < eclipse;
    }
}
