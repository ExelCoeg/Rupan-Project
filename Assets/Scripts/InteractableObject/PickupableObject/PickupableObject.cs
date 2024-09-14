using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupableObject : InteractableObject
{
    public Transform originalParent;
    public bool isUsable;
    public override void Interacted()
    {
        PickUp();
    }
    public abstract void Use();
    public void PickUp()
    {
        GameObject.Find("Player").GetComponent<Player>().SetRightHandObject(this);
    }


    public virtual void ReturnToOriginalPosition()
    {
        if(originalParent == null) return;
        transform.SetParent(originalParent);
        transform.position = originalParent.position;
        transform.rotation = originalParent.rotation;
    }
    public void Drop()
    {
        GameObject.Find("Player").GetComponent<Player>().RemoveRightHandObject();
    }
    public void EnableUsable(){
        isUsable = true;
    }
    public void DisableUsable(){
        isUsable = false;
    }
}

