using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deliveryPointAnimation : MonoBehaviour
{
    public void AnimFinishTrigger()
    {
        GetComponentInParent<DeliveryPoint>().DeliverPlayerToPoint();
    }
}
