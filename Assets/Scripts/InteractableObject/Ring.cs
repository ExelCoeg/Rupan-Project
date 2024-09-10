using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : InteractableObject
{
    public override void Interacted()
    {
        GetComponentInParent<SearchRings>().ringsFound++;
        Destroy(gameObject);
    }
}
