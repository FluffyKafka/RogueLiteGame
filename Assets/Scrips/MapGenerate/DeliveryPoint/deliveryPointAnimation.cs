using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryPointAnimation : MonoBehaviour
{
    public void AnimFinishTrigger()
    {
        GetComponentInParent<DeliveryPoint>().DeliverPlayerToPoint();
        GetComponent<Animator>().SetTrigger("Active");
    }
    public void TiggerActive()
    {
        GetComponent<Animator>().SetTrigger("Active");
    }
}
