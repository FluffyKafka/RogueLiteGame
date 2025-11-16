using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemyCheck : MonoBehaviour
{

    [SerializeField] private float delay;
    [SerializeField] private float enemyCheckRaduis;
    [SerializeField] private LayerMask whatIsEnemy;
    private Coroutine exitBattleStateRoutine = null;

    [Header("Test")]
    public bool isInBossFight = false;
    [SerializeField] private bool isBattle = false;
    [SerializeField] private float bgmVolume = 1;

    public void SetIsBattle(bool _isBattle)
    {
        if(isBattle && !_isBattle)
        {
            BattleState_Exit();
        }
        else if(!isBattle && _isBattle)
        {
            BattleState_Enter();
        }

        isBattle = _isBattle;

        if (exitBattleStateRoutine != null)
        {
            StopCoroutine(exitBattleStateRoutine);
            exitBattleStateRoutine = null;
        }
    }

    private void ExitBattleStateAfterDelay()
    {
        if (isBattle && exitBattleStateRoutine == null)
        {
            exitBattleStateRoutine = StartCoroutine(ExitBattleStateAfterDelay_Helper());
        }
    }
    private IEnumerator ExitBattleStateAfterDelay_Helper()
    {
        yield return new WaitForSeconds(delay);
        BattleState_Exit();
        isBattle = false;
        exitBattleStateRoutine = null;
    }

    private void Update()
    {
        if (isBattle && exitBattleStateRoutine == null)
        {
            if(!IsEnemyNearBy() && !isInBossFight)
            {
                ExitBattleStateAfterDelay();
            }
        }

        if(exitBattleStateRoutine != null)
        {
            bgmVolume -= (1/delay)*Time.deltaTime;
            SceneAudioManager.instance.bgm.SetCurrentBGMVolume(bgmVolume);
        }
        else
        {
            bgmVolume = 1;
            SceneAudioManager.instance.bgm.SetCurrentBGMVolume(bgmVolume);
        }
    }

    private bool IsEnemyNearBy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, enemyCheckRaduis, whatIsEnemy);
        foreach (Collider2D hit in hits)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                return true;
            }
        }
        return false;
        
    }

    private void BattleState_Enter()
    {
        SceneAudioManager.instance.bgm.SetRandomBGMOfType(BGMType.Fight);
        GameManager.instance.isBattle = true;
    }
    private void BattleState_Exit()
    {
        SceneAudioManager.instance.bgm.SetRandomBGMOfType(BGMType.Normal);
        GameManager.instance.isBattle = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, enemyCheckRaduis);
    }
}
