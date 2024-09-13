using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupableObject : InteractableObject
{
    public override void Interacted()
    {
        PickUp();
    }
    public abstract void Use();
    public void PickUp()
    {
        GameObject.Find("Player").GetComponent<Player>().SetRightHandObject(this);
    }

    public void Drop()
    {
        GameObject.Find("Player").GetComponent<Player>().RemoveRightHandObject();
    }
}

