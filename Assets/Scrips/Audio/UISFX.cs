using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AudioSystem
{

}
public class UISFX : SoundManagerBase
{
    [Header("Sounds")]
    [SerializeField] private AudioSource buttonClick_SS;
    [SerializeField] private AudioSource craft_SS;
    [SerializeField] private AudioSource equip_SS;
    [SerializeField] private AudioSource buy_SS;
    [SerializeField] private AudioSource upgrade_SS;
    [SerializeField] private AudioSource discardInventory_SS;
    [SerializeField] private AudioSource communicating_SS;

    public Sound buttonClick { get; private set; }
    public Sound craft { get; private set; }
    public Sound equip { get; private set; }
    public Sound buy { get; private set; }
    public Sound upgrade { get; private set; }
    public Sound discardInventory { get; private set; }
    public Sound communicating { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        buttonClick = GetSound(buttonClick_SS, true);
        craft = GetSound(craft_SS, true);
        equip = GetSound(equip_SS, true);
        buy = GetSound(buy_SS, true);
        upgrade = GetSound(upgrade_SS, true);
        discardInventory = GetSound(discardInventory_SS, true);
        communicating = GetSound(communicating_SS, true);
    }
}
