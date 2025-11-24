using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    [SerializeField] private DamageDataSerializable damageData;
    [SerializeField] private CheckPoint checkPoint;
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if(_collision.GetComponent<Enemy>() != null)
        {
            _collision.GetComponent<Enemy>().Die();
        }
        else if (_collision.GetComponent<Player>() != null)
        {
            _collision.GetComponent<Player>().cs.TakeDamage(damageData.GetDamageData(), transform);

            CheckPoint check = checkPoint;
            if(check == null || !check.isCheck)
            {
                check = GameManager.instance.lastCheckPoint;
            }

            if(check != null)
            {
                _collision.GetComponent<Player>().transform.position = check.transform.position;
            }
            else
            {
                _collision.GetComponent<Player>().Die();
            }
        }
    }
}
