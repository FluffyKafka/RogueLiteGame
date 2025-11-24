using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    public string id;
    public bool isCheck = false;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
    }

    [ContextMenu("Generate checkpoint id")]
    protected void GenerateId()
    {
        id = System.Guid.NewGuid().ToString();
    }

    protected virtual void OnTriggerEnter2D(Collider2D _collision)
    {
        if(_collision.GetComponent<Player>() != null)
        {
            Check();
            GameManager.instance.lastCheckPoint = this;
        }
    }

    public void Check()
    {
        anim.SetBool("isCheck", true);

        if (SceneAudioManager.instance != null)
        {
            if (!isCheck)
            {
                SceneAudioManager.instance.itemSFX.torchLighting.Play(transform);
                GetComponentInChildren<AreaSound>().isActivate = true;
            }
        }

        isCheck = true;
    }
}
