using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAnimation : MonoBehaviour
{
    protected Enemy enemy;
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    public virtual void AnimFinishTrigger()
    {
        enemy.AnimFinishTriggerCalled();
    }

    protected virtual void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackValidCheck.position, enemy.attackValidCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                float damage = enemy.cs.DoDamageTo_PrimaryAttack(hit.GetComponent<Player>());
            }
        }
    }

    protected virtual void OpenCounterAttackWindowTrigger()
    {
        enemy.OpenCounterAttackWindow();
        Debug.Log("Open");
    }

    protected virtual void CloseCounterAttackWindowTrigger()
    {
        enemy.CloseCounterAttackWindow();
    }
}
