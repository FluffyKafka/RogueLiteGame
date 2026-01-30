using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Door : MonoBehaviour, IPlayerEnterableo
{
    [SerializeField] public Transform otherDoor;

    public void Enter(Player _player)
    {
        _player.transform.position = otherDoor.position;
    }
}
