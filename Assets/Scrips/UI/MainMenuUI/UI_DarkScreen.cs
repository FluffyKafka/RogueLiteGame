using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DarkScreen : MonoBehaviour
{
    private Animator anim;
    [SerializeField] public float fadeDuration;
    public bool isAnimFinish;

    private void Start()
    {
        anim = GetComponent<Animator>();
        isAnimFinish = false;
    }

    public void FadeOut()
    {
        anim.SetTrigger("FadeOut");
        isAnimFinish = false;
    }

    public void FadeIn()
    {
        anim.SetTrigger("FadeIn");
        isAnimFinish = false;
    }

    public void AnimFinishTrigger()
    {
        isAnimFinish = true;
    }
}
