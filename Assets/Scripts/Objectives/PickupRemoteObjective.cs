using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupRemoteObjective : Objective
{

    public Collider sofa;

    public override void Update() {
        base.Update();
        Player player = GameObject.Find("Player").GetComponent<Player>();
        if(player.currentPickupableObject != null){
            if(player.currentPickupableObject is TVRemote){
                sofa.enabled = true;
                CompleteObjective();
            }
        }
    }
}
