using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveTorch : CheckPoint, ISaveManager
{
    [SerializeField] private Animator backAnim;
    [SerializeField] private Animator anim;
    [SerializeField] private string loadSceneName = string.Empty;
    private bool isLighting = false;

    private bool isSaving = false;

    protected override void Start()
    {
        
    }
    public void LoadData(GameData _data)
    {
        
    }

    public void SaveData(ref GameData _data)
    {
        if (isSaving)
        {
            if (loadSceneName == string.Empty)
            {
                _data.currentSceneName = SceneManager.GetActiveScene().name;
            }
            else
            {
                _data.currentSceneName = loadSceneName;
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D _collision)
    {
        base.OnTriggerEnter2D(_collision);

        if(isLighting)
        {
            return;
        }

        Player player = _collision.GetComponent<Player>();
        if(player != null)
        {
            backAnim.SetTrigger("Fire");
            anim.SetTrigger("Fire");

            if(SceneAudioManager.instance != null)
            {
                if(!isLighting)
                {
                    SceneAudioManager.instance.itemSFX.torchLighting.Play(transform);
                    GetComponentInChildren<AreaSound>().isActivate = true;
                    isLighting = true;
                }
            }
            
            isSaving = true;
            SaveManager.instance.SaveGame();
            isSaving = false;
        }
    }
}
