using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sofa : InteractableObject
{
    public override void Interacted()
    {
        //player sit
        GameObject.Find("Player").GetComponent<Player>().PlayTimeline("PlayerSit");
    }

}
