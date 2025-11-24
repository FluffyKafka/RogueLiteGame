using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryPoint : MonoBehaviour
{
    private Animator anim;
    public bool isChosen;
    public Transform playerTargetTransform;
    private Transform nextTargetTransform;
    private bool isPlayer;
    public bool isHolding;
    public bool isActive;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        isPlayer = false;
        isHolding = false;
        isActive = false;
    }

    public void Choose(bool _isChosen)
    {
        if(!isActive)
        {
            return;
        }

        isChosen = _isChosen;
        if (_isChosen)
        {
            anim.speed *= GameManager.instance.pauseTimeSpeedDivider;
        }
        else
        {
            anim.speed /= GameManager.instance.pauseTimeSpeedDivider;
        }
        anim.SetBool("Chosen", _isChosen);
    }

    public void Deliver(Transform _target)
    {
        anim.SetTrigger("Deliver");
        nextTargetTransform = _target;
    }

    public void DeliverPlayerToPoint()
    {
        PlayerManager.instance.player.transform.position = nextTargetTransform.position;
        nextTargetTransform = null;
        isHolding = false;
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if(_collision.GetComponent<Player>() != null)
        {
            PlayerManager.instance.player.fx.CreatePopUpText("°´T´«ËÍ");
            isPlayer = true;

            if(!isActive)
            {
                isActive = true;
                anim.SetTrigger("Active");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.GetComponent<Player>() != null)
        {
            isPlayer = false;
            isHolding = false;
        }
    }

    private void Update()
    {
        if (isPlayer && !isHolding)
        {
            if (Input.GetKeyDown(KeyCode.T) && GameManager.instance.CanPause())
            {
                isHolding = true;
                UI.instance.mapUI.GetComponent<MapUI>().SetOriginDeliveryPoint(this);
            }
        }
    }
}
