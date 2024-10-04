using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : InteractableObject
{
    public override void Awake()
    {
        base.Awake();
    }
    public override void Interacted()
    {
        SoundManager.instance.PlaySound2D("Collected_Ring");
        GetComponentInParent<SearchRings>().ringsFound++;
        Destroy(gameObject);
    }
}
